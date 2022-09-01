using NetMongoDb.Collections;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NetMongoDb.Repository
{
    public abstract class Repository<TEntity> : MongoBase<TEntity>, IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoDatabase mongoDatabase;
        private IMongoCollection<TEntity> mongoCollection;
        private readonly IMongoClient mongoClient;

        protected IMongoCollection<TEntity> Collection
        {
            get
            {
                if (mongoCollection == null)
                {
                    InitializeAsync(default).Wait();
                }

                if (mongoCollection == null)
                {
                    throw new Exception("Collection has not been initialized yet.");
                    return default!;
                }

                return mongoCollection;
            }
        }

        protected IMongoClient MongoClient
        {
            get { return mongoClient; }
        }

        protected IMongoDatabase Database
        {
            get => mongoDatabase;
        }

        protected Repository(TenantService<Tenant> tenantService)
        {
            var tenant = tenantService.GetTenantAsync().GetAwaiter().GetResult();
            MongoClient client = new MongoClient(tenant.ConnectionString);

            mongoClient = client;

            mongoDatabase = client.GetDatabase(tenant.Database);
        }

        protected Repository(IMongoDatabase database)
        {
            mongoDatabase = database;
        }

        protected virtual MongoCollectionSettings CollectionSettings()
        {
            return new MongoCollectionSettings();
        }

        protected virtual string CollectionName()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}Set", typeof(TEntity).Name);
        }

        protected virtual Task SetupCollectionAsync(IMongoCollection<TEntity> collection,
            CancellationToken ct)
        {
            return Task.CompletedTask;
        }

        public virtual async Task ClearAsync(
            CancellationToken ct = default)
        {
            try
            {
                await Database.DropCollectionAsync(CollectionName(), ct);
            }
            catch (MongoCommandException ex)
            {
                if (ex.Code != 26)
                {
                    throw;
                }
            }

            await InitializeAsync(ct);
        }

        public async Task InitializeAsync(
            CancellationToken ct)
        {
            try
            {
                CreateCollection();

                await SetupCollectionAsync(Collection, ct);
            }
            catch (Exception ex)
            {
                var databaseName = Database.DatabaseNamespace.DatabaseName;

                var error = new Exception($"MongoDb connection failed to connect to database {databaseName}.");

                throw error;
            }
        }

        private void CreateCollection()
        {
            mongoCollection = mongoDatabase.GetCollection<TEntity>(
                CollectionName(),
                CollectionSettings() ?? new MongoCollectionSettings());
        }


        public IQueryable<TEntity> GetAll()
        {
           return Collection.AsQueryable();
        }

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null)
        {
            FilterDefinition<TEntity> filter = Filter.Empty;

            if (predicate != null)
            {
                filter = Filter.And(predicate);
            }

            var filted = Collection.Find(filter);

            if (orderBy != null)
            {
                return orderBy(filted.ToEnumerable<TEntity>());
            }
            else
            {
                return filted.ToEnumerable<TEntity>();
            }
        }




        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual TEntity Insert(TEntity entity)
        {
           Collection.InsertOne(entity);
            return entity;
        }


        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Insert(params TEntity[] entities)
        {
            Collection.InsertMany(entities);
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            Collection.InsertMany(entities);
        }

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
           await Collection.InsertOneAsync(entity,null, cancellationToken);
            return entity;

        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        public virtual async Task InsertAsync(params TEntity[] entities)
        {
            await Collection.InsertManyAsync(entities, null, cancellationToken: default(CancellationToken));
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.InsertManyAsync(entities, null, cancellationToken: default(CancellationToken));
        }


        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TEntity entity, Expression<Func<TEntity, bool>> predict)
        {
            Collection.ReplaceOne(predict, entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predict, CancellationToken cancel)
        {
            await Collection.ReplaceOneAsync(predict, entity, cancellationToken: cancel);

        }


        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> expression)
        {
            Collection.DeleteOne(expression);
        }

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.DeleteOneAsync(expression, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null)
        {
            FilterDefinition<TEntity> filter = Filter.Empty;

            if (predicate != null)
            {
                filter = Filter.And(predicate);
            }

            var filted = Collection.Find(filter);

            if (orderBy != null)
            {
                return orderBy(filted.ToEnumerable<TEntity>()).FirstOrDefault();
            }
            else
            {
                return filted.FirstOrDefault();
            }
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The found entity or null.</returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            var filter = Filter.Empty;

            if (expression != null)
            {
                filter = Filter.And(expression);
            }

            return Collection.Find(filter).First();

        }

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var filter = Filter.Empty;

            if (expression != null)
            {
                filter = Filter.And(expression);
            }

            return (await Collection.FindAsync(filter)).First();
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null,
                                                int pageIndex = 0,
                                                int pageSize = 20)
        {

            FilterDefinition<TEntity> filter = Filter.Empty;

            if (predicate != null)
            {
                filter = Filter.And(predicate);
            }

            var filted = Collection.Find(filter);

            if (orderBy != null)
            {
                 var orderd = orderBy(filted.ToEnumerable<TEntity>());

                 return orderd.ToList().ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return filted.ToList().ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public async Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                    Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null,
                                                    int pageIndex = 0,
                                                    int pageSize = 20,
                                                    CancellationToken cancellationToken = default(CancellationToken))
        {
            FilterDefinition<TEntity> filter = Filter.Empty;

            if (predicate != null)
            {
                filter = Filter.And(predicate);
            }

            var filted = Collection.Find(filter);

            if (orderBy != null)
            {
                var orderd = orderBy(filted.ToEnumerable<TEntity>());
                return orderd.ToList().ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return filted.ToList().ToPagedList(pageIndex, pageSize);
            }
        }


    }
}
