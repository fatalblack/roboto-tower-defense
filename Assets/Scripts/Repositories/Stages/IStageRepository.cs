using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IStageRepository
{
	Task<IEnumerable<Stage>> GetAllAsync();

	Task<IEnumerable<Stage>> GetByWorldIdAsync(Guid worldId);
}