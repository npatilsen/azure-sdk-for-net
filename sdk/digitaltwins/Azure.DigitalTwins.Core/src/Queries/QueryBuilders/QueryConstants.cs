// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Keywords for building queries.
    /// </summary>
    internal static class QueryConstants
    {
        public const string Select = "SELECT";
        public const string From = "FROM";
        public const string Join = "JOIN";
        public const string Where = "WHERE";
        public const string Top = "TOP";
        public const string Count = "COUNT";

        // Maps comparison operators represented alphabetically to respective symbolic representations.
        public static Dictionary<string, string> ComparisonOperators = new Dictionary<string, string>()
        {
            { "Equal", "=" },
            { "NotEqual", "!=" },
            { "GeaterThan", ">" },
            { "LessThan", "<" },
            { "GreaterThanEqual", ">=" },
            { "LessThanEqual", "<=" }
        };
    }
}
