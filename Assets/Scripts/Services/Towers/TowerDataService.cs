using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TowerDataService : ITowerDataService
{
	private readonly ITowerRepository towerRepository;

	public TowerDataService(ITowerRepository towerRepository)
	{
		this.towerRepository = towerRepository;
	}

	public async Task<IEnumerable<Tower>> GetAllAsync()
	{
		// gets Towers list from context and return it
		return await towerRepository.GetAllAsync();
	}

	public async Task<Tower> GetByIdAsync(Guid id)
	{
		// gets Tower by id from context and return it
		return await towerRepository.GetByIdAsync(id);
	}

	public async Task<Tower> GetByCodeAsync(TowerCodes code)
	{
		// gets Tower by code from context and return it
		return await towerRepository.GetByCodeAsync(code);
	}
}