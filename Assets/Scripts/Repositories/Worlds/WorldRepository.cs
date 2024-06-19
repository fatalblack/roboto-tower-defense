using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WorldRepository : IWorldRepository
{
	private readonly IDbContext dbContext;

	public WorldRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<World>> GetAllAsync()
	{
		// gets Worlds list from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.Worlds.OrderBy(world => world.Order);
	}

	public async Task<World> GetByIdAsync(Guid id)
	{
		// gets World by id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.Worlds.FirstOrDefault(tower => tower.Id == id);
	}

	public async Task<World> GetByCodeAsync(WorldCodes code)
	{
		// gets World by code from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.Worlds.FirstOrDefault(world => world.Code == code);
	}
}