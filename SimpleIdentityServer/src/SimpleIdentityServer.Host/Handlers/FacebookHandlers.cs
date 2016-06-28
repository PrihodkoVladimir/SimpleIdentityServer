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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Host.Handlers
{
    internal class FacebookHandler : OAuthHandler<FacebookOptions>
    {
        public FacebookHandler(HttpClient httpClient)
            : base(httpClient)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity, 
            AuthenticationProperties properties, 
            OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            if (Options.SendAppSecretProof)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
            }
            if (Options.Fields.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);

            var identifier = FacebookHelper.GetId(payload);
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var ageRangeMin = FacebookHelper.GetAgeRangeMin(payload);
            if (!string.IsNullOrEmpty(ageRangeMin))
            {
                identity.AddClaim(new Claim("urn:facebook:age_range_min", ageRangeMin, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var ageRangeMax = FacebookHelper.GetAgeRangeMax(payload);
            if (!string.IsNullOrEmpty(ageRangeMax))
            {
                identity.AddClaim(new Claim("urn:facebook:age_range_max", ageRangeMax, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var birthday = FacebookHelper.GetBirthday(payload);
            if (!string.IsNullOrEmpty(birthday))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate, birthday, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var email = FacebookHelper.GetEmail(payload);
            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var firstName = FacebookHelper.GetFirstName(payload);
            if (!string.IsNullOrEmpty(firstName))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName, firstName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var gender = FacebookHelper.GetGender(payload);
            if (!string.IsNullOrEmpty(gender))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender, gender, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var lastName = FacebookHelper.GetLastName(payload);
            if (!string.IsNullOrEmpty(lastName))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName, lastName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var link = FacebookHelper.GetLink(payload);
            if (!string.IsNullOrEmpty(link))
            {
                identity.AddClaim(new Claim("urn:facebook:link", link, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var location = FacebookHelper.GetLocation(payload);
            if (!string.IsNullOrEmpty(location))
            {
                identity.AddClaim(new Claim("urn:facebook:location", location, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var locale = FacebookHelper.GetLocale(payload);
            if (!string.IsNullOrEmpty(locale))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale, locale, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var middleName = FacebookHelper.GetMiddleName(payload);
            if (!string.IsNullOrEmpty(middleName))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName, middleName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var name = FacebookHelper.GetName(payload);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var timeZone = FacebookHelper.GetTimeZone(payload);
            if (!string.IsNullOrEmpty(timeZone))
            {
                identity.AddClaim(new Claim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo, timeZone, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        private string GenerateAppSecretProof(string accessToken)
        {
            using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(Options.AppSecret)))
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                var builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        protected override string FormatScope()
        {
            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
            // https://developers.facebook.com/docs/reference/dialogs/oauth
            // http://tools.ietf.org/html/rfc6749#section-3.3
            return string.Join(",", Options.Scope);
        }
    }
}
