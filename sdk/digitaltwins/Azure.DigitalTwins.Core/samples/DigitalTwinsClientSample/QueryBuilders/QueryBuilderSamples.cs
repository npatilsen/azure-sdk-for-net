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
            new DigitalTwinsQueryBuilder().Build();
            new DigitalTwinsQueryBuilder<BasicDigitalTwin>().Build();

            // SELECT * FROM DigitalTwins
            new DigitalTwinsQueryBuilder().Build().GetQueryText();
            new DigitalTwinsQueryBuilder<BasicDigitalTwin>().Build().GetQueryText();
            #endregion
            #region Snippet:DigitalTwinsQueryBuilder

            // SELECT * FROM DigitalTwins
            DigitalTwinsQueryBuilder<BasicDigitalTwin> simplestQuery = new DigitalTwinsQueryBuilder().Build();

            // SELECT * FROM Relationsips
            DigitalTwinsQueryBuilder<BasicDigitalTwin> simplestQueryRelationships = new DigitalTwinsQueryBuilder(DigitalTwinsCollection.Relationships)
                .Build();

            // Use LINQ expressions to select defined properties in type T of DigitalTwinsQueryBuilder
            // SELECT Temperature From DigitalTwins
            DigitalTwinsQueryBuilder<ConferenceRoom> selectSingleProperty = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select(r => r.Temperature)
                .Build();

            // SELECT TOP(3) FROM DIGITALTWINS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectTop = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Take(3)
                .Build();

            // Strings are valid ways to denote selectable properties as an alternative to LINQ expressions
            // SELECT TOP(3) Temperature, Humidity FROM DIGITALTWINS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectTopProperty = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select("Temperature", "Humidity")
                .Take(3)
                .Build();

            // SELECT COUNT() FROM RELATIONSHIPS
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithSelectRelationships = new DigitalTwinsQueryBuilder<ConferenceRoom>(DigitalTwinsCollection.Relationships)
                .Count()
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL("dtmi:example:room;1")
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithIsOfModel = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1"))
                .Build();

            #endregion Snippet:DigitalTwinsQueryBuilder

            #region Snippet:DigitalTwinsQueryBuilderToString

            string basicQueryStringFormat = new DigitalTwinsQueryBuilder<ConferenceRoom>().Build().GetQueryText();

            #endregion Snippet:DigitalTwinsQueryBuilderToString

            #region Snippet:DigitalTwinsQueryBuilderBuild
            // construct query and build string representation
            DigitalTwinsQueryBuilder<ConferenceRoom> builtQuery = new DigitalTwinsQueryBuilder<ConferenceRoom>()
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
            new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select(r => r.Temperature)
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build();

            // pass in an optional string as a second parameter of From() to alias a collection
            // SELECT Temperature FROM DigitalTwins T
            new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select(r => r.Temperature)
                .From(DigitalTwinsCollection.DigitalTwins, "T")
                .Build();

            // SELECT Temperature FROM DigitalTwins
            new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Select(r => r.Temperature)
                .FromCustom("DigitalTwins")
                .Build();
            #endregion

            // SELECT Room, Temperature From DIGTIALTWINS
            DigitalTwinsQueryBuilder<BasicDigitalTwin> queryWithMultipleProperties = new DigitalTwinsQueryBuilder()
                .Select("Room", "Temperature")
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE TEMPERATURE < 5
            DigitalTwinsQueryBuilder<ConferenceRoom> queryWithComparisonWhereClause = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature < 5)
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('dtmi:example:room;1', exact)
            DigitalTwinsQueryBuilder<BasicDigitalTwin> queryWithIsOfModelExact = new DigitalTwinsQueryBuilder()
                .Where(_ => DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true))
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 AND IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_SingleAnd = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature == 50 && DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true)).Build();

            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_SingleOr = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => (r.Temperature == 50 || DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true)) && r.IsOccupied == true).Build();

            #region Snippet:DigitalTwinsQueryBuilder_ComplexConditions
            // SELECT * FROM DIGITALTWINS WHERE Temperature = 50 OR IS_OF_MODEL("dtmi..", exact) OR IS_NUMBER(Temperature)
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOps_MultipleOr = new DigitalTwinsQueryBuilder<ConferenceRoom>()
                .Where(r => r.Temperature == 50 || 
                DigitalTwinsFunctions.IsOfModel("dtmi:example:room;1", true) || 
                DigitalTwinsFunctions.IsNumber(r.Temperature))
                .Build();

            // SELECT * FROM DIGITALTWINS WHERE (IS_NUMBER(Humidity) OR IS_DEFINED(Humidity)) 
            // OR (IS_OF_MODEL("dtmi:example:hvac;1") AND IS_NULL(Occupants))
            DigitalTwinsQueryBuilder<ConferenceRoom> logicalOpsNested = new DigitalTwinsQueryBuilder<ConferenceRoom>()
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
            new DigitalTwinsQueryBuilder()
            .SelectCustom("TOP(3) Room, Temperature")
            .Build();
            #endregion

            #region Snippet:DigitalTwinsQueryBuilder_Aliasing
            // SELECT Temperature AS Temp, Humidity AS HUM FROM DigitalTwins
            DigitalTwinsQueryBuilder<BasicDigitalTwin> selectAsSample = new DigitalTwinsQueryBuilder(DigitalTwinsCollection.DigitalTwins, "T")
                .SelectAs("Temperature", "Temp")
                .SelectAs("Humidity", "Hum")
                .Build();

            // SELECT Temperature, Humidity AS Hum FROM DigitalTwins
            DigitalTwinsQueryBuilder<BasicDigitalTwin> selectAndSelectAs = new DigitalTwinsQueryBuilder()
                .Select("Temperature")
                .SelectAs("Humidity", "Hum")
                .From(DigitalTwinsCollection.DigitalTwins, "T")
                .Build();
            #endregion
        }
    }
}
