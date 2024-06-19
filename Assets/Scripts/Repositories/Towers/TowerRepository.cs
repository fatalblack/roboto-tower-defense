using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TowerRepository : ITowerRepository
{
	private readonly IDbContext dbContext;

	public TowerRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<Tower>> GetAllAsync()
	{
		// gets Towers list from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		IEnumerable<Tower> towers = dataContext.Towers;

		// assigns children
		await JoinChildren(towers);

		// return towers
		return towers;
	}

	public async Task<Tower> GetByIdAsync(Guid id)
	{
		// gets Tower by id from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		Tower tower = dataContext.Towers.FirstOrDefault(tower => tower.Id == id);

		// assigns children
		await JoinChildren(tower);

		// return tower
		return tower;
	}

	public async Task<Tower> GetByCodeAsync(TowerCodes code)
	{
		// gets Tower by code from context
		DataContext dataContext = await dbContext.GetDataContextAsync();
		Tower tower = dataContext.Towers.FirstOrDefault(tower => tower.Code == code);

		// assigns children
		await JoinChildren(tower);

		// return tower
		return tower;
	}

	private async Task JoinChildren(Tower tower)
	{
		if (tower != null)
		{

			DataContext dataContext = await dbContext.GetDataContextAsync();

			// gets TowerCharacteristic by id from context and assign it
			TowerCharacteristic towerCharacteristic = dataContext.TowerCharacteristics.FirstOrDefault(characteristic => characteristic.TowerId == tower.Id);
			tower.Characteristic = towerCharacteristic;
		}
	}

	private async Task JoinChildren(IEnumerable<Tower> towers)
	{
		IEnumerable<TowerCharacteristic> characteristics = new List<TowerCharacteristic>();

		if (towers.Any())
		{
			DataContext dataContext = await dbContext.GetDataContextAsync();
			characteristics = dataContext.TowerCharacteristics.Where(characteristic => towers.Any(tower => tower.Id == characteristic.TowerId));
		}
		foreach (Tower tower in towers)
		{
			// gets TowerCharacteristic by id from context and assign it
			TowerCharacteristic towerCharacteristic = characteristics.FirstOrDefault(characteristic => characteristic.TowerId == tower.Id);
			tower.Characteristic = towerCharacteristic;
		}
	}
}