// Copyright (c) 2017 CLMS. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
#if NETFRAMEWORK
using System.Web.Http.Controllers;
#else
using Microsoft.AspNetCore.Http;
#endif
using zAppDev.DotNet.Framework.Services;
using System.Net.Http;

namespace zAppDev.DotNet.Framework.Logging
{
    public interface IAPILogger
    {
#if NETFRAMEWORK
        void LogExposedAPIAccess(Guid requestId, HttpActionContext actionContext, TimeSpan processingTime, bool cacheHit);

        void LogExternalAPIAccess(Guid requestId, string service, string operation, 
                                  ServiceConsumptionOptions options, object response,
                                  HttpStatusCode status, TimeSpan processingTime, bool throwOnError = false, bool cachedResponse = false);
        
        void LogExposedAPIAccess(Guid requestId, string service, HttpRequestMessage request,
            HttpResponseMessage response, TimeSpan processingTime, bool throwOnError, bool cacheHit);

        void Log(string apiType, string apiTitle, LogMessage message, bool throwOnError);
#else
        void LogExposedAPIAccess(string controller, string action, Guid requestId, TimeSpan processingTime, bool cacheHit);
#endif
    }
}