using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITowerDataService
{
	Task<IEnumerable<Tower>> GetAllAsync();

	Task<Tower> GetByIdAsync(Guid id);

	Task<Tower> GetByCodeAsync(TowerCodes code);
}