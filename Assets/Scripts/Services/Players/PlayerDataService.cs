using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlayerDataService : IPlayerDataService
{
	private readonly IPlayerRepository playerRepository;
	private readonly IWorldRepository worldRepository;
	private readonly IStageRepository stageRepository;

	public PlayerDataService(
		IPlayerRepository playerRepository,
		IWorldRepository worldRepository,
		IStageRepository stageRepository)
	{
		this.playerRepository = playerRepository;
		this.worldRepository = worldRepository;
		this.stageRepository = stageRepository;
	}

	public async Task<IEnumerable<Player>> GetAllAsync()
	{
		// gets Players list from context and return it
		return await playerRepository.GetAllAsync();
	}

	public async Task<Player> GetByIdAsync(Guid id)
	{
		// gets Player by id from context and return it
		return await playerRepository.GetByIdAsync(id);
	}

	public async Task UpdateAsync(Player playerInput)
	{
		// updates Player from context
		await playerRepository.UpdateAsync(playerInput);
	}

	public async Task<Player> AddAsync(string name)
	{
		// gets the first World by default
		World world = await worldRepository.GetByCodeAsync(WorldCodes.MUNDO1);

		// creates Player entity
		Player player = new Player
		{
			Id = Guid.NewGuid(),
			Name = name,
			Health = GameDefaults.playerMaxHealth,
			Gold = GameDefaults.playerInitialGold,
			CurrentWorldId = world.Id,
			CurrentStageNumber = 1
		};

		// adds Player to context
		await playerRepository.AddAsync(player);

		// return added Player
		return player;
	}

	public async Task<GenericResponse<Player>> MoveToNextStage(Guid id)
	{
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(id);
		// gets player current world id
		Guid currentWorldId = player.CurrentWorldId;
		// gets player current stage number
		int currentStageNumber = player.CurrentStageNumber;

		// if PlayerTower does not exists must return error
		if (player == null)
		{
			return GenericResponse<Player>.SetFail("El jugador no existe.");
		}

		// gets Worlds list from context
		IEnumerable<World> worlds = await worldRepository.GetAllAsync();
		// gets Stages list from context
		IEnumerable<Stage> stages = await stageRepository.GetAllAsync();
		// gets current world
		World currentWorld = worlds.FirstOrDefault(world => world.Id == currentWorldId);
		// sets next world id
		Guid? nextWorldId = worlds.FirstOrDefault(world => world.Order > currentWorld.Order)?.Id;

		// gets the next stage for the current world and set this one if exists
		// if was in the last stage must find the first stage of the next world
		Stage nextStage =
			stages.FirstOrDefault(stage => stage.WorldId == currentWorldId && stage.StageNumber > currentStageNumber) ??
			stages.FirstOrDefault(stage => stage.WorldId == nextWorldId);

		// if can't find a stage it means the player won whole game
		if (nextStage == null)
		{
			return GenericResponse<Player>.SetOk(player, "¡Completaste todos los niveles, felicidades!");
		}

		// if a stage was found
		// sets player's current world and stage number
		player.CurrentWorldId = nextStage.WorldId;
		player.CurrentStageNumber = nextStage.StageNumber;

		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return succeeded status
		return GenericResponse<Player>.SetOk(player);
	}

	public async Task<GenericResponse<Player>> SetHealth(Guid id, int health)
	{
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(id);

		// if PlayerTower does not exists must return error
		if (player == null)
		{
			return GenericResponse<Player>.SetFail("El jugador no existe.");
		}

		// sets player's health
		player.Health = health;

		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return succeeded status
		return GenericResponse<Player>.SetOk(player);
	}

	public async Task<GenericResponse<Player>> Damage(Guid id, int damageTook = 1)
	{
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(id);

		// if PlayerTower does not exists must return error
		if (player == null)
		{
			return GenericResponse<Player>.SetFail("El jugador no existe.");
		}

		// sets player's health
		player.Health -= damageTook;

		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return succeeded status
		return GenericResponse<Player>.SetOk(player);
	}

	public async Task<GenericResponse<Player>> AddReward(Guid id, int gold)
	{
		// gets Player by id from context
		Player player = await playerRepository.GetByIdAsync(id);

		// if PlayerTower does not exists must return error
		if (player == null)
		{
			return GenericResponse<Player>.SetFail("El jugador no existe.");
		}

		// adds reward
		player.Gold += gold;

		// updates Player from context
		await playerRepository.UpdateAsync(player);

		// return succeeded status
		return GenericResponse<Player>.SetOk(player);
	}
}