using System;
using System.Net;
#if NETFRAMEWORK
using System.Web.Http.Controllers;
#endif
using CLMS.Framework.Services;
using System.Net.Http;

namespace CLMS.Framework.Logging
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
#endif
    }
}