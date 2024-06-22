using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerTowerRepository : IPlayerTowerRepository
{
	private readonly IDbContext dbContext;

	public PlayerTowerRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<PlayerTower>> GetAllAsync()
	{
		// gets PlayerTowers list from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.PlayerTowers;
	}

	public async Task<IEnumerable<PlayerTower>> GetAllByPlayerIdAsync(Guid playerId)
	{
		// gets PlayerTowers list by player id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.PlayerTowers.Where(playerTower => playerTower.PlayerId == playerId);
	}

	public async Task<PlayerTower> GetByIdAsync(Guid id)
	{
		// gets PlayerTower by id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.PlayerTowers.FirstOrDefault(playerTower => playerTower.Id == id);
	}

	public async Task UpdateAsync(PlayerTower playerTowerInput)
	{
		// gets PlayerTower from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		PlayerTower playerTower = dataContext.PlayerTowers.FirstOrDefault(playerTower => playerTower.Id == playerTower.Id);

		// if PlayerTower exists update it
		if (playerTower != null)
		{
			playerTower = playerTowerInput;
		}
	}

	public async Task AddAsync(PlayerTower playerTowerInput)
	{
		// adds Player to context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		dataContext.PlayerTowers.Add(playerTowerInput);
	}

	public async Task DeleteAsync(Guid id)
	{
		// gets PlayerTower from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		PlayerTower playerTower = dataContext.PlayerTowers.FirstOrDefault(playerTower => playerTower.Id == id);

		// if PlayerTower exists delete it
		if (playerTower != null)
		{
			dataContext.PlayerTowers.Remove(playerTower);
		}
	}
}