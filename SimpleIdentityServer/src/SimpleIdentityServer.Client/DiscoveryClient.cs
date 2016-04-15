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

using SimpleIdentityServer.Client.DTOs.Response;
using SimpleIdentityServer.Client.Operations;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Client
{
    public interface IDiscoveryClient
    {
        /// <summary>
        /// Get information about open-id contract synchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        DiscoveryInformation GetDiscoveryInformation(string discoveryDocumentationUrl);

        /// <summary>
        /// Get information about open-id contract synchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        DiscoveryInformation GetDiscoveryInformation(Uri discoveryDocumentationUrl);

        /// <summary>
        /// Get information about open-id contract asynchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        Task<DiscoveryInformation> GetDiscoveryInformationAsync(string discoveryDocumentationUrl);

        /// <summary>
        /// Get information about open-id contract asynchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        Task<DiscoveryInformation> GetDiscoveryInformationAsync(Uri discoveryDocumentationUrl);
    }

    internal class DiscoveryClient : IDiscoveryClient
    {
        private readonly IGetDiscoveryOperation _getDiscoveryOperation;

        #region Constructor

        public DiscoveryClient(IGetDiscoveryOperation getDiscoveryOperation)
        {
            _getDiscoveryOperation = getDiscoveryOperation;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get information about open-id contract synchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        public DiscoveryInformation GetDiscoveryInformation(string discoveryDocumentationUrl)
        {
            return _getDiscoveryOperation.ExecuteAsync(discoveryDocumentationUrl)
                .Result;
        }

        /// <summary>
        /// Get information about open-id contract synchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        public DiscoveryInformation GetDiscoveryInformation(Uri discoveryDocumentationUrl)
        {
            if (discoveryDocumentationUrl == null)
            {
                throw new ArgumentNullException(nameof(discoveryDocumentationUrl));
            }

            var absoluteUri = discoveryDocumentationUrl.AbsoluteUri;
            return _getDiscoveryOperation.ExecuteAsync(absoluteUri)
                .Result;
        }
        
        /// <summary>
        /// Get information about open-id contract asynchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        public async Task<DiscoveryInformation> GetDiscoveryInformationAsync(string discoveryDocumentationUrl)
        {
            return await _getDiscoveryOperation.ExecuteAsync(discoveryDocumentationUrl);
        }

        /// <summary>
        /// Get information about open-id contract asynchronously.
        /// </summary>
        /// <param name="discoveryDocumentationUrl">Absolute uri of the open-id contract</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown when parameter is not a valid URI</exception>
        /// <returns>Open-id contract</returns>
        public async Task<DiscoveryInformation> GetDiscoveryInformationAsync(Uri discoveryDocumentationUrl)
        {
            if (discoveryDocumentationUrl == null)
            {
                throw new ArgumentNullException(nameof(discoveryDocumentationUrl));
            }

            var absoluteUri = discoveryDocumentationUrl.AbsoluteUri;
            return await _getDiscoveryOperation.ExecuteAsync(absoluteUri);
        }

        #endregion
    }
}