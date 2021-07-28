// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Query that already contains a SELECT and FROM clause.
    /// </summary>
    internal class WhereStatement : QueryBase
    {
        private readonly WhereLogic _upstreamWhereLogic;
        private readonly QueryAssembler _parent;

        internal WhereStatement(QueryAssembler parent, WhereLogic upstreamWhere)
        {
            _parent = parent;
            _upstreamWhereLogic = upstreamWhere;
        }

        /// <summary>
        /// Adds a WHERE statement to a query.
        /// </summary>
        /// <returns> Query that already contains a SELECT and FROM clause. </returns>
        public WhereLogic Where()
        {
            return _upstreamWhereLogic;
        }

        /// <inheritdoc/>
        public override QueryAssembler Build()
        {
            return _parent;
        }

        /// <inheritdoc/>
        public override string GetQueryText()
        {
            string whereLogicString = _upstreamWhereLogic.GetLogicText();

            if (!string.IsNullOrEmpty(whereLogicString))
            {
                return whereLogicString;
            }

            return whereLogicString;
        }
    }
}
