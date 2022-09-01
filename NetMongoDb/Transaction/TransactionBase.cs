using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetMongoDb.Transaction
{
    public abstract class TransactionBase: ITransaction
    {
        public bool AutoCommit { get; set; }

        public virtual object? DbTransaction { get; set; }
        public abstract void Commit();

        public abstract Task CommitAsync(CancellationToken cancellationToken = default);

        public abstract void Rollback();

        public abstract Task RollbackAsync(CancellationToken cancellationToken = default);

        public abstract void Dispose();
    }
}
