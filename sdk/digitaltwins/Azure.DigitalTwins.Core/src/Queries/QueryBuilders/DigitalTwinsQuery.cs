// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;
using System.Text;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// TODO.
    /// </summary>
    public class DigitalTwinsQuery<T>
    {
        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery() : this(AdtCollection.DigitalTwins) { }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="collection"></param>
        public DigitalTwinsQuery(AdtCollection collection)
        {
            _collection = collection switch
            {
                AdtCollection.DigitalTwins => "DigitalTwins",
                AdtCollection.Relationships => "Relationships",
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="customCollection"></param>
        public DigitalTwinsQuery(string customCollection)
        {
            _collection = customCollection;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery<T> Select(params string[] propertyNames)
        {
            _propertyNames ??= new List<string>();
            _propertyNames.AddRange(propertyNames);

            return this;
        }

        /// <summary>
        /// TODO
        /// </summary>
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

        private static void Ensure(bool condition, string message = null)
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

        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery<T> Take(int count)
        {
            _top = count;
            return this;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery<T> Count()
        {
            _count = true;
            return this;
        }

        /// <summary>
        /// TODO
        /// </summary>
        private DigitalTwinsQuery<T> Where(string filter)
        {
            _clauses ??= new List<string>();
            _clauses.Add(filter);

            return this;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery<T> Where(FormattableString filter) => Where(filter, null);

        /// <summary>
        /// TODO
        /// </summary>
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

        /// <summary>
        /// TODO
        /// </summary>
        public DigitalTwinsQuery<T> Where(Expression<Func<T, bool>> filter)
        {
            string query = string.Empty;
            Expression e = Evaluator.PartialEval(filter);
            e = ExpressionNormalizer.Normalize(e, new Dictionary<Expression, Expression>());

            LambdaExpression l = e as LambdaExpression;
            Ensure(l != null);
            Ensure(l.Parameters.Count == 1);

            query = QueryFilterVisitor.Translate(l.Body);

            return Where(query);
        }

        private string _collection;
        private int? _top;
        private bool _count;

        private List<string> _propertyNames;
        private List<string> _clauses;

        /// <summary>
        /// TODO
        /// </summary>
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

        /// <summary>
        /// TODO.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => GetQueryText();
    }
}
