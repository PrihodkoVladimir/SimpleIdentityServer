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

using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.Uma.Core.Models;
using SimpleIdentityServer.Uma.Core.Repositories;
using SimpleIdentityServer.Uma.EF.Extensions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Uma.EF.Repositories
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly SimpleIdServerUmaContext _context;

        public TicketRepository(SimpleIdServerUmaContext context)
        {
            _context = context;
        }

        public async Task<Ticket> Get(string id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
            return ticket == null ? null : ticket.ToDomain();
        }

        public async Task<IEnumerable<Ticket>> Get(IEnumerable<string> ids)
        {
            return await _context.Tickets.Where(t => ids.Contains(t.Id)).Select(t => t.ToDomain()).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> Insert(IEnumerable<Ticket> tickets)
        {
            if (tickets == null)
            {
                throw new ArgumentNullException(nameof(tickets));
            }

            using (var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                try
                {
                    foreach(var ticket in tickets)
                    {
                        _context.Tickets.Add(ticket.ToModel());
                    }

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> Insert(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            using (var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                try
                {
                    _context.Tickets.Add(ticket.ToModel());
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}