#region copyright
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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.DataAccess.SqlServer.Repositories;

namespace SimpleIdentityServer.DataAccess.SqlServer
{
    public static class SimpleIdentityServerSqlServerExtensions
    {
        public static IServiceCollection AddSimpleIdentityServerSqlServer(
            this IServiceCollection serviceCollection, 
            string connectionString)
        {
            RegisterServices(serviceCollection);
            serviceCollection.AddEntityFramework()
                .AddDbContext<SimpleIdentityServerContext>(options =>
                    options.UseSqlServer(connectionString));
            return serviceCollection;
        }

        public static IServiceCollection AddSimpleIdentityServerSqlLite(
            this IServiceCollection serviceCollection,
            string connectionString)
        {
            RegisterServices(serviceCollection);
            serviceCollection.AddEntityFramework()
                .AddDbContext<SimpleIdentityServerContext>(options =>
                    options.UseSqlite(connectionString));
            return serviceCollection;
        }

        public static IServiceCollection AddSimpleIdentityServerPostgre(
            this IServiceCollection serviceCollection,
            string connectionString) {
            RegisterServices(serviceCollection);
            serviceCollection.AddEntityFramework()
                .AddDbContext<SimpleIdentityServerContext>(options =>
                    options.UseNpgsql(connectionString));
            return serviceCollection;
        }

        public static IServiceCollection AddSimpleIdentityServerInMemory(
            this IServiceCollection serviceCollection)
        {
            RegisterServices(serviceCollection);
            serviceCollection.AddEntityFramework()
                .AddDbContext<SimpleIdentityServerContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return serviceCollection;
        }

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITranslationRepository, TranslationRepository>();
            serviceCollection.AddTransient<IResourceOwnerRepository, ResourceOwnerRepository>();
            serviceCollection.AddTransient<IScopeRepository, ScopeRepository>();
            serviceCollection.AddTransient<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
            serviceCollection.AddTransient<IClientRepository, ClientRepository>();
            serviceCollection.AddTransient<IConsentRepository, ConsentRepository>();
            serviceCollection.AddTransient<IGrantedTokenRepository, GrantedTokenRepository>();
            serviceCollection.AddTransient<IJsonWebKeyRepository, JsonWebKeyRepository>();
            serviceCollection.AddTransient<IConfirmationCodeRepository, ConfirmationCodeRepository>();
            serviceCollection.AddTransient<IClaimRepository, ClaimRepository>();
        }
    }
}
