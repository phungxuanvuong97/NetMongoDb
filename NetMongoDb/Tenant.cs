﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetMongoDb
{
    /// <summary>
    /// Tenant Entity
    /// The properties are for showcase only
    /// Additional tenant properties/data can be added, depending on the case
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// The tenant Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        /// <summary>
        /// The tenant identifier, dependant on the tenant identification strategy
        /// </summary>
        public string Identifier { get; set; }


        /// <summary>
        /// The tenant connection string to any type storage (SQL/NoSQL database, object storage, key/value storage etc.)
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The tenant database to any type storage (SQL/NoSQL database, object storage, key/value storage etc.)
        /// </summary>
        public string Database { get; set; }
    }
}
