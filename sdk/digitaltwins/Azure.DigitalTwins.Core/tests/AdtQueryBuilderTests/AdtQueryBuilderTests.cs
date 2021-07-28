// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
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
            new DigitalTwinsQuery<ConferenceRoom>(DigitalTwinsCollection.Relationships).GetQueryText().Should().Be("SELECT * FROM Relationships");
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
        public void Select_SelectAsWithoutLinq()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Humidity")
                .SelectAs("Room", "R")
                .SelectAs("Temperature", "Temp")
                .Select("Factory")
                .ToString()
                .Should()
                .Be("SELECT Humidity, Room AS R, Temperature AS Temp, Factory FROM DigitalTwins");
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
        public void Select_SelectAs()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature AS Temp, Humidity AS Hum FROM DigitalTwins");
        }

        [Test]
        public void Select_SelectAsChainedWithSelect()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Occupants", "T")
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .GetQueryText()
                .Should()
                .Be("SELECT Occupants, T, Temperature AS Temp, Humidity AS Hum FROM DigitalTwins");
        }

        [Test]
        public void Select_SelectAs_CustomFrom()
        {
            new DigitalTwinsQuery<ConferenceRoom>("DigitalTwins T")
                .SelectAs("T.Temperature", "Temp")
                .GetQueryText()
                .Should()
                .Be("SELECT T.Temperature AS Temp FROM DigitalTwins T");
        }

        [Test]
        public void Select_SelectAs_FromAlias()
        {
            new DigitalTwinsQuery<ConferenceRoom>("DigitalTwins T")
                .Select("T.Temperature")
                .SelectAs("T.Humidity", "Hum")
                .Where(r => r.Temperature >= 50)
                .GetQueryText()
                .Should()
                .Be("SELECT T.Temperature, T.Humidity AS Hum FROM DigitalTwins T WHERE Temperature >= 50");
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
        public void Where_Override()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .WhereCustom($"IS_OF_MODEL('dtmi:example:room;1', exact)")
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('dtmi:example:room;1', exact)");
        }

        [Test]
        public void Where_IsOfModel()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('dtmi:example:room;1', exact)");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1"))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OF_MODEL('dtmi:example:room;1')");
        }

        [Test]
        public void Where_IsBool()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.IsBool(r.IsOccupied))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_BOOL(IsOccupied)");
        }

        [Test]
        public void Where_IsDefined()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.IsDefined(r.Temperature))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_DEFINED(Temperature)");
        }

        [Test] public void Where_IsPrimitive()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
               .Where(r => DigitalTwinsFunctions.IsPrimitive(r.Temperature))
               .ToString()
               .Should()
               .Be("SELECT * FROM DigitalTwins WHERE IS_PRIMITIVE(Temperature)");
        }

        [Test]
        public void Where_IsNumber()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
               .Where(r => DigitalTwinsFunctions.IsNumber(r.Temperature))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_NUMBER(Temperature)");
        }

        [Test]
        public void Where_IsString()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.IsString(r.Factory))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_STRING(Factory)");
        }

        [Test]
        public void Where_IsObject()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.IsObject(r.Factory))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_OBJECT(Factory)");
        }

        [Test]
        public void Where_IsNull()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.IsNull(r.Temperature))
                .ToString()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE IS_NULL(Temperature)");
        }

        [Test]
        public void Where_MultipleWhere()
        {
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
        public void WhereLogic_StartEndsWith()
        {
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => DigitalTwinsFunctions.StartsWith(r.Room, "3"))
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE STARTSWITH(Room, '3')");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Room.EndsWith("3"))
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE ENDSWITH(Room, '3')");
        }

        [Test]
        public void MultipleNested()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .Parenthetical(q => q
                    .IsOfType("Humidity", DigitalTwinsDataType.AdtNumber)
                    .Or()
                    .IsOfType("Humidity", DigitalTwinsDataType.AdtPrimative))
                .Or()
                .Parenthetical(q => q
                    .IsOfType("Temperature", DigitalTwinsDataType.AdtNumber)
                    .Or()
                    .IsOfType("Temperature", DigitalTwinsDataType.AdtPrimative))
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
            Func<DigitalTwinsQuery<ConferenceRoom>> act = () => new DigitalTwinsQuery<ConferenceRoom>(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void WhereLogic_StartsEndsWith_Null()
        {
            Func<DigitalTwinsQuery<ConferenceRoom>> act = () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.StartsWith(null, null));
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void WhereLogic_IsOfModel_Null()
        {
            Func<DigitalTwinsQuery<ConferenceRoom>> act1 = () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsOfModel(null));
            Func<DigitalTwinsQuery<ConferenceRoom>> act2 = () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsOfModel(null, true));

            act1.Should().Throw<InvalidOperationException>();
            act2.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void WhereLogic_IsOfType_Null()
        {
            Func<DigitalTwinsQuery<ConferenceRoom>>[] funcs = new Func<DigitalTwinsQuery<ConferenceRoom>>[]
            {
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsString(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsDefined(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsObject(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsBool(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsPrimitive(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsNull(null)),
                () => new DigitalTwinsQuery<ConferenceRoom>().Where(r => DigitalTwinsFunctions.IsNumber(null)),
            };

            foreach (var func in funcs)
            {
                func.Should().Throw<InvalidOperationException>();
            }
        }

        [Test]
        public void WhereLogic_ContainsNotContains_Null()
        {
            string[] cities = null;
            string property = null;
            Func<DigitalTwinsQuery<ConferenceRoom>> act = () => new DigitalTwinsQuery<ConferenceRoom>().Where($"{property} NIN {cities}");
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void WhereLogic_Comparison_Null()
        {
            Func<DigitalTwinsQuery<ConferenceRoom>> act = () => new DigitalTwinsQuery<ConferenceRoom>().Where($"Temperature >= {null}");
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Select_Null()
        {
            Func<DigitalTwinsQuery<ConferenceRoom>> act1 = () => new DigitalTwinsQuery<ConferenceRoom>().SelectCustom(null);
            Func<DigitalTwinsQuery<ConferenceRoom>> act2 = () => new DigitalTwinsQuery<ConferenceRoom>().SelectAs(null, null);
            Func<DigitalTwinsQuery<ConferenceRoom>> act3 = () => new DigitalTwinsQuery<ConferenceRoom>().Select(r => null);

            act1.Should().Throw<InvalidOperationException>();
            act2.Should().Throw<InvalidOperationException>();
            act3.Should().Throw<InvalidOperationException>();
        }
    }
}
