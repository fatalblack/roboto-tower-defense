using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlayerTowerDataService : IPlayerTowerDataService
{
	private readonly IPlayerTowerRepository playerTowerRepository;
	private readonly ITowerCharacteristicRepository towerCharacteristicRepository;
	private readonly IPlayerRepository playerRepository;

	public PlayerTowerDataService(
		IPlayerTowerRepository playerTowerRepository,
		ITowerCharacteristicRepository towerCharacteristicRepository,
		IPlayerRepository playerRepository)
	{
		this.playerTowerRepository = playerTowerRepository;
		this.towerCharacteristicRepository = towerCharacteristicRepository;
		this.playerRepository = playerRepository;
	}

	public async Task<IEnumerable<PlayerTower>> GetAllByPlayerIdAsync(Guid playerId)
	{
		// gets PlayerTowers list by player id from context and return it
		return await playerTowerRepository.GetAllByPlayerIdAsync(playerId);
	}

	public async Task<PlayerTower> GetByIdAsync(Guid id)
	{
		// gets PlayerTower by id from context and return it
		return await playerTowerRepository.GetByIdAsync(id);
	}

	public async Task<GenericResponse<PlayerTower>> BuyAsync(Guid playerId, Guid towerId)
	{
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(playerId);
		// gets TowerCharacteristics by id from context
		TowerCharacteristic towerCharacteristic = await towerCharacteristicRepository.GetByIdAsync(towerId);
		// sets the buy cost
		int buyCost = towerCharacteristic.BuyCost;

		// if the buy cost is greater than the player's gold available must return error
		if (buyCost > player.Gold)
		{
			return GenericResponse<PlayerTower>.SetFail("No tienes suficiente dinero.");
		}

		// creates PlayerTower entity
		PlayerTower playerTower = new PlayerTower
		{
			Id = Guid.NewGuid(),
			PlayerId = player.Id,
			WorldId = player.CurrentWorldId,
			StageNumber = player.CurrentStageNumber,
			TowerId = towerId,
			TowerLevel = 1,
			IndexColumn = null,
			IndexRow = null
		};

		// adds PlayerTower to context
		await playerTowerRepository.AddAsync(playerTower);

		// discounts from player's gold the upgrade cost
		player.Gold -= buyCost;
		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return added PlayerTower
		return GenericResponse<PlayerTower>.SetOk(playerTower);
	}

	public async Task<GenericResponse<PlayerTower>> UpgradeAsync(Guid id)
	{
		// gets PlayerTower by id from context
		PlayerTower playerTower = await playerTowerRepository.GetByIdAsync(id);
		// if PlayerTower does not exists must return error
		if (playerTower == null)
		{
			return GenericResponse<PlayerTower>.SetFail("La torre no existe.");
		}

		// gets TowerCharacteristics by id from context
		TowerCharacteristic towerCharacteristic = await towerCharacteristicRepository.GetByIdAsync(playerTower.TowerId);
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(playerTower.PlayerId);
		// sets the level to upgrade (next level)
		int toLevel = playerTower.TowerLevel + 1;

		// if wanted level es out from the max level range must return error
		if (toLevel > towerCharacteristic.MaxLevel)
		{
			return GenericResponse<PlayerTower>.SetFail("La torre ya tiene el nivel máximo posible.");
		}

		// calculates upgrade cost
		int upgradeCost = PlayerTowerCalculations.CalculateUpgradeCost(
			toLevel,
			towerCharacteristic.UpgradeCost,
			towerCharacteristic.UpgradeCostMultiplierByLevel);

		// if the upgrade cost is greater than the player's gold available must return error
		if (upgradeCost > player.Gold)
		{
			return GenericResponse<PlayerTower>.SetFail("No tienes suficiente dinero.");
		}

		// sets the new level to the tower
		playerTower.TowerLevel = toLevel;
		// updates PlayerTower from context
		await playerTowerRepository.UpdateAsync(playerTower);

		// discounts from player's gold the upgrade cost
		player.Gold -= upgradeCost;
		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return updated PlayerTower
		return GenericResponse<PlayerTower>.SetOk(playerTower);
	}

	public async Task<GenericResponse> SellAsync(Guid id)
	{
		// gets PlayerTower by id from context
		PlayerTower playerTower = await playerTowerRepository.GetByIdAsync(id);
		// if PlayerTower does not exists must return error
		if (playerTower == null)
		{
			return GenericResponse.SetFail("La torre no existe.");
		}

		// gets TowerCharacteristics by id from context
		TowerCharacteristic towerCharacteristic = await towerCharacteristicRepository.GetByIdAsync(playerTower.TowerId);
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(playerTower.PlayerId);

		// calculates selling price
		int sellingPrice = PlayerTowerCalculations.CalculateSellingPrice(
			playerTower.TowerLevel,
			towerCharacteristic.BuyCost,
			towerCharacteristic.UpgradeCost,
			towerCharacteristic.UpgradeCostMultiplierByLevel);

		// deletes PlayerTower from context
		await playerTowerRepository.DeleteAsync(id);

		// returns to player's gold the price of the selling
		player.Gold += sellingPrice;
		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return succeeded response
		return GenericResponse.SetOk();
	}

	public async Task<GenericResponse<PlayerTower>> SetPositionAsync(Guid id, int indexColumn, int indexRow)
	{
		// gets PlayerTower by id from context
		PlayerTower playerTower = await playerTowerRepository.GetByIdAsync(id);
		// if PlayerTower does not exists must return error
		if (playerTower == null)
		{
			return GenericResponse<PlayerTower>.SetFail("La torre no existe.");
		}

		// gets PlayerTowers list by player id from context and return it, except the chosen one who will be positioned
		IEnumerable<PlayerTower> playerTowers = await playerTowerRepository.GetAllByPlayerIdAsync(playerTower.PlayerId);

		// if the position is already in use must return error
		if (playerTowers.Any(playerTowerItem =>
			playerTowerItem.Id != id &&
			playerTowerItem.IndexColumn == indexColumn &&
			playerTowerItem.IndexRow == indexRow))
		{
			return GenericResponse<PlayerTower>.SetFail("La posición indicada ya está siendo utilizada por otra torre.");
		}

		// sets the new position
		playerTower.IndexColumn = indexColumn;
		playerTower.IndexRow = indexRow;
		// updates PlayerTower from context
		await playerTowerRepository.UpdateAsync(playerTower);

		// return updated PlayerTower
		return GenericResponse<PlayerTower>.SetOk(playerTower);
	}

	public async Task<GenericResponse<PlayerTower>> RemovePositionAsync(Guid id)
	{
		// gets PlayerTower by id from context
		PlayerTower playerTower = await playerTowerRepository.GetByIdAsync(id);
		// if PlayerTower does not exists must return error
		if (playerTower == null)
		{
			return GenericResponse<PlayerTower>.SetFail("La torre no existe.");
		}

		// sets the new position
		playerTower.IndexColumn = null;
		playerTower.IndexRow = null;
		// updates PlayerTower from context
		await playerTowerRepository.UpdateAsync(playerTower);

		// return updated PlayerTower
		return GenericResponse<PlayerTower>.SetOk(playerTower);
	}
}