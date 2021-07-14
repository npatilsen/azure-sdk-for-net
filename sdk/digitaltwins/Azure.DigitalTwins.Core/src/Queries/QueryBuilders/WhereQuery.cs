// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.DigitalTwins.Core.QueryBuilder;

namespace Azure.DigitalTwins.Core.Queries.QueryBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class WhereQuery
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public WhereQuery Custom(string condition)
        {
            Console.WriteLine(condition);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public WhereQuery IsOfModel(string property, string modelId)
        {
            Console.WriteLine($"{property}{modelId}");
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public WhereQuery IsOftype(string property, AdtDataType modelId)
        {
            Console.WriteLine($"{property}{modelId}");
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereQuery And()
        {
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereQuery Or()
        {
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public WhereQuery And(Func<WhereQuery, WhereQuery> nested)
        {
            nested.Invoke(null);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public WhereQuery Or(Func<WhereQuery, WhereQuery> nested)
        {
            nested.Invoke(null);
            return this;
        }
    }
}
