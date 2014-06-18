﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders {
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using MidFunc = Func<Func<IDictionary<string, object>, Task>,Func<IDictionary<string, object>, Task>>;
    public static class StrictTransportSecurityMiddleware {
        public static MidFunc StrictTransportSecurityHeader(StrictTransportSecurityOptions options) {
            return next =>
                env => {
                    var context = env.AsContext();

                    if (RedirectToSecureTransport(options, context.Request)) {
                        SetResponseForRedirect(context, options);
                        return Task.FromResult(true);
                    }

                    // Only over secure transport (http://tools.ietf.org/html/rfc6797#section-7.2)
                    // Quotation: "An HSTS Host MUST NOT include the STS header field in HTTP responses conveyed over non-secure transport."
                    if (context.Request.IsSecure) {
                        var response = context.Response;
                        var state = new {
                            Options = options,
                            Response = response
                        };

                        response.OnSendingHeaders(ApplyHeader, state);
                    }
                    
                    return next(env);
                };
        }
        private static void SetResponseForRedirect(IOwinContext context, StrictTransportSecurityOptions options) {
            var response = context.Response;
            response.StatusCode = 301;
            response.ReasonPhrase = options.RedirectReasonPhrase(301);
            response.Headers[HeaderConstants.Location] = options.RedirectUriBuilder(context.Request.Uri);
        }

        private static void ApplyHeader(dynamic obj) {
            var options = (StrictTransportSecurityOptions)obj.Options;
            var response = (OwinResponse)obj.Response;

            response.Headers[HeaderConstants.StrictTransportSecurity] = ConstructHeaderValue(options);
        }

        private static string ConstructHeaderValue(StrictTransportSecurityOptions options) {
            var age = MaxAge(options.MaxAge);
            var subDomains = IncludeSubDomains(options.IncludeSubDomains);

            if (subDomains == "") {
                return age;
            }

            return "{0}; {1}".FormatWith(age, subDomains);
        }

        #region [ Helper ]
        private static string MaxAge(uint seconds) {
            return "max-age={0}".FormatWith(seconds);
        }
        private static string IncludeSubDomains(bool includeSubDomains) {
            return includeSubDomains ? "includeSubDomains" : "";
        }
        private static bool RedirectToSecureTransport(StrictTransportSecurityOptions options, IOwinRequest request) {
            return options.RedirectToSecureTransport && !request.IsSecure;
        }
        #endregion
    }
}