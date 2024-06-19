using System;

public class TowerCharacteristic
{
	public Guid TowerId { get; set; }

	public float Attack { get; set; }

	public float AttackMultiplierByLevel { get; set; }

	public float Range { get; set; }

	public float RangeMultiplierByLevel { get; set; }

	public float AttacksPerSecond { get; set; }

	public float AttacksPerSecondMultiplierByLevel { get; set; }

	public bool DamageInArea { get; set; }

	public int PersistAttackTimes { get; set; }

	public int BuyCost { get; set; }

	public int UpgradeCost { get; set; }

	public int UpgradeCostMultiplierByLevel { get; set; }

	public int MaxLevel { get; set; }
}