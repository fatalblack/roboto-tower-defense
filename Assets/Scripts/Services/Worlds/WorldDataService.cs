using System;
using System.Threading.Tasks;

public class WorldDataService : IWorldDataService
{
	private readonly IWorldRepository worldRepository;

	public WorldDataService(IWorldRepository worldRepository)
	{
		this.worldRepository = worldRepository;
	}

	public async Task<World> GetByIdAsync(Guid id)
	{
		// gets Tower by id from context and return it
		return await worldRepository.GetByIdAsync(id);
	}
}