﻿#region copyright
// Copyright 2016 Habart Thierry
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

namespace SimpleIdentityServer.Uma.Common
{
    public static class ResourceSetResponseNames
    {
        public const string Id = "_id";
        public const string Name = "name";
        public const string Uri = "uri";
        public const string Type = "type";
        public const string Scopes = "scopes";
        public const string IconUri = "icon_uri";
    }

    public static class AddResourceSetResponseNames
    {
        public const string Id = "_id";
        public const string UserAccessPolicyUri = "user_access_policy_uri";
    }

    public static class UpdateSetResponseNames
    {
        public const string Id = "_id";
    }

    public static class ClaimNames
    {
        public const string Type = "type";
        public const string Value = "value";
    }

    public static class PolicyRuleNames
    {
        public const string Id = "id";
        public const string ClientIdsAllowed = "clients";
        public const string IsResourceOwnerConsentNeeded = "consent_needed";
        public const string Scopes = "scopes";
        public const string Script = "script";
        public const string Claims = "claims";
    }

    public static class PolicyNames
    {
        public const string Id = "id";
        public const string ResourceSetIds = "resource_set_ids";
        public const string Rules = "rules";
    }

    public static class AddPolicyResponseNames
    {
        public const string PolicyId = "policy";
    }

    public static class PostAddResourceSetNames
    {
        public const string ResourceSets = "resources";
    }


    public static class PostPermissionNames
    {
        public const string ResourceSetId = "resource_set_id";
        public const string Scopes = "scopes";
    }

    public static class AddPermissionResponseNames
    {
        public const string TicketId = "ticket_id";
    }

    public static class AddPermissionsResponseNames
    {
        public const string TicketIds = "ticket_ids";
    }

    public static class PostClaimTokenNames
    {
        public const string Format = "format";
        public const string Token = "token";
    }

    public static class PostAuthorizationNames
    {
        public const string Rpt = "rpt";
        public const string TicketId = "ticket";
        public const string ClaimTokens = "claim_tokens";
    }

    public static class PostIntrospectionNames
    {
        public const string Rpts = "rpts";
    }

    public static class AuthorizationResponseNames
    {
        public const string Rpt = "rpt";
    }

    public static class BulkAuthorizationResponseNames
    {
        public const string Rpts = "rpts";
    }

    public static class IntrospectPermissionNames
    {
        public const string ResourceSetIdName = "resource_set_id";
        public const string ScopesName = "scopes";
        public const string ExpirationName = "exp";
    }

    public static class IntrospectNames
    {
        public const string ActiveName = "active";
        public const string ExpirationName = "exp";
        public const string IatName = "iat";
        public const string NbfName = "nbf";
        public const string PermissionsName = "permissions";
    }

    public static class ConfigurationResponseNames
    {
        public const string Version = "version";
        public const string Issuer = "issuer";
        public const string PatProfilesSupported = "pat_profiles_supported";
        public const string AatProfilesSupported = "aat_profiles_supported";
        public const string RptProfilesSupported = "rpt_profiles_supported";
        public const string PatGrantTypesSupported = "pat_grant_types_supported";
        public const string AatGrantTypesSupported = "aat_grant_types_supported";
        public const string ClaimTokenProfilesSupported = "claim_token_profiles_supported";
        public const string UmaProfilesSupported = "uma_profiles_supported";
        public const string DynamicClientEndPoint = "dynamic_client_endpoint";
        public const string TokenEndPoint = "token_endpoint";
        public const string AuthorizationEndPoint = "authorization_endpoint";
        public const string RequestingPartyClaimsEndPoint = "requesting_party_claims_endpoint";
        public const string IntrospectionEndPoint = "introspection_endpoint";
        public const string ResourceSetRegistrationEndPoint = "resource_set_registration_endpoint";
        public const string PermissionRegistrationEndPoint = "permission_registration_endpoint";
        public const string RptEndPoint = "rpt_endpoint";
        public const string PolicyEndPoint = "policy_endpoint";
        public const string ScopeEndPoint = "scope_endpoint";
    }

    public static class ScopeResponseNames
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string IconUri = "icon_uri";
    }
}
