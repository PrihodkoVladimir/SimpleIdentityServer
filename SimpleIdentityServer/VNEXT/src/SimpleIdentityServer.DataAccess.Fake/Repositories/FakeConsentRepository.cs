﻿using System.Collections.Generic;

using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Repositories;

using System.Linq;
using System;
using SimpleIdentityServer.DataAccess.Fake.Extensions;

namespace SimpleIdentityServer.DataAccess.Fake.Repositories
{
    public class FakeConsentRepository : IConsentRepository
    {
        public List<Consent> GetConsentsForGivenUser(string subject)
        {
            var result = FakeDataSource.Instance().Consents
                .Where(c => c.ResourceOwner.Id == subject);
            if (!result.Any())
            {
                return null;
            }

            return result.Select(c => c.ToBusiness()).ToList();
        }

        public Consent InsertConsent(Consent record)
        {
            var newRecord = record.ToFake();
            FakeDataSource.Instance().Consents.Add(newRecord);
            return null;
        }
        
        public bool DeleteConsent(Consent record)
        {
            var consentToBeRemoved = FakeDataSource.Instance().Consents.FirstOrDefault(c => c.Id == record.Id);
            if (consentToBeRemoved == null)
            {
                return false;
            }

            FakeDataSource.Instance().Consents.Remove(consentToBeRemoved);
            return true;
        }
    }
}
