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
            #region Snippet:DigitalTwinsQueryBuilderNonGeneric
            new DigitalTwinsQueryBuilderV2().Build();
            new DigitalTwinsQueryBuilderV2<BasicDigitalTwin>().Build();

            // SELECT * FROM DigitalTwins
            new DigitalTwinsQueryBuilderV2().Build().GetQueryText();
            new DigitalTwinsQueryBuilderV2<BasicDigitalTwin>().Build().GetQueryText();
            #endregion
            #region Snippet:DigitalTwinsQueryBuilder

            // SELECT * FROM DigitalTwins
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> simplestQuery = new DigitalTwinsQueryBuilderV2().Build();

            // SELECT * FROM Relationsips
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> simplestQueryRelationships = new DigitalTwinsQueryBuilderV2(DigitalTwinsCollection.Relationships)
                .Build();

            // Use LINQ expressions to select defined properties in type T of DigitalTwinsQueryBuilder
            // SELECT Temperature From DigitalTwins
            DigitalTwinsQueryBuilderV2<ConferenceRoom> selectSingleProperty = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Select(r => r.Temperature)
                .Build();

            // SELECT TOP(3) FROM DIGITALTWINS
            DigitalTwinsQueryBuilderV2<ConferenceRoom> queryWithSelectTop = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Take(3)
                .Build();

            // Strings are valid ways to denote selectable properties as an alternative to LINQ expressions
            // SELECT TOP(3) Temperature, Humidity FROM DIGITALTWINS
            DigitalTwinsQueryBuilderV2<ConferenceRoom> queryWithSelectTopProperty = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Select("Temperature", "Humidity")
                .Take(3)
                .Build();

            // SELECT COUNT() FROM RELATIONSHIPS
            DigitalTwinsQueryBuilderV2<ConferenceRoom> queryWithSelectRelationships = new DigitalTwinsQueryBuilderV2<ConferenceRoom>(DigitalTwinsCollection.Relationships)
                .Count()
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL("dtmi:example:room;1")
            DigitalTwinsQueryBuilderV2<ConferenceRoom> queryWithIsOfModel = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1"))
                .Build();

            #endregion Snippet:DigitalTwinsQueryBuilder

            #region Snippet:DigitalTwinsQueryBuilderToString

            string basicQueryStringFormat = new DigitalTwinsQueryBuilderV2<ConferenceRoom>().Build().GetQueryText();

            #endregion Snippet:DigitalTwinsQueryBuilderToString

            #region Snippet:DigitalTwinsQueryBuilderBuild
            // construct query and build string representation
            DigitalTwinsQueryBuilderV2<ConferenceRoom> builtQuery = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Take(5)
                .Where(r => r.Temperature > 50)
                .Build();
            
            // SELECT TOP(5) From DigitalTwins WHERE Temperature > 50
            string builtQueryString = builtQuery.GetQueryText();

            // if not rebuilt, string representation does not update, even if new methods are chained
            // SELECT TOP(5) From DigitalTwins WHERE Temperature > 50
            builtQuery.Select("Humidity").GetQueryText();

            // string representation updated after Build() called again
            // SELECT TOP(5) Humidity From DigitalTwins WHERE Temperature > 50
            builtQuery.Build().GetQueryText();
            #endregion

            #region Snippet:DigitalTwinsQueryBuilderFromMethod
            // SELECT Temperature FROM DigitalTwins
            new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Select(r => r.Temperature)
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build();

            // pass in an optional string as a second parameter of From() to alias a collection
            // SELECT Temperature FROM DigitalTwins T
            new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Select(r => r.Temperature)
                .From(DigitalTwinsCollection.DigitalTwins, "T")
                .Build();

            // SELECT Temperature FROM DigitalTwins
            new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Select(r => r.Temperature)
                .FromCustom("DigitalTwins")
                .Build();
            #endregion

            // SELECT Room, Temperature From DIGTIALTWINS
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> queryWithMultipleProperties = new DigitalTwinsQueryBuilderV2()
                .Select("Room", "Temperature")
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE TEMPERATURE < 5
            DigitalTwinsQueryBuilderV2<ConferenceRoom> queryWithComparisonWhereClause = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(r => r.Temperature < 5)
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('dtmi:example:room;1', exact)
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> queryWithIsOfModelExact = new DigitalTwinsQueryBuilderV2()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true))
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 AND IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilderV2<ConferenceRoom> logicalOps_SingleAnd = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(r => r.Temperature == 50 && DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true)).Build();

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilderV2<ConferenceRoom> logicalOps_SingleOr = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(r => (r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true)) && r.IsOccupied == true).Build();

            #region Snippet:DigitalTwinsQueryBuilder_ComplexConditions
            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact) OR IS_NUMBER(Temperature)
            DigitalTwinsQueryBuilderV2<ConferenceRoom> logicalOps_MultipleOr = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(r => r.Temperature == 50 || 
                DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true) || 
                DigitalTwinsFunctions.IsNumber(r.Temperature))
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE (IS_NUMBER(Humidity) OR IS_DEFINED(Humidity)) 
            // OR (IS_OF_MODEL("dtmi:example:hvac;1") AND IS_NULL(Occupants))
            DigitalTwinsQueryBuilderV2<ConferenceRoom> logicalOpsNested = new DigitalTwinsQueryBuilderV2<ConferenceRoom>()
                .Where(r => 
                    (DigitalTwinsFunctions.IsNumber(r.Humidity) 
                        || DigitalTwinsFunctions.IsDefined(r.Humidity))
                    &&
                    (DigitalTwinsFunctions.IsOfModel("dtmi:example:hvac;1") 
                        && DigitalTwinsFunctions.IsNull(r.Occupants)))
                .Build();

            #endregion

            #region Snippet:DigitalTwinsQueryBuilderOverride
            // SELECT TOP(3) Room, Temperature FROM DIGITALTWINS
            new DigitalTwinsQueryBuilderV2()
            .SelectCustom("TOP(3) Room, Temperature")
            .Build();
            #endregion

            #region Snippet:DigitalTwinsQueryBuilder_Aliasing
            // SELECT Temperature AS Temp, Humidity AS HUM FROM DigitalTwins
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> selectAsSample = new DigitalTwinsQueryBuilderV2(DigitalTwinsCollection.DigitalTwins, "T")
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .Build();

            // SELECT Temperature, Humidity AS Hum FROM DigitalTwins
            DigitalTwinsQueryBuilderV2<BasicDigitalTwin> selectAndSelectAs = new DigitalTwinsQueryBuilderV2()
                .Select("Temperature")
                .SelectAs("Humidity", "Hum")
                .From(DigitalTwinsCollection.DigitalTwins, "T")
                .Build();
            #endregion
        }
    }
}
