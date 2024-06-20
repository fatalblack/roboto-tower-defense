using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StageRepository : IStageRepository
{
	private readonly IDbContext dbContext;

	public StageRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<Stage>> GetAllAsync()
	{
		// gets Stages list from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();

		IEnumerable<Stage> stages = dataContext.Stages
			.OrderBy(stage => stage.WorldId)
			.ThenBy(stage => stage.StageNumber);

		// return stages
		return stages;
	}

	public async Task<IEnumerable<Stage>> GetByWorldIdAsync(Guid worldId)
	{
		// gets Stages list by world id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		
		IEnumerable<Stage> stages = dataContext.Stages
			.Where(stage => stage.WorldId == worldId)
			.OrderBy(stage => stage.WorldId)
			.ThenBy(stage => stage.StageNumber);

		// return stages
		return stages;
	}
}