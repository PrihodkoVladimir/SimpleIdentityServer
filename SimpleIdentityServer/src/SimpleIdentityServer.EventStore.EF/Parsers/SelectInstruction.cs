﻿#region copyright
// Copyright 2017 Habart Thierry
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

using SimpleIdentityServer.EventStore.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.EventStore.EF.Parsers
{
    public class SelectInstruction
    {
        private readonly string _filter;

        public SelectInstruction() { }

        public SelectInstruction(string filter)
        {
            _filter = filter;
        }

        public WhereInstruction WhereInst { get; set; }
        public GroupByInstruction GroupByInst { get; set; }

        public IEnumerable<dynamic> Evaluate<TEntity>(IQueryable<TEntity> elts)
        {
            if (GroupByInst != null)
            {
                return GroupByInst.Evaluate(elts);
            }

            if (WhereInst != null)
            {
                elts = WhereInst.Evaluate(elts);
            }

            var result = elts.Select(_filter);
            return result;
        }
    }
}
