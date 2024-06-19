using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlayerRepository : IPlayerRepository
{
	private readonly IDbContext dbContext;

	public PlayerRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<Player>> GetAllAsync()
	{
		// gets Players list from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.Players;
	}

	public async Task<Player> GetByIdAsync(Guid id)
	{
		// gets Player by id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.Players.FirstOrDefault(player => player.Id == id);
	}

	public async Task UpdateAsync(Player playerInput)
	{
		// gets Player from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		Player player = dataContext.Players.FirstOrDefault(player => player.Id == player.Id);

		// if Player exists update it
		if (player != null)
		{
			player = playerInput;
		}
	}

	public async Task AddAsync(Player playerInput)
	{
		// adds Player to context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		dataContext.Players.Add(playerInput);
	}
}