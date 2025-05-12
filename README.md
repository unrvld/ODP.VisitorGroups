# Visitor Groups for Optimizely Data Platform


> If you are looking for documentation relevant to V1.* then please refer to the [legacy documentation](./docs/README-legacy.md).  This version supports Optimizely CMS 11 and 12.

[![ODP Visitor Groups](https://github.com/made-to-engage/ODP.VisitorGroups/actions/workflows/build-visitor-groups.yml/badge.svg?branch=main)](https://github.com/made-to-engage/ODP.VisitorGroups/actions/workflows/build-visitor-groups.yml)


## Features

Supports multiple ODP instances. 

Visitor Groups for:
- Real Time Segments
- Engagement Rank
- Order Likelihood
- Winback zone
- Customer Properties (Text/Numeric)
- Observation
  - Total Revenue
  - Order Count
  - Average Order Revenue

----

## Installation

Install the package directly from the Optimizely Nuget repository.

``` 
dotnet add package UNRVLD.ODP.VisitorGroups
```


## Configuration

*Startup.cs*
``` c#
// Adds the registration for visitor groups
services.AddODPVisitorGroups();
```

*appsettings.json*
All settings are optional, apart from the PrivateApiKey
``` json
{
   "EPiServer": {
      //Other config
    "OdpVisitorGroupOptions": {
        "OdpCookieName": "vuid",
        "CacheTimeoutSeconds": 10,
        "SchemaCacheTimeoutSeconds": 86400,
        "PopulationEstimateCacheTimeoutSeconds": 4320,
        "OdpEndpoints": [
          {
              "Name": "US",  //Unique name for the instance
              "BaseEndPoint": "https://api.zaius.com",
              "PrivateApiKey": "..."
          },
          {
              "Name": "EU",
              "BaseEndPoint": "https://api.zaius.eu",
              "PrivateApiKey": "..."
          }]
    }
   }
}
```
 ---
 ## Version History

 |Version| Details|
 |:---|:---------------|
 |1.0|Initial Release|
 |1.1|Add new criterion (Customer Properties)<br/>Support for .net6|
 |1.1.1|Refactor code to deal with HttpContect access issue|
 |1.1.2|Ensure Visitor Group UI doesn’t break if invalid or missing API key|
 |1.2.0|Adds counts to the segments, indicating the number of matching profiles|
 |1.3.0|Load the RTS segment count async|
 |1.4.0|Removed support for .NET5<br/>Moved minimum .net framework requirements to v4.7.1<br/>Added support for .NET7<br/>Updated minimum version of RestSharp as this caused issues when later versions of optimizely and visitor groups.|
 |2.0.0|Removed Support for CMS11<br/>Added support for .NET8<br/>Added support for multiple ODP instances |
 |2.1.0|Added support to get vuid from httprequest, this supports content delivery api |
 |2.1.1|Fixed bug with to HTTPClient instantiation |