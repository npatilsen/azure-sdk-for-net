// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.DigitalTwins.Core.QueryBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace Azure.DigitalTwins.Core.Tests.QueryBuilderTests
{
    public class AdtQueryBuilderTests
    {
        [Test]
        public void Select_AllSimple()
        {
            new DigitalTwinsQuery<ConferenceRoom>().ToString().Should().Be("SELECT * FROM DigitalTwins");
        }

        [Test]
        public void Select_SingleProperty()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Room")
                .ToString()
                .Should()
                .Be("SELECT Room FROM DigitalTwins");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Room)
                .ToString()
                .Should()
                .Be("SELECT Room FROM DigitalTwins");
        }

        [Test]
        public void Select_SelectAllRelationships()
        {
            new DigitalTwinsQuery<ConferenceRoom>(AdtCollection.Relationships).GetQueryText().Should().Be("SELECT * FROM Relationships");
        }

        [Test]
        public void Select_MultipleProperties()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Room", "Factory", "Temperature", "Humidity")
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Room, r => r.Factory, r => r.Temperature, r => r.Humidity)
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            var digitalTwinsQuery = new DigitalTwinsQuery<ConferenceRoom>();
            digitalTwinsQuery = digitalTwinsQuery.Select("Room", "Factory", "Temperature", "Humidity");
            digitalTwinsQuery
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");
        }

        [Test]
        public void Select_Aggregates_Top_All()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Take(5)
                .ToString()
                .Should()
                .Be("SELECT TOP(5) FROM DigitalTwins");
        }

        [Test]
        public void Select_Aggregates_Top_Properties()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Temperature", "Humidity")
                .Take(3)
                .ToString()
                .Should()
                .Be("SELECT TOP(3) Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Temperature, r => r.Humidity)
                .Take(3)
                .ToString()
                .Should()
                .Be("SELECT TOP(3) Temperature, Humidity FROM DigitalTwins");
        }

        public void Select_Aggregates_Count()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Count()
                .ToString()
                .Should()
                .Be("SELECT COUNT() FROM DigitalTwins");
        }

        [Test]
        public void Select_Override()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .SelectCustom("TOP(3) Room, Temperature")
                .ToString()
                .Should()
                .Be("SELECT TOP(3) Room, Temperature FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_SelectAs()
        {
            new AdtQueryBuilder()
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature AS Temp, Humidity AS Hum FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_SelectAsChainedWithSelect()
        {
            new AdtQueryBuilder()
                .Select("Occupants", "T")
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Occupants, T, Temperature AS Temp, Humidity AS Hum FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_SelectAs_CustomFrom()
        {
            new AdtQueryBuilder()
                .SelectAs("T.Temperature", "Temp")
                .FromCustom("DigitalTwins T")
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT T.Temperature AS Temp FROM DigitalTwins T");
        }

        [Test]
        public void zzz_Select_SelectAs_FromAlias()
        {
            new AdtQueryBuilder()
                .Select("T.Temperature")
                .SelectAs("T.Humidity", "Hum")
                .From(AdtCollection.DigitalTwins, "T")
                .Where()
                .Compare("T.Temperature", QueryComparisonOperator.GreaterOrEqual, 50)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT T.Temperature, T.Humidity AS Hum FROM DigitalTwins T WHERE T.Temperature >= 50");
        }

        [Test]
        public void Where_Comparison()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where($"Temperature >= {50}")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature >= 50)
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");
        }

        [Test]
        public void Where_Contains()
        {
            string city = "Paris";
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where($"Location NIN [{city}, 'Tokyo', 'Madrid', 'Prague']")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Location NIN ['Paris', 'Tokyo', 'Madrid', 'Prague']");

            string[] cities = new string[] { "Paris", "Tokyo", "Madrid", "Prague" };
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where($"Location NIN {cities}")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Location NIN ['Paris', 'Tokyo', 'Madrid', 'Prague']");
        }

        [Test]
        public void zzz_Where_Override()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .CustomClause("IS_OF_MODEL('dtmi:example:room;1', exact)")
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('dtmi:example:room;1', exact)");
        }

        [Test]
        public void zzz_Where_IsOfModel()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .IsOfModel("dtmi:example:room;1", true)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('dtmi:example:room;1', exact)");
        }

        [Test]
        public void zzz_Where_IsBool()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.Relationships)
                .Where()
                .IsOfType("isOccupied", AdtDataType.AdtBool)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM Relationships WHERE IS_BOOL(isOccupied)");
        }

        [Test]
        public void Where_MultipleWhere()
        {
            new AdtQueryBuilder()
                .Select("Temperature")
                .From(AdtCollection.DigitalTwins)
                .Where()
                .IsDefined("Humidity")
                .And()
                .CustomClause("Occupants < 10")
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            int count = 10;
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Temperature")
                .Where($"IS_DEFINED(Humidity) AND Occupants < {count}")
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Temperature")
                .Where($"IS_DEFINED(Humidity)")
                .Where($"Occupants < {count}")
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Temperature)
                .Where(r => DigitalTwinsFunctions.IsDefined(r.Humidity) && r.Occupants < 10)
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");
        }

        [Test]
        public void MultipleNested()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .Parenthetical(q => q
                    .IsOfType("Humidity", AdtDataType.AdtNumber)
                    .Or()
                    .IsOfType("Humidity", AdtDataType.AdtPrimative))
                .Or()
                .Parenthetical(q => q
                    .IsOfType("Temperature", AdtDataType.AdtNumber)
                    .Or()
                    .IsOfType("Temperature", AdtDataType.AdtPrimative))
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE (IS_NUMBER(Humidity) OR IS_PRIMATIVE(Humidity)) OR (IS_NUMBER(Temperature) OR IS_PRIMATIVE(Temperature))");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => (DigitalTwinsFunctions.IsNumber(r.Humidity) || DigitalTwinsFunctions.IsPrimitive(r.Humidity))
                    || (DigitalTwinsFunctions.IsNumber(r.Temperature) || DigitalTwinsFunctions.IsPrimitive(r.Temperature)))
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE (IS_NUMBER(Humidity) OR IS_PRIMITIVE(Humidity)) OR (IS_NUMBER(Temperature) OR IS_PRIMITIVE(Temperature))");

            new DigitalTwinsQuery<ConferenceRoom>()
               .Where(r => (DigitalTwinsFunctions.IsNumber(r.Humidity) || DigitalTwinsFunctions.IsPrimitive(r.Humidity)) &&
                   (DigitalTwinsFunctions.IsNumber(r.Temperature) || DigitalTwinsFunctions.IsPrimitive(r.Temperature)))
               .GetQueryText()
               .Should()
               .Be("SELECT * FROM DigitalTwins WHERE (IS_NUMBER(Humidity) OR IS_PRIMITIVE(Humidity)) AND (IS_NUMBER(Temperature) OR IS_PRIMITIVE(Temperature))");
        }

        [Test]
        public void Select_EmptyString()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("")
                .GetQueryText()
                .Should()
                .Be("SELECT  FROM DigitalTwins");
        }

        [Test]
        public void FromCustom_Null()
        {
            new DigitalTwinsQuery<ConferenceRoom>(null)
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM");
        }

        [Test]
        public void FromCustom_EmptyString()
        {
            new DigitalTwinsQuery<ConferenceRoom>("")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM");
        }

        [Test]
        public void zzz_WhereLogic_Null()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .IsOfModel(null)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('')");
        }

        [Test]
        public void zzz_WhereLogic_Is_Of_Type()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .IsOfType(null, AdtDataType.AdtBool)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_BOOL()");
        }

        [Test]
        public void zzz_WhereLogic_StartsEndsWith_Null()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .StartsWith(null, null)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE STARTSWITH(, '')");
        }

        [Test]
        public void WhereLogic_StartEndsWith()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.StartsWith(r.Room, "3"))
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE STARTSWITH(Room, '3')");

            // alternate type of writing
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Room.StartsWith("3"))
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE STARTSWITH(Room, '3')");
        }

        [Test]
        public void zzz_WhereLogic_ContainsNotContains_Null()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .Contains(null, null)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE  IN []");
        }

        [Test]
        public void zzz_WhereLogic_Compare_Null()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .Compare(null, QueryComparisonOperator.Equal, 10)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE  = 10");
        }
    }
}
