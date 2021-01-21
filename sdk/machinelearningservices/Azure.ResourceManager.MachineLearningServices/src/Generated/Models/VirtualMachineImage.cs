// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.ResourceManager.MachineLearningServices.Models
{
    /// <summary> Virtual Machine image for Windows AML Compute. </summary>
    public partial class VirtualMachineImage
    {
        /// <summary> Initializes a new instance of VirtualMachineImage. </summary>
        /// <param name="id"> Virtual Machine image path. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public VirtualMachineImage(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }

        /// <summary> Virtual Machine image path. </summary>
        public string Id { get; set; }
    }
}