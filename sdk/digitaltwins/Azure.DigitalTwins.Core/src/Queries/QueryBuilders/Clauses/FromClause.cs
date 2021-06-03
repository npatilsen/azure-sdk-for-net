// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Custom object for a FROM clause. Only meant to be used when adding FROM to a query. Hidden from user.
    /// </summary>
    internal class FromClause : BaseClause
    {
        /// <summary>
        /// The collection to query from. Stored in an enum to give user some intellisense since there are only two possible collection types,
        /// ADT instances or relationships.
        /// </summary>
        public AdtCollection Collection { get; set; }

        /// <summary>
        /// Constructor for a FROM clause.
        /// </summary>
        /// <param name="collection"> Enum Collection that can be either a ADT instance or a relationship. </param>
        public FromClause(AdtCollection collection)
        {
            Type = ClauseType.FROM;
            Collection = collection;
        }

        public override string Stringify()
        {
            List<string> fromClauseComponents = new List<string>();
            fromClauseComponents.Add("FROM");

            // check what kind of collection
            if (Collection == AdtCollection.DigitalTwins)
            {
                // TODO - support aliasing (ex. DIGITALTWINS DT)
                fromClauseComponents.Add("DIGITALTWINS");
            }
            else
            {
                fromClauseComponents.Add("RELATIONSHIPS");
            }

            return string.Join(" ", fromClauseComponents);
        }
    }
}
