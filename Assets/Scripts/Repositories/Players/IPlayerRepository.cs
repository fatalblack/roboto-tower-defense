using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlayerRepository
{
	Task<IEnumerable<Player>> GetAllAsync();

	Task<Player> GetByIdAsync(Guid id);

	Task UpdateAsync(Player playerInput);

	Task AddAsync(Player playerInput);
}