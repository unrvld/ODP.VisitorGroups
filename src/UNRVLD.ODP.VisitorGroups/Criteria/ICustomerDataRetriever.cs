﻿using UNRVLD.ODP.VisitorGroups.GraphQL.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public interface ICustomerDataRetriever
    { 
        Customer? GetCustomerInfo(string vuidValue, string? endpointKey = null);

        bool IsInAudience(string vuidValue, string audience, string? endpointKey = null);
    }
}