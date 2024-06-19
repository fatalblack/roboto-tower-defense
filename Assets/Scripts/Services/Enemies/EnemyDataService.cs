using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnemyDataService : IEnemyDataService
{
	private readonly IEnemyRepository enemyRepository;

	public EnemyDataService(IEnemyRepository enemyRepository)
	{
		this.enemyRepository = enemyRepository;
	}

	public async Task<IEnumerable<Enemy>> GetAllAsync()
	{
		// gets Enemies list from context and return it
		return await enemyRepository.GetAllAsync();
	}

	public async Task<Enemy> GetByIdAsync(Guid id)
	{
		// gets Enemy by id from context and return it
		return await enemyRepository.GetByIdAsync(id);
	}

	public async Task<Enemy> GetByCodeAsync(EnemyCodes code)
	{
		// gets Enemy by code from context and return it
		return await enemyRepository.GetByCodeAsync(code);
	}
}