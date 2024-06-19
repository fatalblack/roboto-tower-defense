using System;
using System.Collections.Generic;

public class Stage
{
	public Guid WorldId { get; set; }

	public int StageNumber { get; set; }

	public IEnumerable<Wave> Waves { get; set; }
}