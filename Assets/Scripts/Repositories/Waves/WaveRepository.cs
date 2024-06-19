using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WaveRepository : IWaveRepository
{
	private readonly IDbContext dbContext;

	public WaveRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<Wave>> GetByWorldCodeAndStageNumberAsync(WorldCodes code, int stageNumber)
	{
		// initalizes datacontext
		DataContext dataContext = await dbContext.GetDataContextAsync();

		// gets World
		World world = dataContext.Worlds.FirstOrDefault(world => world.Code == code);

		// if world does not exist return null
		if (world == null)
		{
			return null;
		}

		// gets Wave by World code and Stage Number from context
		IEnumerable<Wave> waves = dataContext.Waves.Where(wave => wave.WorldId == world.Id && wave.StageNumber == stageNumber);

		// assigns children
		await JoinChildren(waves);

		// return waves
		return waves;
	}

	private async Task JoinChildren(IEnumerable<Wave> waves)
	{
		if (waves.Any())
		{
			DataContext dataContext = await dbContext.GetDataContextAsync();
			
			// gets enemy for each wave
			foreach (Wave wave in waves)
			{
				// gets enemy by enemyid from context and assign it
				wave.Enemy = dataContext.Enemies.FirstOrDefault(enemy => enemy.Id == wave.EnemyId);
			}
		}
	}
}