using Application;


namespace Infrastructure.Context;

public class UnitOfWork(AppDbContext context):IUnitOfWork
{
    private readonly AppDbContext context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
