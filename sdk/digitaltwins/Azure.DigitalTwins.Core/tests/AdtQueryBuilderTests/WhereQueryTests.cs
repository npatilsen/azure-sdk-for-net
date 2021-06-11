// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.DigitalTwins.Core.QueryBuilder;
using Azure.DigitalTwins.Core.Queries.QueryBuilders;
using FluentAssertions;
using NUnit.Framework;

namespace Azure.DigitalTwins.Core.Tests
{
    public class WhereQueryTests
    {
        [Test]
        public void WhereQuery_Comparison()
        {
            var query = new WhereQuery(null);
            query.WhereComparison("Temperature", QueryComparisonOperator.Equal, "5");
            query.Stringify()
                .ToUpper()
                .Should()
                .Be("WHERE TEMPERATURE = 5");
        }

        [Test]
        public void WhereQuery_Contains()
        {
            var query = new WhereQuery(null);
            query.WhereContains("Owner", QueryContainOperator.IN, new string[] { "John", "Sally", "Marshall" });
            query.Stringify()
                .ToUpper()
                .Should()
                .Be("WHERE OWNER IN ['JOHN', 'SALLY', 'MARSHALL']");
        }
    }
}
