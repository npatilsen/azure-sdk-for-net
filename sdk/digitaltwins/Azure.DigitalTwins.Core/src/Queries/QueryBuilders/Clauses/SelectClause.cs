// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    /// Custom object for a SELECT clause. Only meant to be used when adding SELECT to a query. Hidden from user.
    /// </summary>
    internal class SelectClause : BaseClause
    {
        /// <summary>
        /// The argument for the SELECT clause (eg. *).
        /// </summary>
        public string ClauseArg { get; set; }

        /// <summary>
        /// Constructor for SELECT clause.
        /// </summary>
        /// <param name="argument"> Argument for what to select (collection, property, etc.). </param>
        public SelectClause(string argument)
        {
            // TODO -- select multiple arguments (string[])
            Type = ClauseType.SELECT;
            ClauseArg = argument;
        }

        public override string Stringify()
        {
            List<string> selectClauseComponents = new List<string>();

            // one argument -- TODO multiple arguments
            selectClauseComponents.Add("SELECT");
            selectClauseComponents.Add(ClauseArg);

            return string.Join(" ", selectClauseComponents);
        }
    }
}
