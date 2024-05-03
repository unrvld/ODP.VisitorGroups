﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Edges<T>
    {
        [JsonProperty("edges")]
        public List<Edge<T>> EdgeItems { get; set; } = [];
    }
}
