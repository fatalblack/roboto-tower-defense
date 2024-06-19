using System;

public class PlayerTower
{
	public Guid Id { get; set; }

	public Guid PlayerId { get; set; }

	public Guid WorldId { get; set; }

	public int StageNumber { get; set; }

	public Guid TowerId { get; set; }

	public int TowerLevel { get; set; }

	public int? IndexColumn { get; set; }

	public int? IndexRow { get; set; }
}