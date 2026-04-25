using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class FunctionalityRepository : _Repository<Functionality, FunctionalityId>, IFunctionalityRepository
{
    public FunctionalityRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public async Task<Functionality?> GetByAbbreviationAsync(string abbreviation)
    {
        var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.Abbreviation == abbreviation);
        return module;
    }

    public async Task<Functionality?> GetByFriendlyIdAsync(string friendlyId)
    {
        var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.FriendlyId == friendlyId);
        return module;
    }

    public async override Task<IPagedResult<Functionality>> GetAllAsync(int skip, int take)
    {
        var count = await Entity.AsNoTracking().CountAsync();

        var records = await Entity.AsNoTracking().OrderBy(ord => ord.CreatedAt)
                    .Skip(skip).Take(take)
                    .Include(m => m.Module).ThenInclude(m => m.Application)
                    .AsSplitQuery() // Use AsSplitQuery to avoid Cartesian product issues with multiple includes
                    .ToListAsync();

        var finalResult = new PagedResult<Functionality>
        {
            TotalOfRecords = count,
            Records = records
        };

        return finalResult;
    }

    public async Task<Functionality> GetByIdWithDetailAsync(FunctionalityId id)
    {
        var functionality = await Entity.AsNoTrackingWithIdentityResolution()
            .Include(m => m.Module).ThenInclude(m => m!.Application)
            .AsSplitQuery() // Use AsSplitQuery to avoid Cartesian product issues with multiple includes
            .FirstOrDefaultAsync(f => f.Id == id);
            
        return functionality!;
    }

}
