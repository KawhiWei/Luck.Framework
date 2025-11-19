using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Luck.EntityFrameworkCore;

public class LuckDbConnectionInterceptor:DbConnectionInterceptor
{
    public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        connection.ConnectionString = "";
        
        return base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }
}