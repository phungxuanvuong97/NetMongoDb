using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetMongoDb.Transaction
{
    public class MongodbTransaction: TransactionBase
    {
        public override void Commit()
        {
            Debug.Assert(DbTransaction != null);

            if (DbTransaction is IClientSessionHandle session) session.CommitTransaction();
        }

        public override async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Assert(DbTransaction != null);

            if (DbTransaction is IClientSessionHandle session) await session.CommitTransactionAsync(cancellationToken);

        }


        public override void Rollback()
        {
            Debug.Assert(DbTransaction != null);

            if (DbTransaction is IClientSessionHandle session) session.AbortTransaction();
        }

        public override async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            Debug.Assert(DbTransaction != null);

            if (DbTransaction is IClientSessionHandle session) await session.AbortTransactionAsync(cancellationToken);
        }

        public override void Dispose()
        {
            (DbTransaction as IClientSessionHandle)?.Dispose();
            DbTransaction = null;
        }


    }


}
