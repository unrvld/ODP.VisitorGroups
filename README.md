# Visitor Groups for Optimizely Data Platform

Vistor groups for the Optimizely Data platform, supports Optimizely CMS 11 and 12.

[![ODP Visitor Groups](https://github.com/made-to-engage/ODP.VisitorGroups/actions/workflows/build-visitor-groups.yml/badge.svg?branch=main)](https://github.com/made-to-engage/ODP.VisitorGroups/actions/workflows/build-visitor-groups.yml)


## Features

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
```
Install-Package UNRVLD.ODP.VisitorGroups
```

## Configuration (.NET 5.0/6.0)

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
         "BaseEndPoint": "https://api.zaius.com",
         "PrivateApiKey": "key-lives-here"
       }
   }
}
```

## Configuration (.Net Framework)


*web.config*
All settings are optional, apart from the PrivateApiKey
``` xml
  <appSettings>
    <add key="episerver:setoption:UNRVLD.ODP.OdpVisitorGroupOptions.OdpCookieName, UNRVLD.ODP.VisitorGroups" value="vuid" />
    <add key="episerver:setoption:UNRVLD.ODP.OdpVisitorGroupOptions.CacheTimeoutSeconds, UNRVLD.ODP.VisitorGroups" value="1" />
    <add key="episerver:setoption:UNRVLD.ODP.OdpVisitorGroupOptions.BaseEndPoint, UNRVLD.ODP.VisitorGroups" value="https://api.zaius.com" />
    <add key="episerver:setoption:UNRVLD.ODP.OdpVisitorGroupOptions.PrivateApiKey, UNRVLD.ODP.VisitorGroups" value="key-lives-here" />
  </appSettings>
```

You can also manage these with the [options admin module](https://world.optimizely.com/blogs/grzegorz-wiechec/dates/2020/3/configuring-options-from-admin-mode/) 

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
 |1.4.0|Removed support for .net5<br/>Moved minimum .net framework requirements to v4.7.1<br/>Added support for .net7<br/>Updated minimum version of RestSharp as this caused issues when later versions of optimizely and visitor groups.|