// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azure.DigitalTwins.Core.Queries.QueryBuilders;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Query that already contains a SELECT and FROM clause.
    /// </summary>
    public sealed class WhereQuery : QueryBase
    {
        private readonly AdtQueryBuilder _parent;
        private IList<WhereClause> _clauses;

        internal WhereQuery(AdtQueryBuilder parent)
        {
            _parent = parent;
            _clauses = new List<WhereClause>();
        }

        /// <summary>
        /// Adds a WHERE and the conditional arguments for a comparison to the query object. Used to compare ADT properties
        /// using the query language's comparison operators.
        /// </summary>
        /// <param name="field"> The field being checked against a certain value. </param>
        /// <param name="operator"> The comparison operator being invoked. </param>
        /// <param name="value"> The value being checked against a Field. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereComparison(string field, QueryComparisonOperator @operator, string value)
        {
            _clauses.Add(new WhereClause(new ComparisonCondition(field, @operator, value)));
            return this;
        }

        /// <summary>
        /// An alternative way to add a WHERE clause to the query by directly providing a string that contains the condition.
        /// </summary>
        /// <param name="condition"> The verbatim condition (SQL-like syntax) in string format. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereOverride(string condition)
        {
            Console.WriteLine(condition);
            return this;
        }

        /// <summary>
        /// Adds the <see href="https://docs.microsoft.com/en-us/azure/digital-twins/reference-query-functions#is_defined">IS_DEFINED</see> function to the condition statement of the query.
        /// </summary>
        /// <param name="property"> The property that the query is looking for as defined. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereIsDefined(string property)
        {
            Console.WriteLine(property);
            return this;
        }

        /// <summary>
        /// Adds the <see href="https://docs.microsoft.com/en-us/azure/digital-twins/reference-query-functions#is_null">IS_NULL</see> function to the condition statement of the query.
        /// </summary>
        /// <param name="expression"> The expression being checked for null. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereIsNull(string expression)
        {
            Console.WriteLine(expression);
            return this;
        }

        /// <summary>
        /// Adds the <see href="https://docs.microsoft.com/en-us/azure/digital-twins/reference-query-functions#startswith">STARTSWITH</see> function to the condition statement of the query.
        /// </summary>
        /// <param name="stringToCheck"> String to check the beginning of. </param>
        /// <param name="beginningString"> String representing the beginning to check for. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereStartsWith(string stringToCheck, string beginningString)
        {
            Console.Write(stringToCheck);
            Console.WriteLine(beginningString);
            return this;
        }

        /*
         WhereEndsWith defined in a similar manner.
         */

        /// <summary>
        /// Adds the <see href="https://docs.microsoft.com/en-us/azure/digital-twins/reference-query-functions#is_of_model">IS_OF_MODEL</see> function to the condition statement of the query.
        /// </summary>
        /// <param name="model"> Model ID to check for. </param>
        /// <param name="exact"> Whether or not an exact match is required. </param>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery WhereIsOfModel(string model, bool exact = false)
        {
            Console.WriteLine(model);
            Console.WriteLine(exact);
            return this;
        }

        /// <summary>
        /// Adds the logical operator AND to the query.
        /// </summary>
        /// <returns> ADT query that already contains SELECT and FROM. </returns>
        public WhereQuery And()
        {
            return this;
        }

        /// <inheritdoc/>
        public override AdtQueryBuilder Build()
        {
            return _parent;
        }

        /// <inheritdoc/>
        public override string Stringify()
        {
            if (_clauses.Any())
            {
                // Where keyword only needs to be appened one time, happends outside of loop
                StringBuilder whereComponents = new StringBuilder();
                whereComponents.Append($"{QueryConstants.Where} ");

                // Parse each Where conditional statement
                foreach (WhereClause _clause in _clauses)
                {
                    if (_clause.Condition != null)
                    {
                        whereComponents.Append(_clause.Condition.Stringify());
                    }
                }

                return whereComponents.ToString().Trim();
            }

            return string.Empty;
        }

        /*
        * The rest of the logical operators defined in a similar manner.
        */
    }
}
