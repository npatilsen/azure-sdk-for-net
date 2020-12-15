// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.Media.Analytics.Edge.Models
{
    /// <summary> A node that accepts raw video as input, and detects if there are moving objects present. If so, then it emits an event, and allows frames where motion was detected to pass through. Other frames are blocked/dropped. </summary>
    public partial class MediaGraphMotionDetectionProcessor : MediaGraphProcessor
    {
        /// <summary> Initializes a new instance of MediaGraphMotionDetectionProcessor. </summary>
        /// <param name="name"> The name for this processor node. </param>
        /// <param name="inputs"> An array of the names of the other nodes in the media graph, the outputs of which are used as input for this processor node. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> or <paramref name="inputs"/> is null. </exception>
        public MediaGraphMotionDetectionProcessor(string name, IEnumerable<MediaGraphNodeInput> inputs) : base(name, inputs)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }

            Type = "#Microsoft.Media.MediaGraphMotionDetectionProcessor";
        }

        /// <summary> Initializes a new instance of MediaGraphMotionDetectionProcessor. </summary>
        /// <param name="type"> The discriminator for derived types. </param>
        /// <param name="name"> The name for this processor node. </param>
        /// <param name="inputs"> An array of the names of the other nodes in the media graph, the outputs of which are used as input for this processor node. </param>
        /// <param name="sensitivity"> Enumeration that specifies the sensitivity of the motion detection processor. </param>
        /// <param name="outputMotionRegion"> Indicates whether the processor should detect and output the regions, within the video frame, where motion was detected. Default is true. </param>
        /// <param name="eventAggregationWindow"> Event aggregation window duration, or 0 for no aggregation. </param>
        internal MediaGraphMotionDetectionProcessor(string type, string name, IList<MediaGraphNodeInput> inputs, MediaGraphMotionDetectionSensitivity? sensitivity, bool? outputMotionRegion, string eventAggregationWindow) : base(type, name, inputs)
        {
            Sensitivity = sensitivity;
            OutputMotionRegion = outputMotionRegion;
            EventAggregationWindow = eventAggregationWindow;
            Type = type ?? "#Microsoft.Media.MediaGraphMotionDetectionProcessor";
        }

        /// <summary> Enumeration that specifies the sensitivity of the motion detection processor. </summary>
        public MediaGraphMotionDetectionSensitivity? Sensitivity { get; set; }
        /// <summary> Indicates whether the processor should detect and output the regions, within the video frame, where motion was detected. Default is true. </summary>
        public bool? OutputMotionRegion { get; set; }
        /// <summary> Event aggregation window duration, or 0 for no aggregation. </summary>
        public string EventAggregationWindow { get; set; }
    }
}