﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class XFrameOptionsExamples {    

        public void Example() {
            BuildFunc buildFunc = null;

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            buildFunc.XFrameOptions();

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            buildFunc.XFrameOptions(
                settings =>
                            //settings.Deny()
                            //settings.SameOrigin()
                            settings.AllowFrom(new Uri("http://www.example.org"))
                            .IgnoreIfHeaderIsPresent()
            );
        }
    }
}