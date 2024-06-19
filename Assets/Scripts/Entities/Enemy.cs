using System;

public class Enemy
{
	public Guid Id { get; set; }

	public EnemyCodes Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public virtual EnemyCharacteristic Characteristic { get; set; }
}