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

            // SELECT * FROM DigitalTwins
            DigitalTwinsQueryBuilder<ConferenceRoom> simplestQuery = new DigitalTwinsQueryBuilder<ConferenceRoom>();

            // SELECT * FROM Relationsips
            DigitalTwinsQueryBuilder<ConferenceRoom> simplestQueryRelationships = new DigitalTwinsQueryBuilder<ConferenceRoom>(DigitalTwinsCollection.Relationships);

            // SELECT TOP(3) FROM DIGITALTWINS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectTop = new DigitalTwinsQueryBuilder<ConferenceRoom>().Take(3);

            // SELECT TOP(3) Temperature, Humidity FROM DIGITALTWINS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectTopProperty = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select("Temperature", "Humidity")
                .Take(3);

            // SELECT COUNT() FROM RELATIONSHIPS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectRelationships = new DigitalTwinsQueryBuilder<ConferenceRoom>(DigitalTwinsCollection.Relationships)
                .Count();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL("dtmi:example:room;1")
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithIsOfModel = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1"));

            #endregion Snippet:DigitalTwinsQueryBuilder

            #region Snippet:DigitalTwinsQueryBuilderToString

            string basicQueryStringFormat = new DigitalTwinsQueryBuilder<ConferenceRoom>().GetQueryText();

            #endregion Snippet:DigitalTwinsQueryBuilderToString

            // SELECT Room, Temperature From DIGTIALTWINS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithMultipleProperties = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select("Room", "Temperature");

            // SELECT * FROM DIGITALTWINS WHERE TEMPERATURE < 5
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithComparisonWhereClause = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature < 5);

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('dtmi:example:room;1', exact)
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithIsOfModelExact = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true));

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 AND IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_SingleAnd = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature == 50 && DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true));

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_SingleOr = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => (r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true)) && r.IsOccupied == true);

            #region Snippet:DigitalTwinsQueryBuilder_ComplexConditions
            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact) OR IS_NUMBER(Temperature)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_MultipleOr = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true) || DigitalTwinsFunctions.IsNumber(r.Temperature));

            // SELECT * FROM DIGITALTWINS WHERE (IS_NUMBER(Humidity) OR IS_DEFINED(Humidity)) 
            // OR (IS_OF_MODEL("dtmi:example:hvac;1") AND IS_NULL(Occupants))
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOpsNested = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => (DigitalTwinsFunctions.IsNumber(r.Humidity) || DigitalTwinsFunctions.IsDefined(r.Humidity))
                            &&
                            (DigitalTwinsFunctions.IsOfModel("dtmi:example:hvac;1") && DigitalTwinsFunctions.IsNull(r.Occupants)));

            #endregion

            #region Snippet:DigitalTwinsQueryBuilderOverride
            // SELECT TOP(3) Room, Temperature FROM DIGITALTWINS
            new DigitalTwinsQueryBuilder<ConferenceRoom>()
            .SelectCustom("TOP(3) Room, Temperature");
            #endregion

            #region Snippet:DigitalTwinsQueryBuilder_Aliasing
            // SELECT Temperature AS Temp, Humidity AS HUM FROM DigitalTwins
            DigitalTwinsQueryBuilder<ConferenceRoom> selectAsSample = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum");

            // SELECT Temperature, Humidity AS Hum FROM DigitalTwins
            DigitalTwinsQueryBuilder<ConferenceRoom> selectAndSelectAs = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select("Temperature")
                .SelectAs("Humidity", "Hum");
            #endregion
        }
    }
}
