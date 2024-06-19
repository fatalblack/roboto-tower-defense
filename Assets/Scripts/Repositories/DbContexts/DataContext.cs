using System;
using System.Collections.Generic;

[Serializable]
public struct DataContext
{
	public List<Enemy> Enemies { get; set; }

	public List<EnemyCharacteristic> EnemyCharacteristics { get; set; }

	public List<Tower> Towers { get; set; }

	public List<TowerCharacteristic> TowerCharacteristics { get; set; }

	public List<World> Worlds { get; set; }

	public List<Stage> Stages { get; set; }

	public List<Wave> Waves { get; set; }

	public List<Player> Players { get; set; }

	public List<PlayerTower> PlayerTowers { get; set; }
}