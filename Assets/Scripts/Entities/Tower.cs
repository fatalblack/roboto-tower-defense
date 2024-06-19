using System;

public class Tower
{
	public Guid Id { get; set; }

	public TowerCodes Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public virtual TowerCharacteristic Characteristic { get; set; }
}