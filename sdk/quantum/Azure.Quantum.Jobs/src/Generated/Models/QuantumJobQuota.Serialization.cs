// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Quantum.Jobs.Models
{
    public partial class QuantumJobQuota
    {
        internal static QuantumJobQuota DeserializeQuantumJobQuota(JsonElement element)
        {
            Optional<string> dimension = default;
            Optional<DimensionScope> scope = default;
            Optional<string> providerId = default;
            Optional<float> utilization = default;
            Optional<float> holds = default;
            Optional<float> limit = default;
            Optional<MeterPeriod> period = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("dimension"))
                {
                    dimension = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("scope"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    scope = new DimensionScope(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("providerId"))
                {
                    providerId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("utilization"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    utilization = property.Value.GetSingle();
                    continue;
                }
                if (property.NameEquals("holds"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    holds = property.Value.GetSingle();
                    continue;
                }
                if (property.NameEquals("limit"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    limit = property.Value.GetSingle();
                    continue;
                }
                if (property.NameEquals("period"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    period = new MeterPeriod(property.Value.GetString());
                    continue;
                }
            }
            return new QuantumJobQuota(dimension.Value, Optional.ToNullable(scope), providerId.Value, Optional.ToNullable(utilization), Optional.ToNullable(holds), Optional.ToNullable(limit), Optional.ToNullable(period));
        }
    }
}