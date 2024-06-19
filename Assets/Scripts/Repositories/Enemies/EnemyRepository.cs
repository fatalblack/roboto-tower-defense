using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EnemyRepository : IEnemyRepository
{
	private readonly IDbContext dbContext;

	public EnemyRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<Enemy>> GetAllAsync()
	{
		// gets Enemies list from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		IEnumerable<Enemy> enemies = dataContext.Enemies;
		
		// assigns children
		await JoinChildren(enemies);

		// return enemies
		return enemies;
	}

	public async Task<Enemy> GetByIdAsync(Guid id)
	{
		// gets Enemy by id from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		Enemy enemy = dataContext.Enemies.FirstOrDefault(enemy => enemy.Id == id);

		// assigns children
		await JoinChildren(enemy);

		// return enemy
		return enemy;
	}

	public async Task<Enemy> GetByCodeAsync(EnemyCodes code)
	{
		// gets Enemy by code from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		Enemy enemy = dataContext.Enemies.FirstOrDefault(enemy => enemy.Code == code);

		// assigns children
		await JoinChildren(enemy);

		// return enemy
		return enemy;
	}

	private async Task JoinChildren(Enemy enemy)
	{
		if (enemy != null)
		{

			DataContext dataContext = await dbContext.GetDataContextAsync();

			// gets EnemyCharacteristic by id from context and assign it
			EnemyCharacteristic enemyCharacteristic = dataContext.EnemyCharacteristics.FirstOrDefault(characteristic => characteristic.EnemyId == enemy.Id);
			enemy.Characteristic = enemyCharacteristic;
		}
	}

	private async Task JoinChildren(IEnumerable<Enemy> enemies)
	{
		IEnumerable<EnemyCharacteristic> characteristics = new List<EnemyCharacteristic>();

		if (enemies.Any())
		{
			DataContext dataContext = await dbContext.GetDataContextAsync();
			characteristics = dataContext.EnemyCharacteristics.Where(characteristic => enemies.Any(enemy => enemy.Id == characteristic.EnemyId));
		}
		foreach (Enemy enemy in enemies)
		{
			// gets EnemyCharacteristic by id from context and assign it
			EnemyCharacteristic enemyCharacteristic = characteristics.FirstOrDefault(characteristic => characteristic.EnemyId == enemy.Id);
			enemy.Characteristic = enemyCharacteristic;
		}
	}
}