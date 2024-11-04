using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AnagraficaAPI.Middleware
{
    public static class ZeusApiResillientStrategy
    {
        public static void AddResillientStrategy(this SqlServerDbContextOptionsBuilder optionsBuilder, bool? EnabledSqlEnableRetryOnFailure)
        {
            if (EnabledSqlEnableRetryOnFailure.HasValue && EnabledSqlEnableRetryOnFailure.Value)
                optionsBuilder.EnableRetryOnFailure(
                                            maxRetryCount: 3,
                                            maxRetryDelay: TimeSpan.FromSeconds(0.5),
                                            errorNumbersToAdd: new List<int>() // Add additional Sql Error Code
                                            );

            //Default Errors for retries strategy

            // SQL Error Code: 49920
            // Cannot process request. Too many operations in progress for subscription "%ld".
            // The service is busy processing multiple requests for this subscription.
            // Requests are currently blocked for resource optimization. Query sys.dm_operation_status for operation status.
            // Wait until pending requests are complete or delete one of your pending requests and retry your request later.

            // SQL Error Code: 49919
            // Cannot process create or update request. Too many create or update operations in progress for subscription "%ld".
            // The service is busy processing multiple create or update requests for your subscription or server.
            // Requests are currently blocked for resource optimization. Query sys.dm_operation_status for pending operations.
            // Wait till pending create or update requests are complete or delete one of your pending requests and
            // retry your request later.

            // SQL Error Code: 49918
            // Cannot process request. Not enough resources to process request.
            // The service is currently busy.Please retry the request later.

            // SQL Error Code: 41839
            // Transaction exceeded the maximum number of commit dependencies.

            // SQL Error Code: 41325
            // The current transaction failed to commit due to a serializable validation failure.

            // SQL Error Code: 41305
            // The current transaction failed to commit due to a repeatable read validation failure.

            // SQL Error Code: 41302
            // The current transaction attempted to update a record that has been updated since the transaction started.

            // SQL Error Code: 41301
            // Dependency failure: a dependency was taken on another transaction that later failed to commit.

            // SQL Error Code: 40613
            // Database XXXX on server YYYY is not currently available. Please retry the connection later.
            // If the problem persists, contact customer support, and provide them the session tracing ID of ZZZZZ.

            // SQL Error Code: 40501
            // The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be decoded).

            // SQL Error Code: 40197
            // The service has encountered an error processing your request. Please try again.

            // SQL Error Code: 10936
            // Resource ID : %d. The request limit for the elastic pool is %d and has been reached.
            // See 'http://go.microsoft.com/fwlink/?LinkId=267637' for assistance.

            // SQL Error Code: 10929
            // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d and the current usage for the database is %d.
            // However, the server is currently too busy to support requests greater than %d for this database.
            // For more information, see http://go.microsoft.com/fwlink/?LinkId=267637. Otherwise, please try again.

            // SQL Error Code: 10928
            // Resource ID: %d. The %s limit for the database is %d and has been reached. For more information,
            // see http://go.microsoft.com/fwlink/?LinkId=267637.

            // SQL Error Code: 10060
            // A network-related or instance-specific error occurred while establishing a connection to SQL Server.
            // The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server
            // is configured to allow remote connections. (provider: TCP Provider, error: 0 - A connection attempt failed
            // because the connected party did not properly respond after a period of time, or established connection failed
            // because connected host has failed to respond.)"}

            // SQL Error Code: 10054
            // A transport-level error has occurred when sending the request to the server.
            // (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)

            // SQL Error Code: 10053
            // A transport-level error has occurred when receiving results from the server.
            // An established connection was aborted by the software in your host machine.

            // SQL Error Code: 1205
            // Deadlock

            // SQL Error Code: 233
            // The client was unable to establish a connection because of an error during connection initialization process before login.
            // Possible causes include the following: the client tried to connect to an unsupported version of SQL Server;
            // the server was too busy to accept new connections; or there was a resource limitation (insufficient memory or maximum
            // allowed connections) on the server. (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by
            // the remote host.)

            // SQL Error Code: 121
            // The semaphore timeout period has expired

            // SQL Error Code: 64
            // A connection was successfully established with the server, but then an error occurred during the login process.
            // (provider: TCP Provider, error: 0 - The specified network name is no longer available.)

            // DBNETLIB Error Code: 20
            // The instance of SQL Server you attempted to connect to does not support encryption.        }
        }
    }
}

