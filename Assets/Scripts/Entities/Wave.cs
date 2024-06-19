using System;

public class Wave
{
    public Guid Id { get; set; }

    public Guid WorldId { get; set; }

    public int StageNumber { get; set; }

    public Guid EnemyId { get; set; }

    public int EnemyLevel { get; set; }

    public int EnemyQuantity { get; set; }

    public virtual Enemy Enemy { get; set; }
}