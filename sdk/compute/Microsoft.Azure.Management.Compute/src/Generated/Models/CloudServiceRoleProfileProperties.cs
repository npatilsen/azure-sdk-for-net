// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Compute.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Describes the role properties.
    /// </summary>
    public partial class CloudServiceRoleProfileProperties
    {
        /// <summary>
        /// Initializes a new instance of the CloudServiceRoleProfileProperties
        /// class.
        /// </summary>
        public CloudServiceRoleProfileProperties()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CloudServiceRoleProfileProperties
        /// class.
        /// </summary>
        /// <param name="name">Resource name.</param>
        public CloudServiceRoleProfileProperties(string name = default(string), CloudServiceRoleSku sku = default(CloudServiceRoleSku))
        {
            Name = name;
            Sku = sku;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets resource name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public CloudServiceRoleSku Sku { get; set; }

    }
}