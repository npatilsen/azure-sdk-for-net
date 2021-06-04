// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.DigitalTwins.Core.QueryBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace Azure.DigitalTwins.Core.Tests
{
    public class QueryBuilderTests
    {
        [Test]
        public void SimpleTests()
        {
            new AdtQuery().Select("*").From(AdtCollection.DigitalTwins).ToString().Should().Be("SELECT * FROM DIGITALTWINS");
        }
    }
}
