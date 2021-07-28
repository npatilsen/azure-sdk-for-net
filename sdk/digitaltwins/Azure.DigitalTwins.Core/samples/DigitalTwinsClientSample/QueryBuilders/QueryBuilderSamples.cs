// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.DigitalTwins.Core.QueryBuilder;

namespace Azure.DigitalTwins.Core.Samples
{
    /// <summary>
    /// Samples for QueryBuilder.
    /// </summary>
    public static class QueryBuilderSamples
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            #region Snippet:DigitalTwinsQueryBuilder

            // SELECT * FROM DIGITALTWINS
            DigitalTwinsQuery<ConferenceRoom> simplestQuery = new DigitalTwinsQuery<ConferenceRoom>();

            // SELECT TOP(3) FROM DIGITALTWINS
            // Note that if no property is specfied, the SelectTopAll() method can be used instead of SelectTop()
            DigitalTwinsQuery<ConferenceRoom> queryWithSelectTop = new DigitalTwinsQuery<ConferenceRoom>().Take(3);

            // SELECT TOP(3) Temperature, Humidity FROM DIGITALTWINS
            DigitalTwinsQuery<ConferenceRoom> queryWithSelectTopProperty = new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Temperature", "Humidity")
                .Take(3);

            // SELECT COUNT() FROM RELATIONSHIPS
            DigitalTwinsQuery<ConferenceRoom> queryWithSelectRelationships = new DigitalTwinsQuery<ConferenceRoom>()
                .Count();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL("dtmi:example:room;1")
            DigitalTwinsQuery<ConferenceRoom> queryWithIsOfModel = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1"));

            #endregion Snippet:DigitalTwinsQueryBuilder

            #region Snippet:DigitalTwinsQueryBuilderToString

            string basicQueryStringFormat = new DigitalTwinsQuery<ConferenceRoom>().GetQueryText();

            #endregion Snippet:DigitalTwinsQueryBuilderToString

            // SELECT Room, Temperature From DIGTIALTWINS
            DigitalTwinsQuery<ConferenceRoom> queryWithMultipleProperties = new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Room", "Temperature");

            // SELECT * FROM DIGITALTWINS WHERE TEMPERATURE < 5
            DigitalTwinsQuery<ConferenceRoom> queryWithComparisonWhereClause = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature < 5);

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('dtmi:example:room;1', exact)
            DigitalTwinsQuery<ConferenceRoom> queryWithIsOfModelExact = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true));

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 AND IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQuery<ConferenceRoom> logicalOps_SingleAnd = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature == 50 && DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true));

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQuery<ConferenceRoom> logicalOps_SingleOr = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true));

            #region Snippet:DigitalTwinsQueryBuilder_ComplexConditions
            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact) OR IS_NUMBER(Temperature)
            DigitalTwinsQuery<ConferenceRoom> logicalOps_MultipleOr = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true) || DigitalTwinsFunctions.IsNumber(r.Temperature));

            // SELECT * FROM DIGITALTWINS WHERE (IS_NUMBER(Humidity) OR IS_DEFINED(Humidity)) 
            // OR (IS_OF_MODEL("dtmi:example:hvac;1") AND IS_NULL(Occupants))
            DigitalTwinsQuery<ConferenceRoom> logicalOpsNested = new DigitalTwinsQuery<ConferenceRoom>()
                .Where(r => (DigitalTwinsFunctions.IsNumber(r.Humidity) || DigitalTwinsFunctions.IsDefined(r.Humidity))
                            &&
                            (DigitalTwinsFunctions.IsOfModel("dtmi:example:hvac;1") && DigitalTwinsFunctions.IsNull(r.Occupants)));

            #endregion

            #region Snippet:DigitalTwinsQueryBuilderOverride
            // SELECT TOP(3) Room, Temperature FROM DIGITALTWINS
            new DigitalTwinsQuery<ConferenceRoom>()
            .SelectCustom("TOP(3) Room, Temperature");
            #endregion

            #region Snippet:DigitalTwinsQueryBuilder_Aliasing
            // SELECT Temperature AS Temp, Humidity AS HUM FROM DigitalTwins
            DigitalTwinsQuery<ConferenceRoom> selectAsSample = new DigitalTwinsQuery<ConferenceRoom>()
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum");

            // SELECT Temperature, Humidity AS Hum FROM DigitalTwins
            DigitalTwinsQuery<ConferenceRoom> selectAndSelectAs = new DigitalTwinsQuery<ConferenceRoom>()
                .Select("Temperature")
                .SelectAs("Humidity", "Hum");
            #endregion
        }
    }
}
