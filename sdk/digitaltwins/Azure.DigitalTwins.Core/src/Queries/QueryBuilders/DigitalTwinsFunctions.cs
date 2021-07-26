// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class DigitalTwinsFunctions
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefined(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPrimitive(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBool(object value) => throw new NotImplementedException();

        /// <summary>
        /// TOOD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsString(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsObject(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(object value) => throw new NotImplementedException();

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public static bool IsOfModel(string model, bool exact) => throw new NotImplementedException();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="field"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool StartsWith(string field, string prefix) => throw new NotImplementedException();

        internal static string Convert(object value, IFormatProvider formatProvider)
        {
            return value switch
            {
                null => "null", // TODO - figure out proper way to represent these per ADT specs
                bool x => x.ToString(formatProvider).ToLowerInvariant(),
                int x => x.ToString(formatProvider),
                double x => x.ToString(formatProvider),
                string x => Quote(x),   // TODO - check formatting, escaping single quotes
                System.Collections.IEnumerable x =>
                    $"[{string.Join(", ", x.OfType<object>().Select(s => Convert(s, formatProvider)))}]",
                _ => throw new ArgumentException($"Unable to convert {value} to query literal")
            };
        }

        internal static string Quote(string text)
        {
            if (text == null)
            { return "null"; }

            // Optimistically allocate an extra 5% for escapes
            StringBuilder builder = new StringBuilder(2 + (int)(text.Length * 1.05));
            builder.Append('\'');
            foreach (char ch in text)
            {
                builder.Append(ch);
                if (ch == '\'')
                {
                    builder.Append(ch);
                }
            }
            builder.Append('\'');
            return builder.ToString();
        }
    }
}
