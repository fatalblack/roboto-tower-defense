using System;
using System.Linq;
using System.Threading.Tasks;

public class TowerCharacteristicRepository : ITowerCharacteristicRepository
{
	private readonly IDbContext dbContext;

	public TowerCharacteristicRepository(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<TowerCharacteristic> GetByIdAsync(Guid id)
	{
		// gets TowerCharacteristic by id from context and return it
		DataContext dataContext = await dbContext.GetDataContextAsync();
		return dataContext.TowerCharacteristics.FirstOrDefault(towerCharacteristic => towerCharacteristic.TowerId == id);
	}
}