using System;

public class EnemyCharacteristic
{
	public Guid EnemyId { get; set; }

	public float Health { get; set; }

	public float HealthMultiplierByLevel { get; set; }

	public float MovementSpeed { get; set; }

	public float MovementSpeedMultiplierByLevel { get; set; }

	public int Gold { get; set; }

	public int GoldMultiplierByLevel { get; set; }

	public int MaxLevel { get; set; }
}