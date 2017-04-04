﻿using System;
using SecurityHeaders.Infrastructure;

namespace SecurityHeaders {
    /// <summary>
    /// The X-Frame-Option header value.
    /// </summary>
    public class XFrameOptionHeaderValue {

        /// <summary>
        /// Gets the header value for the x-frame-option.
        /// </summary>
        public string HeaderValue { get; }

        private XFrameOptionHeaderValue(string headerValue) {
            Guard.NotNullOrWhiteSpace(headerValue, nameof(headerValue));
            HeaderValue = headerValue;
        }

        /// <summary>
        /// Creates a header value with DENY.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XFrameOptionHeaderValue Deny() => Create("DENY");

        /// <summary>
        /// Creates a header value with SAMEORIGIN.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XFrameOptionHeaderValue SameOrigin() => Create("SAMEORIGIN");

        /// <summary>
        /// Creates a header value of ALLOW-FROM with the provided <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin">The origin which should be allowed.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="origin"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="origin"/> is empty or whitespace.</exception>
        /// <exception cref="FormatException">When <paramref name="origin"/> is not a valid <see cref="Uri"/>.</exception>
        public static XFrameOptionHeaderValue AllowFrom(string origin) {
            Guard.NotNullOrWhiteSpace(origin, nameof(origin));
            return AllowFrom(new Uri(origin));
        }

        /// <summary>
        /// Creates a header value of ALLOW-FROM with the provided <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin">The origin which should be allowed.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="origin"/> is null.</exception>
        public static XFrameOptionHeaderValue AllowFrom(Uri origin) {
            Guard.NotNull(origin, nameof(origin));
            return Create($"ALLOW-FROM {Rfc6454Utility.SerializeOrigin(origin)}");
        }

        private static XFrameOptionHeaderValue Create(string value) => new XFrameOptionHeaderValue(value);

        /// <summary>
        /// Extracts the header value for implicit usage on strings.
        /// </summary>
        /// <param name="headerValue">The <see cref="XFrameOptionHeaderValue"/> instance where the value should be extracted.</param>
        public static implicit operator string(XFrameOptionHeaderValue headerValue) {
            return headerValue.HeaderValue;
        }

        #region ReSharper-Equals generated
        /// <inheritdoc />
        protected bool Equals(XFrameOptionHeaderValue other) {
            return string.Equals(HeaderValue, other.HeaderValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return Equals((XFrameOptionHeaderValue) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(HeaderValue);
        }

        /// <inheritdoc />
        public static bool operator ==(XFrameOptionHeaderValue left, XFrameOptionHeaderValue right) {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(XFrameOptionHeaderValue left, XFrameOptionHeaderValue right) {
            return !Equals(left, right);
        }
        #endregion
    }
}
