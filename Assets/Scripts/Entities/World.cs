using System;
using System.Collections.Generic;

public class World
{
	public Guid Id { get; set; }

	public WorldCodes Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int Order { get; set; }

	public int WidthBlocks { get; set; }

	public int HeightBlocks { get; set; }

	public int TotalTowersAllowed { get; set; }

	public IEnumerable<Stage> Stages { get; set; }
}