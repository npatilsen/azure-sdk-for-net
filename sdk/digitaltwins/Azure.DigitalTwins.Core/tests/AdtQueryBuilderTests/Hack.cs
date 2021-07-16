// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core.QueryBuilder;
using FluentAssertions;
using NUnit.Framework;
using Azure.DigitalTwins.Core;
using System.Linq.Expressions;
using System.Diagnostics;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

namespace Linq
{
    public class ConferenceRoom
    {
        public string Room { get; set; }
        public string Factory { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int Occupants { get; set; }
    }

    public class Hack
    {
        [Test]
        public void Select_AllSimple()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .BuildLogic()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins");

            new DigitalTwinsQuery<BasicDigitalTwin>().ToString().Should().Be("SELECT * FROM DigitalTwins");
        }

        [Test]
        public void Select_SingleProperty()
        {
            new AdtQueryBuilder()
                .Select("Room")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Room FROM DigitalTwins");

            new DigitalTwinsQuery<BasicDigitalTwin>()
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
        public void Select_MultipleProperties()
        {
            new AdtQueryBuilder()
                .Select("Room", "Factory", "Temperature", "Humidity")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery<BasicDigitalTwin>()
                .Select("Room", "Factory", "Temperature", "Humidity")
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Room, r => r.Factory, r => r.Temperature, r => r.Humidity)
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_Aggregates_Top_All()
        {
            new AdtQueryBuilder()
                .SelectTopAll(5)
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT TOP(5) FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_Aggregates_Top_Properties()
        {
            new AdtQueryBuilder()
                .SelectTop(3, "Temperature", "Humidity")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT TOP(3) Temperature, Humidity FROM DigitalTwins");
        }

        public void zzz_Select_Aggregates_Count()
        {
            new AdtQueryBuilder()
                .SelectCount()
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT COUNT() FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_Override()
        {
            new AdtQueryBuilder()
                .SelectCustom("TOP(3) Room, Temperature")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
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
            new AdtQueryBuilder()
                .Select("*")
                .From(AdtCollection.DigitalTwins)
                .Where()
                .Compare("Temperature", QueryComparisonOperator.GreaterOrEqual, 50)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");

            new DigitalTwinsQuery<BasicDigitalTwin>()
                .Where("Temperature >= 50")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");

            //new DigitalTwinsQuery<ConferenceRoom>()
            //    .Where(r => r.Temperature >= 50)
            //    .GetQueryText()
            //    .Should()
            //    .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");
        }

        [Test]
        public void zzz_Where_Contains()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .From(AdtCollection.DigitalTwins)
                .Where()
                .NotContains("Location", new string[] { "Paris", "Tokyo", "Madrid", "Prague" })
                .Build()
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

            new DigitalTwinsQuery<BasicDigitalTwin>()
                .Select("Temperature")
                .Where("IS_DEFINED(Humidity) AND Occupants < 10")
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            new DigitalTwinsQuery<BasicDigitalTwin>()
                .Select("Temperature")
                .Where("IS_DEFINED(Humidity)")
                .Where("Occupants < 10")
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
        public void zzz_MultipleNested()
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
        }

        [Test]
        public void zzz_Select_Null()
        {
            new AdtQueryBuilder()
                .Select(null)
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT  FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_EmptyString()
        {
            new AdtQueryBuilder()
                .Select("")
                .From(AdtCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT  FROM DigitalTwins");
        }

        [Test]
        public void zzz_FromCustom_Null()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .FromCustom(null)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM");
        }

        [Test]
        public void zzz_FromCustom_EmptyString()
        {
            new AdtQueryBuilder()
                .SelectAll()
                .FromCustom("")
                .Build()
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

    public class DigitalTwinsQuery<T>
    {
        public DigitalTwinsQuery() : this(AdtCollection.DigitalTwins) { }

        public DigitalTwinsQuery(AdtCollection collection) : this(
            collection switch
            {
                AdtCollection.DigitalTwins => "DigitalTwins",
                AdtCollection.Relationships => "Relationships",
                _ => throw new ArgumentException("Unknown collection", nameof(collection))
            })
        { }

        public DigitalTwinsQuery(string customCollection)
        {
            _collection = customCollection;
        }

        public DigitalTwinsQuery<T> Select(params string[] propertyNames)
        {
            _propertyNames ??= new List<string>();
            _propertyNames.AddRange(propertyNames);

            return this;
        }

        public DigitalTwinsQuery<T> Select(params Expression<Func<T, object>>[] selectors)
        {
            _propertyNames ??= new List<string>();
            _propertyNames.AddRange(selectors.Select(GetPropertyName));

            //foreach (var selector in selectors)
            //{
            //    _propertyNames.Add(GetPropertyName(selector));
            //}

            return this;
        }

        private static void Ensure(bool condition, string message=null)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message ?? "Invalid expression.");
            }
        }

        private static string GetPropertyName(Expression<Func<T, object>> selector)
        {
            LambdaExpression lambda = selector as LambdaExpression;
            Ensure(lambda != null); // TODO - write messages
            Ensure(lambda.Parameters.Count == 1);

            ParameterExpression param = lambda.Parameters[0];
            Ensure(param.Type == typeof(T));    // derived classes? => Type.isAssignableFrom

            Expression body = lambda.Body;
            UnaryExpression conversion = body as UnaryExpression;

            if (conversion != null)
            {
                Ensure(conversion.NodeType == ExpressionType.Convert);
                body = conversion.Operand;
            }

            MemberExpression member = body as MemberExpression;
            Ensure(member != null);
            Ensure(member.Expression == param);
            Ensure(member.Member.MemberType == System.Reflection.MemberTypes.Property);
            // TODO - also support fields?

            return member.Member.Name;
        }

        public DigitalTwinsQuery<T> Take(int count)
        {
            _top = count;
            return this;
        }

        public DigitalTwinsQuery<T> Count()
        {
            _count = true;
            return this;
        }

        public DigitalTwinsQuery<T> Where(string filter)
        {
            _clauses ??= new List<string>();
            _clauses.Add(filter);

            return this;
        }
        public DigitalTwinsQuery<T> Where(Expression<Func<T, bool>> filter)
        {
            _clauses ??= new List<string>();
            //_clauses.Add(filter);
            Assert.Inconclusive();

            return this;
        }

        private string _collection;
        private int? _top;
        private bool _count;

        private List<string> _propertyNames;
        private List<string> _clauses;

        public string GetQueryText()
        {
            AdtQueryBuilder query = new AdtQueryBuilder();
            SelectAsQuery select =
                _count ? query.SelectCount() :
                _top != null && _propertyNames != null ? query.SelectTop(_top.Value, _propertyNames.ToArray()) :
                _top != null ? query.SelectTopAll(_top.Value) :
                _propertyNames != null ? query.Select(_propertyNames.ToArray()) :
                query.SelectAll();

            WhereStatement where = select.FromCustom(_collection);

            if (_clauses?.Count > 0)
            {
                // TODO - change Where()
                var custom = _clauses.Skip(1).Aggregate(
                    where.Where().CustomClause(_clauses[0]),
                    (expr, clause) => expr.And().CustomClause(clause));
            }

            return where
                .Build()
                .GetQueryText();
        }

        public override string ToString() => GetQueryText();
    }

    public static class DigitalTwinsFunctions
    {
        public static bool IsDefined(object value) => throw new NotImplementedException();
    }
}
