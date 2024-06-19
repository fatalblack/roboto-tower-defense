using System;

public class Player
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public int Health { get; set; }

	public int Gold { get; set; }

	public Guid CurrentWorldId { get; set; }

	public int CurrentStageNumber { get; set; }
}