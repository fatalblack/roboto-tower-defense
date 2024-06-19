using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlayerTowerRepository
{
	Task<IEnumerable<PlayerTower>> GetAllAsync();

	Task<IEnumerable<PlayerTower>> GetAllByPlayerIdAsync(Guid playerId);

	Task<PlayerTower> GetByIdAsync(Guid id);

	Task UpdateAsync(PlayerTower playerTowerInput);

	Task AddAsync(PlayerTower playerTowerInput);

	Task DeleteAsync(Guid id);
}