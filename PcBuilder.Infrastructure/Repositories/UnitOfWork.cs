using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Data;

namespace PcBuilder.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public async Task<int> GuardarCambiosAsync(CancellationToken ct = default) =>
        await db.SaveChangesAsync(ct);
}
