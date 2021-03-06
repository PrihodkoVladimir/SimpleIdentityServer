﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Authentication.Middleware;
using SimpleIdentityServer.Authentication.Middleware.Extensions;
using SimpleIdentityServer.Configuration.Client;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Services;
using System.Collections.Generic;
using WebApiContrib.Core.Storage;
using WebApiContrib.Core.Storage.InMemory;

namespace SimpleIdentityServer.Startup
{
    public class Startup
    {
        private AuthenticationMiddlewareOptions _authenticationOptions;
        private IdentityServerOptions _options;
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Load all the configuration information from the "json" file & the environment variables.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _authenticationOptions = new AuthenticationMiddlewareOptions
            {
                IdServer = new IdServerOptions
                {
                    ExternalLoginCallback = "/Authenticate/LoginCallback",
                    LoginUrls = new List<string>
                    {
                        "/Authenticate",
                        "/Authenticate/ExternalLogin",
                        "/Authenticate/OpenId",
                        "/Authenticate/LocalLoginOpenId",
                        "/Authenticate/LocalLogin",
                        "/Authenticate/ExternalLoginOpenId"
                    }
                },
                ConfigurationEdp = new ConfigurationEdpOptions
                {
                    ConfigurationUrl = Configuration["ConfigurationEdp:Url"],
                    ClientId = Configuration["ConfigurationEdp:ClientId"],
                    ClientSecret = Configuration["ConfigurationEdp:ClientSecret"],
                    Scopes = new List<string>
                    {
                        "display_configuration"
                    }
                }
            };
            var twoFactorServiceStore = new TwoFactorServiceStore();
            var factory = new SimpleIdServerConfigurationClientFactory();
            twoFactorServiceStore.Add(new DefaultTwilioSmsService(factory, Configuration["ConfigurationEdp:Url"]));
            twoFactorServiceStore.Add(new DefaultEmailService(factory, Configuration["ConfigurationEdp:Url"]));
            _options = new IdentityServerOptions
            {
                IsDeveloperModeEnabled = false,
                DataSource = new DataSourceOptions
                {
                    IsOpenIdDataMigrated = true,
                    IsEvtStoreDataMigrated = true,
                },
                Logging = new LoggingOptions
                {
                    ElasticsearchOptions = new ElasticsearchOptions(),
                    FileLogOptions = new FileLogOptions()
                },
                Authenticate = new AuthenticateOptions
                {
                    CookieName = Constants.CookieName
                },
                Scim = new ScimOptions
                {
                    IsEnabled = true,
                    EndPoint = "http://localhost:5555/"
                },
                TwoFactorServiceStore = twoFactorServiceStore
            };

