public static class TowerCalculations
{
    public static TowerStatsResult CalculateStats(int level, TowerCharacteristic towerCharacteristic)
    {
        // initializes multipliers in 0 to be use as default for level 1
        float attackMultiplier = 0;
        float rangeMultiplier = 0;
        float attacksPerSecondMultiplier = 0;

		// if level is greater than 1 calculates the multipliers
		if (level > 1)
		{
            attackMultiplier = towerCharacteristic.AttackMultiplierByLevel * level;
            rangeMultiplier = towerCharacteristic.RangeMultiplierByLevel * level;
            attacksPerSecondMultiplier = towerCharacteristic.AttacksPerSecondMultiplierByLevel * level;
        }

        // calculates attack
        float attack = towerCharacteristic.Attack + attackMultiplier;
        // calculates range
        float range = towerCharacteristic.Range + rangeMultiplier;
        // calculates attacks per second
        float attacksPerSecond = towerCharacteristic.AttacksPerSecond + attacksPerSecondMultiplier;

        // return stats
        return new TowerStatsResult
        {
            Attack = attack,
            Range = range,
            AttacksPerSecond = attacksPerSecond,
            DamageInArea = towerCharacteristic.DamageInArea,
            PersistAttackTimes = towerCharacteristic.PersistAttackTimes
        };
    }

    public static float CalculateAttackIntervalInSeconds(float attacksPerSecond)
	{
        return 1 / attacksPerSecond;
	}
}