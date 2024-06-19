using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEnemyDataService
{
	Task<IEnumerable<Enemy>> GetAllAsync();

	Task<Enemy> GetByIdAsync(Guid id);

	Task<Enemy> GetByCodeAsync(EnemyCodes code);
}