            var openIdType = Configuration["Db:OpenIdType"];
            var evtStoreType = Configuration["Db:EvtStoreType"];
            if (string.Equals(openIdType, "SQLSERVER", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.OpenIdDataSourceType = DataSourceTypes.SqlServer;
                _options.DataSource.OpenIdConnectionString = Configuration["Db:OpenIdConnectionString"];
            }
            else if (string.Equals(openIdType, "SQLITE", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.OpenIdDataSourceType = DataSourceTypes.SqlLite;
                _options.DataSource.OpenIdConnectionString = Configuration["Db:OpenIdConnectionString"];
            }
            else if (string.Equals(openIdType, "POSTGRE", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.OpenIdDataSourceType = DataSourceTypes.Postgre;
                _options.DataSource.OpenIdConnectionString = Configuration["Db:OpenIdConnectionString"];
            }
            else
            {
                _options.DataSource.OpenIdDataSourceType = DataSourceTypes.InMemory;
            }

            if (string.Equals(evtStoreType, "SQLSERVER", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.EvtStoreDataSourceType = DataSourceTypes.SqlServer;
                _options.DataSource.EvtStoreConnectionString = Configuration["Db:EvtStoreConnectionString"];
            }
            else if (string.Equals(evtStoreType, "SQLITE", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.EvtStoreDataSourceType = DataSourceTypes.SqlLite;
                _options.DataSource.EvtStoreConnectionString = Configuration["Db:EvtStoreConnectionString"];
            }
            else if (string.Equals(evtStoreType, "POSTGRE", System.StringComparison.CurrentCultureIgnoreCase))
            {
                _options.DataSource.EvtStoreDataSourceType = DataSourceTypes.Postgre;
                _options.DataSource.EvtStoreConnectionString = Configuration["Db:EvtStoreConnectionString"];
            }
            else
            {
                _options.DataSource.EvtStoreDataSourceType = DataSourceTypes.InMemory;
            }

            bool isLogFileEnabled,
                isElasticSearchEnabled;
            if (bool.TryParse(Configuration["Log:File:Enabled"], out isLogFileEnabled))
            {
                _options.Logging.FileLogOptions.IsEnabled = isLogFileEnabled;
                if (isLogFileEnabled)
                {
                    _options.Logging.FileLogOptions.PathFormat = Configuration["Log:File:PathFormat"];
                }
            }
            
            if (bool.TryParse(Configuration["Log:Elasticsearch:Enabled"], out isElasticSearchEnabled))
            {
                _options.Logging.ElasticsearchOptions.IsEnabled = isElasticSearchEnabled;
                if (isElasticSearchEnabled)
                {
                    _options.Logging.ElasticsearchOptions.Url = Configuration["Log:Elasticsearch:Url"];
                }
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cachingDatabase = Configuration["Caching:Database"];
            var cachingConnectionPath = Configuration["Caching:ConnectionPath"];
            if (string.IsNullOrWhiteSpace(cachingDatabase))
            {
                cachingDatabase = "INMEMORY";
            }

            // 1. Configure the caching
            if (string.Equals(cachingDatabase, "REDIS", System.StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddStorage(opt => opt.UseRedis(o =>
                {
                    o.Configuration = Configuration[cachingConnectionPath + ":ConnectionString"];
                    o.InstanceName = Configuration[cachingConnectionPath + ":InstanceName"];
                }));
            }
            else if (string.Equals(cachingDatabase, "INMEMORY", System.StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddStorage(opt => opt.UseInMemory());
            }

            // 2. Add the dependencies needed to enable CORS
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            // 3. Configure the rate limitation
            /*
            services.Configure<RateLimitationOptions>(opt =>
            {
                opt.IsEnabled = true;
                opt.RateLimitationElements = new List<RateLimitationElement>
                {
                    new RateLimitationElement
                    {
                        Name = "PostToken",
                        NumberOfRequests = 20,
                        SlidingTime = 2000
                    }
                };
                opt.MemoryCache = new MemoryCache(new MemoryCacheOptions());
            });
            */

            // 4. Configure Simple identity server
            services.AddSimpleIdentityServer(_options);
            // 5. Enable logging
            services.AddLogging();
            // 6. Configure MVC
            services.AddMvc();
            // 7. Add authentication dependencies & configure it.
            services.AddAuthenticationMiddleware();
            services.AddAuthentication(opts => opts.SignInScheme = Constants.CookieName);
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("Connected", policy => policy.RequireAssertion((ctx) => {
                    return ctx.User.Identity != null && ctx.User.Identity.AuthenticationType == Constants.CookieName;
                }));
            });
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            //1 . Enable CORS.
            app.UseCors("AllowAll");
            // 2. Use static files.
            app.UseStaticFiles();
            // 3. Redirect error to custom pages.
            app.UseStatusCodePagesWithRedirects("~/Error/{0}");
            // 4. Enable cookie authentication.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = Host.Constants.TwoFactorCookieName
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = Authentication.Middleware.Constants.CookieName
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/Authenticate"),
                AuthenticationScheme = Constants.CookieName,
                CookieName = Constants.CookieName
            });
            // 5. Enable multi parties authentication.
            // app.UseAuthentication(_authenticationOptions);
            // 6. Enable SimpleIdentityServer
            app.UseSimpleIdentityServer(_options, loggerFactory);
            // 7. Configure ASP.NET MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute("Error401Route",
                    Host.Constants.EndPoints.Get401,
                    new
                    {
                        controller = "Error",
                        action = "Get401"
                    });
                routes.MapRoute("Error404Route",
                    Host.Constants.EndPoints.Get404,
                    new
                    {
                        controller = "Error",
                        action = "Get404"
                    });
                routes.MapRoute("Error500Route",
                    Host.Constants.EndPoints.Get500,
                    new
                    {
                        controller = "Error",
                        action = "Get500"
                    });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
