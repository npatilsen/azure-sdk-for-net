// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.DigitalTwins.Core.Queries.QueryBuilder;

namespace Azure.DigitalTwins.Core.QueryBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class DigitalTwinsQueryBuilder
    {
        private readonly string _alias;
        private readonly AdtCollection _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalTwinsQueryBuilder"/> class.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="alias"></param>
        public DigitalTwinsQueryBuilder(AdtCollection collection, string alias = null)
        {
            _collection = collection;
            _alias = alias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalTwinsQueryBuilder"/> class.
        /// </summary>
        public DigitalTwinsQueryBuilder()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DigitalTwinsQueryBuilder Select(params string[] args)
        {
            Console.Write(args);
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public DigitalTwinsQueryBuilder SelectAs(string property, string alias)
        {
            Console.WriteLine($"{property}{alias}");
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public DigitalTwinsQueryBuilder From(AdtCollection collection, string alias = null)
        {
            Console.WriteLine($"{collection}{alias}");
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public DigitalTwinsQueryBuilder Where(Func<WhereQuery, WhereQuery> logic)
        {
            logic.Invoke(null);
            return this;
        }
    }
}
