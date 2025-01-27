// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Azure.Storage.Queues.Models
{
    /// <summary> The object returned when calling List Queues on a Queue Service. </summary>
    internal partial class ListQueuesSegmentResponse
    {
        /// <summary> Initializes a new instance of ListQueuesSegmentResponse. </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="prefix"></param>
        /// <param name="maxResults"></param>
        /// <param name="nextMarker"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="serviceEndpoint"/>, <paramref name="prefix"/>, or <paramref name="nextMarker"/> is null. </exception>
        internal ListQueuesSegmentResponse(string serviceEndpoint, string prefix, int maxResults, string nextMarker)
        {
            if (serviceEndpoint == null)
            {
                throw new ArgumentNullException(nameof(serviceEndpoint));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            if (nextMarker == null)
            {
                throw new ArgumentNullException(nameof(nextMarker));
            }

            ServiceEndpoint = serviceEndpoint;
            Prefix = prefix;
            MaxResults = maxResults;
            QueueItems = new ChangeTrackingList<QueueItem>();
            NextMarker = nextMarker;
        }

        /// <summary> Initializes a new instance of ListQueuesSegmentResponse. </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="prefix"></param>
        /// <param name="marker"></param>
        /// <param name="maxResults"></param>
        /// <param name="queueItems"></param>
        /// <param name="nextMarker"></param>
        internal ListQueuesSegmentResponse(string serviceEndpoint, string prefix, string marker, int maxResults, IReadOnlyList<QueueItem> queueItems, string nextMarker)
        {
            ServiceEndpoint = serviceEndpoint;
            Prefix = prefix;
            Marker = marker;
            MaxResults = maxResults;
            QueueItems = queueItems;
            NextMarker = nextMarker;
        }

        public string ServiceEndpoint { get; }
        public string Prefix { get; }
        public string Marker { get; }
        public int MaxResults { get; }
        public IReadOnlyList<QueueItem> QueueItems { get; }
        public string NextMarker { get; }
    }
}
