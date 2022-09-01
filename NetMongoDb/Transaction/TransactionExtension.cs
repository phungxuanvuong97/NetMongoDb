using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetMongoDb.Transaction
{
    public static class TransactionExtension
    {
            public static ITransaction Begin(this ITransaction transaction,
                IClientSessionHandle dbTransaction, bool autoCommit = false)
            {
                if (!dbTransaction.IsInTransaction) dbTransaction.StartTransaction();

                transaction.DbTransaction = dbTransaction;
                transaction.AutoCommit = autoCommit;

                return transaction;
            }

        /// <summary>
        /// Start the CAP transaction
        /// </summary>
        /// <param name="client">The <see cref="IMongoClient" />.</param>
        /// <param name="autoCommit">Whether the transaction is automatically committed when the message is published</param>
        /// <returns>The <see cref="IClientSessionHandle" /> of MongoDB transaction session object.</returns>
        public static IClientSessionHandle StartTransaction(this IMongoClient client, IServiceProvider priver, bool autoCommit = false)
        {
            var clientSessionHandle = client.StartSession();

            var mongodbTrans = ActivatorUtilities.CreateInstance<MongodbTransaction>(priver);

            var capTrans = mongodbTrans.Begin(clientSessionHandle, autoCommit);

            return new ClientSessionHandle(capTrans);
        }


    }
}
