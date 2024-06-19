using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DbContext : IDbContext
{
    private DataContext dataContext;

	public DbContext()
	{
		// if the dataContext is null must be initialized
		if (EqualityComparer<DataContext>.Default.Equals(dataContext, default(DataContext)))
		{
			// initialize dataContext
			dataContext = new DataContext();

			// set to empty each entity list
			dataContext.Enemies = new List<Enemy>();
			dataContext.EnemyCharacteristics = new List<EnemyCharacteristic>();
			dataContext.Towers = new List<Tower>();
			dataContext.TowerCharacteristics = new List<TowerCharacteristic>();
			dataContext.Worlds = new List<World>();
			dataContext.Stages = new List<Stage>();
			dataContext.Waves = new List<Wave>();
			dataContext.Players = new List<Player>();
			dataContext.PlayerTowers = new List<PlayerTower>();

			// add mocked data
			FeedEnemies();
			FeedEnemyCharacteristics();
			FeedWorlds();
			FeedStages();
			FeedWaves();
			FeedTowers();
			FeedTowerCharacteristics();
		}
	}

	public async Task<DataContext> GetDataContextAsync()
	{
		// returns current dataContext
		return await Task.FromResult<DataContext>(dataContext);
	}

	private void FeedEnemies()
	{
		// mock data for Enemies
		List<Enemy> enemies = new List<Enemy>
		{
			new Enemy
			{
				Id = Guid.NewGuid(),
				Code = EnemyCodes.ROBOTSPY,
				Name = "Robot espía",
				Description = "Debido a su pequeño tamaño y ser tan escurridizo la función que cumple es la de espía."
			},
			new Enemy
			{
				Id = Guid.NewGuid(),
				Code = EnemyCodes.ROBOTSOLDIER,
				Name = "Robot soldado",
				Description = "Un robot algo lento pero resistente para la batalla."
			},
			new Enemy
			{
				Id = Guid.NewGuid(),
				Code = EnemyCodes.ROBOTEXPLORER,
				Name = "Robot explorador",
				Description = "Rápido para moverse pero así también carece de resistencia."
			},
			new Enemy
			{
				Id = Guid.NewGuid(),
				Code = EnemyCodes.ROBOTTANK,
				Name = "Robot tanque",
				Description = "Lento y súper resistente, soporta golpes como ninguno."
			}
		};

		// add mocked data
		dataContext.Enemies.AddRange(enemies);
	}

	private void FeedEnemyCharacteristics()
	{
		// find ids to fill EnemyCharacteristics data
		Guid robotSpyEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTSPY).Id;
		Guid robotSoldierEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTSOLDIER).Id;
		Guid robotExplorerEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTEXPLORER).Id;
		Guid robotTankEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTTANK).Id;

		// mock data for EnemyCharacteristics
		List<EnemyCharacteristic> enemyCharacteristics = new List<EnemyCharacteristic>
		{
			new EnemyCharacteristic
			{
				EnemyId = robotSpyEnemyId,
				Health = 20,
				HealthMultiplierByLevel = 1.5f,
				MovementSpeed = 1.2f,
				MovementSpeedMultiplierByLevel = 1.1f,
				Gold = 20,
				GoldMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new EnemyCharacteristic
			{
				EnemyId = robotSoldierEnemyId,
				Health = 30,
				HealthMultiplierByLevel = 1.5f,
				MovementSpeed = 1f,
				MovementSpeedMultiplierByLevel = 1.1f,
				Gold = 30,
				GoldMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new EnemyCharacteristic
			{
				EnemyId = robotExplorerEnemyId,
				Health = 20,
				HealthMultiplierByLevel = 1.5f,
				MovementSpeed = 1.5f,
				MovementSpeedMultiplierByLevel = 1.1f,
				Gold = 30,
				GoldMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new EnemyCharacteristic
			{
				EnemyId = robotTankEnemyId,
				Health = 60,
				HealthMultiplierByLevel = 1.5f,
				MovementSpeed = 0.6f,
				MovementSpeedMultiplierByLevel = 1.1f,
				Gold = 100,
				GoldMultiplierByLevel = 2,
				MaxLevel = 4
			}
		};

		// add mocked data
		dataContext.EnemyCharacteristics.AddRange(enemyCharacteristics);
	}

	private void FeedWorlds()
	{
		// mock data for Worlds
		List<World> worlds = new List<World>
		{
			new World
			{
				Id = Guid.NewGuid(),
				Code = WorldCodes.MUNDO1,
				Name = "Planicies",
				Description = "Un mundo algo hostil, pero nada con lo que no puedas lidiar.",
				Order = 1,
				WidthBlocks = 8,
				HeightBlocks = 8,
				TotalTowersAllowed = 6
			},
			new World
			{
				Id = Guid.NewGuid(),
				Code = WorldCodes.MUNDO2,
				Name = "Montañas",
				Description = "¡Ten cuidado! Se comenta que habitan muchos robotos rápidos por aquí.",
				Order = 2,
				WidthBlocks = 10,
				HeightBlocks = 10,
				TotalTowersAllowed = 8
			}
		};

		// add mocked data
		dataContext.Worlds.AddRange(worlds);
	}

	private void FeedStages()
	{
		// find ids to fill Stages data
		Guid worldId1 = dataContext.Worlds.First(world => world.Code == WorldCodes.MUNDO1).Id;
		Guid worldId2 = dataContext.Worlds.First(world => world.Code == WorldCodes.MUNDO2).Id;

		// mock data for Stages
		List<Stage> stages = new List<Stage>
		{
			new Stage { WorldId = worldId1, StageNumber = 1 },
			new Stage { WorldId = worldId1, StageNumber = 2 },
			new Stage { WorldId = worldId1, StageNumber = 3 },
			new Stage { WorldId = worldId1, StageNumber = 4 },
			new Stage { WorldId = worldId1, StageNumber = 5 },
			new Stage { WorldId = worldId2, StageNumber = 1 },
			new Stage { WorldId = worldId2, StageNumber = 2 },
			new Stage { WorldId = worldId2, StageNumber = 3 },
			new Stage { WorldId = worldId2, StageNumber = 4 },
			new Stage { WorldId = worldId2, StageNumber = 5 }
		};

		// add mocked data
		dataContext.Stages.AddRange(stages);
	}

	private void FeedWaves()
	{
		// find ids to fill Waves needed data
		Guid worldId1 = dataContext.Worlds.First(world => world.Code == WorldCodes.MUNDO1).Id;
		Guid worldId2 = dataContext.Worlds.First(world => world.Code == WorldCodes.MUNDO2).Id;
		Guid robotSpyEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTSPY).Id;
		Guid robotSoldierEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTSOLDIER).Id;
		Guid robotExplorerEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTEXPLORER).Id;
		Guid robotTankEnemyId = dataContext.Enemies.First(enemy => enemy.Code == EnemyCodes.ROBOTTANK).Id;

		// mock data for Waves
		List<Wave> waves = new List<Wave>
		{
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 1,
				EnemyId = robotSpyEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 5
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 2,
				EnemyId = robotSpyEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 5
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 2,
				EnemyId = robotSoldierEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 3
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 3,
				EnemyId = robotSpyEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 4
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 3,
				EnemyId = robotSoldierEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 4
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 3,
				EnemyId = robotExplorerEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 4
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 4,
				EnemyId = robotSoldierEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 8
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 4,
				EnemyId = robotExplorerEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 8
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 5,
				EnemyId = robotSpyEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 5
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 5,
				EnemyId = robotSoldierEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 5
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 5,
				EnemyId = robotExplorerEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 5
			},
			new Wave
			{
				Id = Guid.NewGuid(),
				WorldId = worldId1,
				StageNumber = 5,
				EnemyId = robotTankEnemyId,
				EnemyLevel = 1,
				EnemyQuantity = 1
			}
		};

		// add mocked data
		dataContext.Waves.AddRange(waves);
	}

	private void FeedTowers()
	{
		// mock data for Towers
		List<Tower> towers = new List<Tower>
		{
			new Tower
			{
				Id = Guid.NewGuid(),
				Code = TowerCodes.RIFLE,
				Name = "Rifle",
				Description = "Dispara municiones a un enemigo, suele lenta pero potente."
			},
			new Tower
			{
				Id = Guid.NewGuid(),
				Code = TowerCodes.PULSE,
				Name = "Pulso",
				Description = "Dispara pulsos de energía a un enemigo, rápida y de daño aceptable."
			},
			new Tower
			{
				Id = Guid.NewGuid(),
				Code = TowerCodes.MACHINEGUN,
				Name = "Ametralladora",
				Description = "Dispara proyectiles a un área de enemigos, muy rápida, daño aceptable."
			},
			new Tower
			{
				Id = Guid.NewGuid(),
				Code = TowerCodes.ROCKET,
				Name = "Lanzacohetes",
				Description = "Dispara cohetes causando daño en área. Muy lento pero potente."
			}
		};

		// add mocked data
		dataContext.Towers.AddRange(towers);
	}

	private void FeedTowerCharacteristics()
	{
		// find ids to fill TowerCharacteristics data
		Guid ritleTowerId = dataContext.Towers.First(tower => tower.Code == TowerCodes.RIFLE).Id;
		Guid pulseTowerId = dataContext.Towers.First(tower => tower.Code == TowerCodes.PULSE).Id;
		Guid machinegunTowerId = dataContext.Towers.First(tower => tower.Code == TowerCodes.MACHINEGUN).Id;
		Guid rocketTowerId = dataContext.Towers.First(tower => tower.Code == TowerCodes.ROCKET).Id;

		// mock data for TowerCharacteristics
		List<TowerCharacteristic> towerCharacteristics = new List<TowerCharacteristic>
		{
			new TowerCharacteristic
			{
				TowerId = ritleTowerId,
				Attack = 9f,
				AttackMultiplierByLevel = 9f,
				Range = GameDefaults.shootingAreaBase,
				RangeMultiplierByLevel = 0.5f,
				AttacksPerSecond = 0.8f,
				AttacksPerSecondMultiplierByLevel = 0.4f,
				DamageInArea = false,
				PersistAttackTimes = 0,
				BuyCost = 50,
				UpgradeCost = 30,
				UpgradeCostMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new TowerCharacteristic
			{
				TowerId = pulseTowerId,
				Attack = 7f,
				AttackMultiplierByLevel = 5f,
				Range = GameDefaults.shootingAreaBase,
				RangeMultiplierByLevel = 0.5f,
				AttacksPerSecond = 2f,
				AttacksPerSecondMultiplierByLevel = 0.5f,
				DamageInArea = false,
				PersistAttackTimes = 0,
				BuyCost = 60,
				UpgradeCost = 35,
				UpgradeCostMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new TowerCharacteristic
			{
				TowerId = machinegunTowerId,
				Attack = 8f,
				AttackMultiplierByLevel = 6f,
				Range = GameDefaults.shootingAreaBase,
				RangeMultiplierByLevel = 0.5f,
				AttacksPerSecond = 4f,
				AttacksPerSecondMultiplierByLevel = 1f,
				DamageInArea = true,
				PersistAttackTimes = 0,
				BuyCost = 70,
				UpgradeCost = 40,
				UpgradeCostMultiplierByLevel = 2,
				MaxLevel = 4
			},
			new TowerCharacteristic
			{
				TowerId = rocketTowerId,
				Attack = 15,
				AttackMultiplierByLevel = 10f,
				Range = GameDefaults.shootingAreaBase,
				RangeMultiplierByLevel = 0.5f,
				AttacksPerSecond = 0.5f,
				AttacksPerSecondMultiplierByLevel = 0.25f,
				DamageInArea = true,
				PersistAttackTimes = 3,
				BuyCost = 80,
				UpgradeCost = 45,
				UpgradeCostMultiplierByLevel = 2,
				MaxLevel = 4
			}
		};

		// add mocked data
		dataContext.TowerCharacteristics.AddRange(towerCharacteristics);
	}
}