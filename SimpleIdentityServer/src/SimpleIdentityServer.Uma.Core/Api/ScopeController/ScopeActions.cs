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

using SimpleIdentityServer.Uma.Core.Api.ScopeController.Actions;
using SimpleIdentityServer.Uma.Core.Models;
using SimpleIdentityServer.Uma.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Uma.Core.Api.ScopeController
{
    public interface IScopeActions
    {
        Task<bool> InsertScope(AddScopeParameter addScopeParameter);
        Task<Scope> GetScope(string scopeId);
        Task<IEnumerable<string>> GetScopes();
        Task<bool> UpdateScope(UpdateScopeParameter updateScopeParameter);
        Task<bool> DeleteScope(string scopeId);
    }

    internal class ScopeActions : IScopeActions
    {
        private readonly IGetScopeAction _getScopeAction;
        private readonly IInsertScopeAction _insertScopeAction;
        private readonly IDeleteScopeAction _deleteScopeAction;
        private readonly IGetScopesAction _getScopesAction;
        private readonly IUpdateScopeAction _updateScopeAction;

        public ScopeActions(
            IGetScopeAction getScopeAction,
            IInsertScopeAction insertScopeAction,
            IUpdateScopeAction updateScopeAction,
            IGetScopesAction getScopesAction,
            IDeleteScopeAction deleteScopeAction)
        {
            _getScopeAction = getScopeAction;
            _insertScopeAction = insertScopeAction;
            _getScopesAction = getScopesAction;
            _updateScopeAction = updateScopeAction;
            _deleteScopeAction = deleteScopeAction;
        }

        public Task<bool> InsertScope(AddScopeParameter addScopeParameter)
        {
            return _insertScopeAction.Execute(addScopeParameter);
        }

        public Task<Scope> GetScope(string scopeId)
        {
            return _getScopeAction.Execute(scopeId);
        }

        public Task<IEnumerable<string>> GetScopes()
        {
            return _getScopesAction.Execute();
        } 

        public Task<bool> UpdateScope(UpdateScopeParameter updateScopeParameter)
        {
            return _updateScopeAction.Execute(updateScopeParameter);
        }

        public Task<bool> DeleteScope(string scopeId)
        {
            return _deleteScopeAction.Execute(scopeId);
        }
    }
}
