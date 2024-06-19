using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlayerTowerDataService
{
	Task<IEnumerable<PlayerTower>> GetAllByPlayerIdAsync(Guid playerId);

	Task<PlayerTower> GetByIdAsync(Guid id);

	Task<GenericResponse<PlayerTower>> BuyAsync(Guid playerId, Guid towerId);

	Task<GenericResponse<PlayerTower>> UpgradeAsync(Guid id);

	Task<GenericResponse> SellAsync(Guid id);

	Task<GenericResponse<PlayerTower>> SetPositionAsync(Guid id, int indexColumn, int indexRow);

	Task<GenericResponse<PlayerTower>> RemovePositionAsync(Guid id);
}