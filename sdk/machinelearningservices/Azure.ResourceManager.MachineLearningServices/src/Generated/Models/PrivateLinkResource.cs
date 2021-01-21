// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.MachineLearningServices.Models
{
    /// <summary> A private link resource. </summary>
    public partial class PrivateLinkResource : Resource
    {
        /// <summary> Initializes a new instance of PrivateLinkResource. </summary>
        public PrivateLinkResource()
        {
            RequiredMembers = new ChangeTrackingList<string>();
            RequiredZoneNames = new ChangeTrackingList<string>();
        }

        /// <summary> Initializes a new instance of PrivateLinkResource. </summary>
        /// <param name="id"> Specifies the resource ID. </param>
        /// <param name="name"> Specifies the name of the resource. </param>
        /// <param name="identity"> The identity of the resource. </param>
        /// <param name="location"> Specifies the location of the resource. </param>
        /// <param name="type"> Specifies the type of the resource. </param>
        /// <param name="tags"> Contains resource tags defined as key/value pairs. </param>
        /// <param name="sku"> The sku of the workspace. </param>
        /// <param name="groupId"> The private link resource group id. </param>
        /// <param name="requiredMembers"> The private link resource required member names. </param>
        /// <param name="requiredZoneNames"> The private link resource Private link DNS zone name. </param>
        internal PrivateLinkResource(string id, string name, Identity identity, string location, string type, IDictionary<string, string> tags, Sku sku, string groupId, IReadOnlyList<string> requiredMembers, IList<string> requiredZoneNames) : base(id, name, identity, location, type, tags, sku)
        {
            GroupId = groupId;
            RequiredMembers = requiredMembers;
            RequiredZoneNames = requiredZoneNames;
        }

        /// <summary> The private link resource group id. </summary>
        public string GroupId { get; }
        /// <summary> The private link resource required member names. </summary>
        public IReadOnlyList<string> RequiredMembers { get; }
        /// <summary> The private link resource Private link DNS zone name. </summary>
        public IList<string> RequiredZoneNames { get; }
    }
}