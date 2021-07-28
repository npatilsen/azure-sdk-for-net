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
using System.Globalization;
using System.Collections.ObjectModel;
using System.Reflection;

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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .BuildLogic()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins");

            new DigitalTwinsQuery().ToString().Should().Be("SELECT * FROM DigitalTwins");
        }

        [Test]
        public void Select_SingleProperty()
        {
            new AdtQueryBuilder()
                .Select("Room")
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Room FROM DigitalTwins");

            new DigitalTwinsQuery()
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
            new AdtQueryBuilder()
                .Select("Room", "Factory", "Temperature", "Humidity")
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery()
                .Select("Room", "Factory", "Temperature", "Humidity")
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Room, r => r.Factory, r => r.Temperature, r => r.Humidity)
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");

            var digitalTwinsQuery = new DigitalTwinsQuery();
            digitalTwinsQuery = digitalTwinsQuery.Select("Room", "Factory", "Temperature", "Humidity");
            digitalTwinsQuery
                .ToString()
                .Should()
                .Be("SELECT Room, Factory, Temperature, Humidity FROM DigitalTwins");
        }

        [Test]
        public void zzz_Select_Aggregates_Top_All()
        {
            new AdtQueryBuilder()
                .SelectTopAll(5)
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT TOP(3) Temperature, Humidity FROM DigitalTwins");
        }

        public void zzz_Select_Aggregates_Count()
        {
            new AdtQueryBuilder()
                .SelectCount()
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins, "T")
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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .Compare("Temperature", QueryComparisonOperator.GreaterOrEqual, 50)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Temperature >= 50");

            new DigitalTwinsQuery()
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
            new AdtQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .NotContains("Location", new string[] { "Paris", "Tokyo", "Madrid", "Prague" })
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Location NIN ['Paris', 'Tokyo', 'Madrid', 'Prague']");

            string city = "Paris";
            new DigitalTwinsQuery()
                .Where($"Location NIN [{city}, 'Tokyo', 'Madrid', 'Prague']")
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE Location NIN ['Paris', 'Tokyo', 'Madrid', 'Prague']");

            string[] cities = new string[] { "Paris", "Tokyo", "Madrid", "Prague" };
            new DigitalTwinsQuery()
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.Relationships)
                .Where()
                .IsOfType("isOccupied", DigitalTwinsDataType.AdtBool)
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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .IsDefined("Humidity")
                .And()
                .CustomClause("Occupants < 10")
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            int count = 10;
            new DigitalTwinsQuery()
                .Select("Temperature")
                .Where($"IS_DEFINED(Humidity) AND Occupants < {count}")
                .GetQueryText()
                .Should()
                .Be("SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10");

            new DigitalTwinsQuery()
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
        public void zzz_Select_Null()
        {
            new AdtQueryBuilder()
                .Select(null)
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .IsOfType(null, DigitalTwinsDataType.AdtBool)
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
                .From(DigitalTwinsCollection.DigitalTwins)
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
            new AdtQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .StartsWith("Room", "3")
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE STARTSWITH(Room, '3')");

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
                .From(DigitalTwinsCollection.DigitalTwins)
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
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where()
                .Compare(null, QueryComparisonOperator.Equal, 10)
                .Build()
                .GetQueryText()
                .Should()
                .Be("SELECT * FROM DigitalTwins WHERE  = 10");
        }
    }

    public class Samples
    {
        public void Stuff()
        {
            // SELECT * FROM DigitalTwins
            DigitalTwinsQuery selectAll = new DigitalTwinsQuery();
            var selectAllConferenceRoom = new DigitalTwinsQuery<ConferenceRoom>();

            // SELECT * FROM Relationships
            var selectRelationships = new DigitalTwinsQuery<ConferenceRoom>(DigitalTwinsCollection.Relationships);

            // SELECT Temperature FROM DigitalTwins
            DigitalTwinsQuery selectSingleProperty = new DigitalTwinsQuery()
                .Select("Temperature");

            // selecting from a non BasicDigitalTwin
            // note that if temperature isn't a field of the Conference Room class, an error will appear
            var anotherSingleSelect = new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Temperature);

            // SELECT Temperature, Humidity FROM DigitalTwins
            var selectMultiple = new DigitalTwinsQuery()
                .Select("Temperature", "Humidity");

            var anotherSelectMultiple = new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Temperature, r => r.Humidity);

            // SELECT * FROM DigitalTwins WHERE Temperature >= 50
            var whereComparison = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature >= 50);

            // SELECT * FROM DigitalTwins WHERE Location NIN ['Paris', 'Tokyo', 'Madrid', 'Prague']
            string[] cities = new string[] { "Paris", "Tokyo", "Madrid", "Prague" };
            var whereContains = new DigitalTwinsQuery()
                .Where($"Location NIN {cities}");

            // SELECT Temperature FROM DigitalTwins WHERE IS_DEFINED(Humidity) AND Occupants < 10
            int count = 10;
            new DigitalTwinsQuery()
                .Select("Temperature")
                .Where($"IS_DEFINED(Humidity) AND Occupants < {count}");

            new DigitalTwinsQuery()
                .Select("Temperature")
                .Where($"IS_DEFINED(Humidity)")
                .Where($"Occupants < {count}");

            new DigitalTwinsQuery<ConferenceRoom>()
                .Select(r => r.Temperature)
                .Where(r => DigitalTwinsFunctions.IsDefined(r.Humidity) && r.Occupants < 10);

            // SELECT * FROM DigitalTwins WHERE (IS_NUMBER(Humidity) OR IS_PRIMITIVE(Humidity)) OR (IS_NUMBER(Temperature) OR IS_PRIMITIVE(Temperature))
            new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r =>
                (
                    DigitalTwinsFunctions.IsNumber(r.Humidity) ||
                    DigitalTwinsFunctions.IsPrimitive(r.Humidity)
                )
                ||
                (
                    DigitalTwinsFunctions.IsNumber(r.Temperature) ||
                    DigitalTwinsFunctions.IsPrimitive(r.Temperature)
                ));

            // SELECT * FROM DigitalTwins WHERE (IS_NUMBER(Humidity) OR IS_PRIMITIVE(Humidity)) AND (IS_NUMBER(Temperature) OR IS_PRIMITIVE(Temperature))
            new DigitalTwinsQuery<ConferenceRoom>()
               .Where(r =>
               (
                    DigitalTwinsFunctions.IsNumber(r.Humidity) ||
                    DigitalTwinsFunctions.IsPrimitive(r.Humidity)
               )
               &&
               (
                    DigitalTwinsFunctions.IsNumber(r.Temperature) ||
                    DigitalTwinsFunctions.IsPrimitive(r.Temperature))
               );
        }
    }

    public class DigitalTwinsQuery : DigitalTwinsQuery<BasicDigitalTwin>
    {
        public new DigitalTwinsQuery Select(params string[] propertyNames)
        {
            base.Select(propertyNames);
            return this;
        }
    }

    public class DigitalTwinsQuery<T>
    {
        public DigitalTwinsQuery() : this(DigitalTwinsCollection.DigitalTwins) { }

        public DigitalTwinsQuery(DigitalTwinsCollection collection)
        {
            _collection = collection switch
            {
                DigitalTwinsCollection.DigitalTwins => "DigitalTwins",
                DigitalTwinsCollection.Relationships => "Relationships",
                _ => throw new ArgumentException("Unknown collection", nameof(collection))
            };
        }

        // Alternate constructor with more "complicated" syntax
        //public DigitalTwinsQuery(AdtCollection collection) : this(
        //    collection switch
        //    {
        //        AdtCollection.DigitalTwins => "DigitalTwins",
        //        AdtCollection.Relationships => "Relationships",
        //        _ => throw new ArgumentException("Unknown collection", nameof(collection))
        //    })
        //{ }

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

            // alternate way of doing this
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

        private DigitalTwinsQuery<T> Where(string filter)
        {
            _clauses ??= new List<string>();
            _clauses.Add(filter);

            return this;
        }

        public DigitalTwinsQuery<T> Where(FormattableString filter) => Where(filter, null);

        public DigitalTwinsQuery<T> Where(FormattableString filter, IFormatProvider formatProvider)
        {
            formatProvider ??= CultureInfo.InvariantCulture;

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            string[] args = new string[filter.ArgumentCount];

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = DigitalTwinsFunctions.Convert(filter.GetArgument(i), formatProvider);
            }

            string text = string.Format(formatProvider, filter.Format, args);
            return Where(text);
        }

        public DigitalTwinsQuery<T> Where(Expression<Func<T, bool>> filter)
        {
            string query = string.Empty;
            Expression e = Evaluator.PartialEval(filter);
            e = ExpressionNormalizer.Normalize(e, new Dictionary<Expression, Expression>());

            LambdaExpression l = e as LambdaExpression;
            Ensure(l != null);
            Ensure(l.Parameters.Count == 1);

            query = QueryFilterVistor.Translate(l.Body);

            return Where(query);
        }

        private string _collection;
        private int? _top;
        private bool _count;

        private List<string> _propertyNames;
        private List<string> _clauses;

        public string GetQueryText()
        {
            AdtQueryBuilder query = new AdtQueryBuilder();
            SelectAsQuery select = _count
                ? query.SelectCount()
                : _top != null && _propertyNames != null
                    ? query.SelectTop(_top.Value, _propertyNames.ToArray())
                    : _top != null
                        ? query.SelectTopAll(_top.Value)
                        : _propertyNames != null
                            ? query.Select(_propertyNames.ToArray())
                            : query.SelectAll();

            WhereStatement where = select.FromCustom(_collection);

            if (_clauses?.Count > 0)
            {
                // TODO - change Where()
                var custom = _clauses
                    .Skip(1)
                    .Aggregate(
                        where
                            .Where()
                            .CustomClause(_clauses[0]), (expr, clause) => expr.And().CustomClause(clause));
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
        public static bool IsNumber(object value) => throw new NotImplementedException();
        public static bool IsPrimitive(object value) => throw new NotImplementedException();
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

    internal class QueryFilterVistor : LinqExpressionVisitor
    {
        private StringBuilder _filter = new StringBuilder();

        private QueryFilterVistor() { }

        public static string Translate(Expression e)
        {
            var visitor = new QueryFilterVistor();
            visitor.Visit(e);

            return visitor._filter.ToString();
        }

        private static Exception NotSupported(Expression e)
        {
            return new InvalidOperationException($"Expression {e} is not supported.");
        }

        internal override Expression VisitConstant(ConstantExpression c)
        {
            _filter.Append(DigitalTwinsFunctions.Convert(c.Value, CultureInfo.InvariantCulture));
            return c;
        }

        internal override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression is ParameterExpression)
            {
                _filter.Append(m.Member.Name);
                return m;
            }

            throw NotSupported(m);
        }

        internal override Expression VisitBinary(BinaryExpression b)
        {
            string op = b.NodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "!=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThan => "<",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                _ => throw NotSupported(b)
            };

            // left side
            bool parens = GetPrecedence(b) >= GetPrecedence(b.Left);
            // isOccupied == (empty || free)
            //             9        4
            /*
                      BinExp (==)
                       /       \
                    Const         BinExp(||)
                                  empty    free
            */

            if (parens)
            {
                _filter.Append('(');
            }

            Visit(b.Left);

            if (parens)
            {
                _filter.Append(')');
            }

            // operator
            _filter.Append(' ').Append(op).Append(' ');

            // right side
            parens = GetPrecedence(b) >= GetPrecedence(b.Right);

            if (parens)
            {
                _filter.Append('(');
            }

            Visit(b.Right);

            if (parens)
            {
                _filter.Append(')');
            }

            return b;
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        private static int GetPrecedence(Expression e) =>
            e.NodeType switch
            {
                // Unused ExpressionType values:
                ExpressionType.Block => 20,
                ExpressionType.Constant => 20,
                ExpressionType.DebugInfo => 20,
                ExpressionType.Dynamic => 20,
                ExpressionType.Extension => 20,
                ExpressionType.Goto => 20,
                ExpressionType.Label => 20,
                ExpressionType.Loop => 20,
                ExpressionType.Parameter => 20,
                ExpressionType.Quote => 20,
                ExpressionType.RuntimeVariables => 20,
                ExpressionType.Throw => 20,
                ExpressionType.Try => 20,
                ExpressionType.Unbox => 20,

                // Primary: x.y, f(x), a[i], x?.y, x?[y], x++, x--, x!, new,
                // typeof, checked, unchecked, default, nameof, delegate,
                // sizeof, stackalloc, x->y
                ExpressionType.MemberAccess => 19,
                ExpressionType.ArrayLength => 19,
                ExpressionType.Call => 19,
                ExpressionType.Invoke => 19,
                ExpressionType.Index => 19,
                ExpressionType.ArrayIndex => 19,
                ExpressionType.PostIncrementAssign => 19,
                ExpressionType.PostDecrementAssign => 19,
                ExpressionType.New => 19,
                ExpressionType.NewArrayBounds => 19,
                ExpressionType.NewArrayInit => 19,
                ExpressionType.MemberInit => 19,
                ExpressionType.ListInit => 19,
                ExpressionType.Default => 19,

                // Unary: +x, -x, !x, ~x, ++x, --x, ^x, (T)x, await, &x, *x, true and false
                ExpressionType.UnaryPlus => 18,
                ExpressionType.Negate => 18,
                ExpressionType.NegateChecked => 18,
                ExpressionType.OnesComplement => 18,
                ExpressionType.Not => 18,
                ExpressionType.Increment => 18,
                ExpressionType.PreIncrementAssign => 18,
                ExpressionType.Decrement => 18,
                ExpressionType.PreDecrementAssign => 18,
                ExpressionType.Convert => 18,
                ExpressionType.ConvertChecked => 18,
                ExpressionType.IsTrue => 18,
                ExpressionType.IsFalse => 18,

                // Range: x..y
                // (not represented in LINQ)

                // switch expression
                ExpressionType.Switch => 16,

                // with expression
                // (not represented in LINQ)

                // Power: a ^ b
                ExpressionType.Power => 14,

                // Multiplicative: x * y, x / y, x % y
                ExpressionType.Multiply => 13,
                ExpressionType.MultiplyChecked => 13,
                ExpressionType.Divide => 13,
                ExpressionType.Modulo => 13,

                // Additive: x + y, x – y
                ExpressionType.Add => 12,
                ExpressionType.AddChecked => 12,
                ExpressionType.Subtract => 12,
                ExpressionType.SubtractChecked => 12,

                // Shift: x << y, x >> y
                ExpressionType.LeftShift => 11,
                ExpressionType.RightShift => 11,

                // Relational and type-testing: x < y, x > y, x <= y, x >= y, is, as
                ExpressionType.LessThan => 10,
                ExpressionType.GreaterThan => 10,
                ExpressionType.LessThanOrEqual => 10,
                ExpressionType.GreaterThanOrEqual => 10,
                ExpressionType.TypeIs => 10,
                ExpressionType.TypeAs => 10,
                ExpressionType.TypeEqual => 10,

                //          Equality: x == y, x != y
                ExpressionType.Equal => 9,
                ExpressionType.NotEqual => 9,

                // Boolean logical AND or bitwise logical AND: x & y
                ExpressionType.And => 8,

                // Boolean logical XOR or bitwise logical XOR: x ^ y
                ExpressionType.ExclusiveOr => 7,

                // Boolean logical OR or bitwise logical OR: x | y
                ExpressionType.Or => 6,

                //          Conditional AND: x && y
                ExpressionType.AndAlso => 5,

                //          Conditional OR: x || y
                ExpressionType.OrElse => 4,

                // Null-coalescing operator: x ?? y
                ExpressionType.Coalesce => 3,

                // Conditional operator: c ? t : f
                ExpressionType.Conditional => 2,

                // Assignment and lambda declaration:
                // x = y, x += y, x -= y, x *= y, x /= y, x %= y, x &= y,
                // x |= y, x ^= y, x <<= y, x >>= y, x ??= y, =>
                ExpressionType.Assign => 1,
                ExpressionType.AddAssign => 1,
                ExpressionType.AddAssignChecked => 1,
                ExpressionType.SubtractAssign => 1,
                ExpressionType.SubtractAssignChecked => 1,
                ExpressionType.MultiplyAssign => 1,
                ExpressionType.MultiplyAssignChecked => 1,
                ExpressionType.DivideAssign => 1,
                ExpressionType.ModuloAssign => 1,
                ExpressionType.PowerAssign => 1,
                ExpressionType.AndAssign => 1,
                ExpressionType.OrAssign => 1,
                ExpressionType.ExclusiveOrAssign => 1,
                ExpressionType.LeftShiftAssign => 1,
                ExpressionType.RightShiftAssign => 1,
                ExpressionType.Lambda => 1,

                // Unknown
                _ => 0
            };

        internal override Expression VisitInvocation(InvocationExpression iv)
        {
            throw NotSupported(iv);
        }

        internal override Expression VisitLambda(LambdaExpression lambda)
        {
            throw NotSupported(lambda);
        }

        internal override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(DigitalTwinsFunctions))
            {
                string name = m.Method.Name switch
                {
                    nameof(DigitalTwinsFunctions.IsDefined) => "IS_DEFINED",
                    nameof(DigitalTwinsFunctions.IsNumber) => "IS_NUMBER",
                    nameof(DigitalTwinsFunctions.IsPrimitive) => "IS_PRIMITIVE",
                    nameof(DigitalTwinsFunctions.StartsWith) => "STARTSWITH",
                    _ => throw NotSupported(m)
                };

                _filter.Append(name).Append('(');

                if (m.Arguments.Count > 0)
                {
                    bool first = true;

                    foreach (var arg in m.Arguments)
                    {
                        if (!first)
                        {
                            _filter.Append(", ");
                        }

                        Visit(arg);
                        first = false;
                    }
                }

                _filter.Append(')');

                return m;
            }

            throw NotSupported(m);
        }

        internal override Expression VisitParameter(ParameterExpression p)
        {
            throw NotSupported(p);
        }

        internal override Expression VisitUnary(UnaryExpression u)
        {
            if (u.NodeType == ExpressionType.Convert)
            {
                return Visit(u.Operand);
            }

            throw NotSupported(u);
        }

        internal override Expression Visit(Expression exp)
        {
            return base.Visit(exp);
        }

        internal override ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            return base.VisitExpressionList(original);
        }
    }

    #region Extracted from https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/tables/Azure.Data.Tables/src/Queryable
    internal abstract class LinqExpressionVisitor
    {
        internal virtual Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return exp;
            }

            switch (exp.NodeType)
            {
                case ExpressionType.UnaryPlus:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Power:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression)exp);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)exp);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression)exp);
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression type {0} is not supported.", exp.NodeType.ToString()));
            }
        }

        internal virtual Expression VisitUnary(UnaryExpression u)
        {
            Expression operand = Visit(u.Operand);
            if (operand != u.Operand)
            {
                return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
            }

            return u;
        }

        internal virtual Expression VisitBinary(BinaryExpression b)
        {
            Expression left = Visit(b.Left);
            Expression right = Visit(b.Right);
            Expression conversion = Visit(b.Conversion);
            if (left != b.Left || right != b.Right || conversion != b.Conversion)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                else
                {
                    return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
                }
            }

            return b;
        }

        internal virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        internal virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        internal virtual Expression VisitMemberAccess(MemberExpression m)
        {
            Expression exp = Visit(m.Expression);
            if (exp != m.Expression)
            {
                return Expression.MakeMemberAccess(exp, m.Member);
            }

            return m;
        }

        internal virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            Expression obj = Visit(m.Object);

            IEnumerable<Expression> args = VisitExpressionList(m.Arguments);
            if (obj != m.Object || args != m.Arguments)
            {
                return Expression.Call(obj, m.Method, args);
            }

            return m;
        }

        internal virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(p);
                }
            }

            if (list != null)
            {
                return new ReadOnlyCollection<Expression>(list);
            }

            return original;
        }

        internal virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression body = Visit(lambda.Body);
            if (body != lambda.Body)
            {
                return Expression.Lambda(lambda.Type, body, lambda.Parameters);
            }

            return lambda;
        }

        internal virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> args = VisitExpressionList(iv.Arguments);
            Expression expr = Visit(iv.Expression);
            if (args != iv.Arguments || expr != iv.Expression)
            {
                return Expression.Invoke(expr, args);
            }

            return iv;
        }
    }

    internal static class Evaluator
    {
        internal static Expression PartialEval(Expression expression, Func<Expression, bool> canBeEvaluated)
        {
            Nominator nominator = new Nominator(canBeEvaluated);
            HashSet<Expression> candidates = nominator.Nominate(expression);
            return new SubtreeEvaluator(candidates).Eval(expression);
        }

        internal static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            int rootResourceSet = 10000;
            return expression.NodeType != ExpressionType.Parameter &&
                expression.NodeType != ExpressionType.Lambda &&
                expression.NodeType != (ExpressionType)rootResourceSet;
        }

        internal class SubtreeEvaluator : LinqExpressionVisitor
        {
            private HashSet<Expression> candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return Visit(exp);
            }

            internal override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }

                if (candidates.Contains(exp))
                {
                    return Evaluate(exp);
                }

                return base.Visit(exp);
            }

            private static Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }

                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                object constantValue = fn.DynamicInvoke(null);
                Debug.Assert(!(constantValue is Expression), "!(constantValue is Expression)");

                Type constantType = e.Type;
                if (constantValue != null && constantType.IsArray && constantType.GetElementType() == constantValue.GetType().GetElementType())
                {
                    constantType = constantValue.GetType();
                }

                return Expression.Constant(constantValue, constantType);
            }
        }

        internal class Nominator : LinqExpressionVisitor
        {
            private Func<Expression, bool> functionCanBeEvaluated;

            private HashSet<Expression> candidates;

            private bool cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> functionCanBeEvaluated)
            {
                this.functionCanBeEvaluated = functionCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                candidates = new HashSet<Expression>(EqualityComparer<Expression>.Default);
                Visit(expression);
                return candidates;
            }

            internal override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = cannotBeEvaluated;
                    cannotBeEvaluated = false;

                    base.Visit(expression);

                    if (!cannotBeEvaluated)
                    {
                        if (functionCanBeEvaluated(expression))
                        {
                            candidates.Add(expression);
                        }
                        else
                        {
                            cannotBeEvaluated = true;
                        }
                    }

                    cannotBeEvaluated |= saveCannotBeEvaluated;
                }

                return expression;
            }
        }
    }

    internal class ExpressionNormalizer : LinqExpressionVisitor
    {
        private const bool LiftToNull = false;

        private readonly Dictionary<Expression, Pattern> _patterns = new Dictionary<Expression, Pattern>(/*ReferenceEqualityComparer<Expression>.Instance*/);

            private ExpressionNormalizer(Dictionary<Expression, Expression> normalizerRewrites)
        {
            Debug.Assert(normalizerRewrites != null, "normalizerRewrites != null");
            NormalizerRewrites = normalizerRewrites;
        }

        internal Dictionary<Expression, Expression> NormalizerRewrites { get; }

        internal static Expression Normalize(Expression expression, Dictionary<Expression, Expression> rewrites)
        {
            Debug.Assert(expression != null, "expression != null");
            Debug.Assert(rewrites != null, "rewrites != null");

            ExpressionNormalizer normalizer = new ExpressionNormalizer(rewrites);
            Expression result = normalizer.Visit(expression);
            return result;
        }

        internal override Expression VisitBinary(BinaryExpression b)
        {
            BinaryExpression visited = (BinaryExpression)base.VisitBinary(b);

            switch (visited.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:

                    Expression normalizedLeft = UnwrapObjectConvert(visited.Left);
                    Expression normalizedRight = UnwrapObjectConvert(visited.Right);
                    if (normalizedLeft != visited.Left || normalizedRight != visited.Right)
                    {
                        visited = CreateRelationalOperator(visited.NodeType, normalizedLeft, normalizedRight);
                    }
                    break;
            }

            if (_patterns.TryGetValue(visited.Left, out Pattern pattern) && pattern.Kind == PatternKind.Compare && IsConstantZero(visited.Right))
            {
                ComparePattern comparePattern = (ComparePattern)pattern;
                if (TryCreateRelationalOperator(visited.NodeType, comparePattern.Left, comparePattern.Right, out BinaryExpression relationalExpression))
                {
                    visited = relationalExpression;
                }
            }

            RecordRewrite(b, visited);

            return visited;
        }

        internal override Expression VisitUnary(UnaryExpression u)
        {
            UnaryExpression visited = (UnaryExpression)base.VisitUnary(u);
            Expression result = visited;

            RecordRewrite(u, result);

            return result;
        }

        private static Expression UnwrapObjectConvert(Expression input)
        {
            if (input.NodeType == ExpressionType.Constant && input.Type == typeof(object))
            {
                ConstantExpression constant = (ConstantExpression)input;

                if (constant.Value != null &&
                    constant.Value.GetType() != typeof(object))
                {
                    return Expression.Constant(constant.Value, constant.Value.GetType());
                }
            }

            while (ExpressionType.Convert == input.NodeType)
            {
                input = ((UnaryExpression)input).Operand;
            }

            return input;
        }

        private static bool IsConstantZero(Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant &&
                ((ConstantExpression)expression).Value.Equals(0);
        }

        internal override Expression VisitMethodCall(MethodCallExpression call)
        {
            Expression visited = VisitMethodCallNoRewrite(call);
            RecordRewrite(call, visited);
            return visited;
        }

        internal Expression VisitMethodCallNoRewrite(MethodCallExpression call)
        {
            MethodCallExpression visited = (MethodCallExpression)base.VisitMethodCall(call);

            if (visited.Method.IsStatic && visited.Method.Name == "Equals" && visited.Arguments.Count > 1)
            {
                return Expression.Equal(visited.Arguments[0], visited.Arguments[1], false, visited.Method);
            }

            if (!visited.Method.IsStatic && visited.Method.Name == "Equals" && visited.Arguments.Count > 0)
            {
                return CreateRelationalOperator(ExpressionType.Equal, visited.Object, visited.Arguments[0]);
            }

            if (visited.Method.IsStatic && visited.Method.Name == "CompareString" && visited.Method.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators")
            {
                return CreateCompareExpression(visited.Arguments[0], visited.Arguments[1]);
            }

            if (!visited.Method.IsStatic && visited.Method.Name == "CompareTo" && visited.Arguments.Count == 1 && visited.Method.ReturnType == typeof(int))
            {
                return CreateCompareExpression(visited.Object, visited.Arguments[0]);
            }

            if (visited.Method.IsStatic && visited.Method.Name == "Compare" && visited.Arguments.Count > 1 && visited.Method.ReturnType == typeof(int))
            {
                return CreateCompareExpression(visited.Arguments[0], visited.Arguments[1]);
            }

            // TODO: Potentially normalize other things like `== null` or `is null` to `IsNull`
            if (visited.Method.Name == nameof(string.StartsWith) && visited.Method.DeclaringType == typeof(string))
            {
                return Expression.Call(typeof(DigitalTwinsFunctions).GetMethod(nameof(DigitalTwinsFunctions.StartsWith)), visited.Object,visited.Arguments[0]);
            }

            // Let everything through
            if (visited.Method.DeclaringType == typeof(DigitalTwinsFunctions))
            {
                return visited;
            }

            throw new NotSupportedException($"Method {visited.Method.Name} not supported.");
        }

        private static readonly MethodInfo StaticRelationalOperatorPlaceholderMethod = typeof(ExpressionNormalizer).GetMethod("RelationalOperatorPlaceholder", BindingFlags.Static | BindingFlags.NonPublic);

        private static bool RelationalOperatorPlaceholder<TLeft, TRight>(TLeft left, TRight right)
        {
            Debug.Assert(false, "This method should never be called. It exists merely to support creation of relational LINQ expressions.");
            return object.ReferenceEquals(left, right);
        }

        private static BinaryExpression CreateRelationalOperator(ExpressionType op, Expression left, Expression right)
        {
            if (!TryCreateRelationalOperator(op, left, right, out BinaryExpression result))
            {
                Debug.Assert(false, "CreateRelationalOperator has unknown op " + op);
            }

            return result;
        }

        private static bool TryCreateRelationalOperator(ExpressionType op, Expression left, Expression right, out BinaryExpression result)
        {
            MethodInfo relationalOperatorPlaceholderMethod = StaticRelationalOperatorPlaceholderMethod.MakeGenericMethod(left.Type, right.Type);

            switch (op)
            {
                case ExpressionType.Equal:
                    result = Expression.Equal(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                case ExpressionType.NotEqual:
                    result = Expression.NotEqual(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                case ExpressionType.LessThan:
                    result = Expression.LessThan(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                case ExpressionType.LessThanOrEqual:
                    result = Expression.LessThanOrEqual(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                case ExpressionType.GreaterThan:
                    result = Expression.GreaterThan(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                case ExpressionType.GreaterThanOrEqual:
                    result = Expression.GreaterThanOrEqual(left, right, LiftToNull, relationalOperatorPlaceholderMethod);
                    return true;

                default:
                    result = null;
                    return false;
            }
        }

        private Expression CreateCompareExpression(Expression left, Expression right)
        {
            Expression result = Expression.Condition(
                CreateRelationalOperator(ExpressionType.Equal, left, right),
                Expression.Constant(0),
                Expression.Condition(
                    CreateRelationalOperator(ExpressionType.GreaterThan, left, right),
                    Expression.Constant(1),
                    Expression.Constant(-1)));

            _patterns[result] = new ComparePattern(left, right);

            return result;
        }

        private void RecordRewrite(Expression source, Expression rewritten)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(rewritten != null, "rewritten != null");
            Debug.Assert(NormalizerRewrites != null, "this.NormalizerRewrites != null");

            if (source != rewritten)
            {
                NormalizerRewrites.Add(rewritten, source);
            }
        }

        private abstract class Pattern
        {
            internal abstract PatternKind Kind { get; }
        }

        private enum PatternKind
        {
            Compare,
        }

        private sealed class ComparePattern : Pattern
        {
            internal ComparePattern(Expression left, Expression right)
            {
                Left = left;
                Right = right;
            }

            internal readonly Expression Left;

            internal readonly Expression Right;

            internal override PatternKind Kind
            {
                get { return PatternKind.Compare; }
            }
        }
    }
    #endregion
}
