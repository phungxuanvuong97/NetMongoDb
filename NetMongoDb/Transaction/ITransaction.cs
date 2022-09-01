﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetMongoDb.Transaction
{
    public interface ITransaction: IDisposable
    {

        /// <summary>
        /// A flag is used to indicate whether the transaction is automatically committed after the message is published
        /// </summary>
        bool AutoCommit { get; set; }

        /// <summary>
        /// Database transaction object, can be converted to a specific database transaction object or IDBTransaction when used
        /// </summary>
        object? DbTransaction { get; set; }

        /// <summary>
        /// Submit the transaction context of the CAP, we will send the message to the message queue at the time of submission
        /// </summary>
        void Commit();

        /// <summary>
        /// Submit the transaction context of the CAP, we will send the message to the message queue at the time of submission
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// We will delete the message data that has not been store in the buffer data of current transaction context.
        /// </summary>
        void Rollback();

        /// <summary>
        /// We will delete the message data that has not been store in the buffer data of current transaction context.
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
