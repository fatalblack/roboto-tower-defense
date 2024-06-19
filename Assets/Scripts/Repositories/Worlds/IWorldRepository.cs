using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWorldRepository
{
	Task<IEnumerable<World>> GetAllAsync();

	Task<World> GetByIdAsync(Guid id);

	Task<World> GetByCodeAsync(WorldCodes code);
}