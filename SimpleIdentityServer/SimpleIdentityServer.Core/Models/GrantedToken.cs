﻿using System;
using System.Runtime.Serialization;
using SimpleIdentityServer.Core.Jwt;

namespace SimpleIdentityServer.Core.Models
{
    [DataContract]
    public class GrantedToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the identity token generated by scope or claims
        /// </summary>
        [DataMember(Name = "id_token")]
        public string IdToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the user information payload
        /// </summary>
        public JwsPayload UserInfoPayLoad { get; set; }

        /// <summary>
        /// Gets or sets the identity token payload
        /// </summary>
        public JwsPayload IdTokenPayLoad { get; set; }
    }
}
