using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlayerDataService
{
	Task<IEnumerable<Player>> GetAllAsync();

	Task<Player> GetByIdAsync(Guid id);

	Task UpdateAsync(Player playerInput);

	Task<Player> AddAsync(string name);

	Task<GenericResponse<Player>> MoveToNextStage(Guid id);

	Task<GenericResponse<Player>> SetHealth(Guid id, int health);

	Task<GenericResponse<Player>> Damage(Guid id, int damageTook = 1);

	Task<GenericResponse<Player>> AddReward(Guid id, int gold);
}