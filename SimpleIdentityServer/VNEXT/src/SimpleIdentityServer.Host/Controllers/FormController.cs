﻿using System.Web;
using Microsoft.AspNet.Mvc;
using SimpleIdentityServer.Host.ViewModels;

namespace SimpleIdentityServer.Api.Controllers
{
    public class FormController : Controller
    {
        public ActionResult Index(dynamic parameters)
        {
            var queryStringValue = Request.QueryString.Value;            
            var queryString = HttpUtility.ParseQueryString(queryStringValue);
            var idToken = queryString[Core.Constants.StandardAuthorizationResponseNames.IdTokenName];
            var accessToken = queryString[Core.Constants.StandardAuthorizationResponseNames.AccessTokenName];
            var authorizationCode = queryString[Core.Constants.StandardAuthorizationResponseNames.AuthorizationCodeName];
            var state = queryString[Core.Constants.StandardAuthorizationResponseNames.StateName];
            var redirectUri = queryString["redirect_uri"];
            return View(new FormViewModel
            {
                AccessToken = accessToken,
                AuthorizationCode = authorizationCode,
                IdToken = idToken,
                RedirectUri = redirectUri,
                State = state
            });
        }
    }
}