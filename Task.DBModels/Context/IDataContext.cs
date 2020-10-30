using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Task.DBModels.Entities
{
    public interface IDataContext
    {
        DbSet<Task> Tasks { get; set; }

        DbSet<ErrorLog> ErrorLogs { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}