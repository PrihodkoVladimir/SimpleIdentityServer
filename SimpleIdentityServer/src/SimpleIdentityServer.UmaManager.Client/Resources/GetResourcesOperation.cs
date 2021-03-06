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

using Newtonsoft.Json;
using SimpleIdentityServer.UmaManager.Client.DTOs.Responses;
using SimpleIdentityServer.UmaManager.Client.Factory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.UmaManager.Client.Resources
{
    public interface IGetResourcesOperation
    {
        Task<List<ResourceResponse>> ExecuteAsync(
            Uri uri,
            string accessToken);
    }

    internal class GetResourcesOperation : IGetResourcesOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        #region Constructor

        public GetResourcesOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Public methods

        public async Task<List<ResourceResponse>> ExecuteAsync(
            Uri uri,
            string accessToken)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = _httpClientFactory.GetHttpClient();
            var httpResult = await httpClient.SendAsync(request);
            httpResult.EnsureSuccessStatusCode();
            var content = await httpResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<ResourceResponse>>(content);
        }

        #endregion
    }
}
