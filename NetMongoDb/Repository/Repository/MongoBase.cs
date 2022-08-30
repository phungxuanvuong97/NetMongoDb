using NetMongoDb.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMongoDb
{
    public abstract class MongoBase<TEntity>
    {
        protected static readonly FilterDefinitionBuilder<TEntity> Filter =
            Builders<TEntity>.Filter;

        protected static readonly IndexKeysDefinitionBuilder<TEntity> Index =
            Builders<TEntity>.IndexKeys;

        protected static readonly ProjectionDefinitionBuilder<TEntity> Projection =
            Builders<TEntity>.Projection;

        protected static readonly SortDefinitionBuilder<TEntity> Sort =
            Builders<TEntity>.Sort;

        protected static readonly UpdateDefinitionBuilder<TEntity> UpdateDef =
            Builders<TEntity>.Update;

        protected static readonly BulkWriteOptions BulkUnordered =
            new BulkWriteOptions { IsOrdered = true };

        protected static readonly InsertManyOptions InsertUnordered =
            new InsertManyOptions { IsOrdered = true };

        protected static readonly ReplaceOptions UpsertReplace =
            new ReplaceOptions { IsUpsert = true };

        protected static readonly UpdateOptions Upsert =
            new UpdateOptions { IsUpsert = true };

        protected static readonly BsonDocument FindAll =
            new BsonDocument();


        static MongoBase()
        {
            //register Serializer, convention...etc

        }
    }
}
