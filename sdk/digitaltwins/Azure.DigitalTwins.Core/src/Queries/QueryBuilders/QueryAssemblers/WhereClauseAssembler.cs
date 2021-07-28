// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Query that already contains a SELECT and FROM clause.
    /// </summary>
    internal class WhereClauseAssembler : QueryBase
    {
        private readonly WhereClauseAssemblerLogic _upstreamWhereLogic;
        private readonly QueryAssembler _parent;

        internal WhereClauseAssembler(QueryAssembler parent, WhereClauseAssemblerLogic upstreamWhere)
        {
            _parent = parent;
            _upstreamWhereLogic = upstreamWhere;
        }

        /// <summary>
        /// Adds a WHERE statement to a query.
        /// </summary>
        /// <returns> Query that already contains a SELECT and FROM clause. </returns>
        public WhereClauseAssemblerLogic Where()
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
