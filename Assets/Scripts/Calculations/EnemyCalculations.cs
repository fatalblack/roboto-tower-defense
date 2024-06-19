using UnityEngine;

public class EnemyCalculations : MonoBehaviour
{
    public static EnemyStatsResult CalculateStats(int level, EnemyCharacteristic enemyCharacteristic)
    {
        // initializes multipliers in 0 to be use as default for level 1
        float healthMultiplier = 0;
        float movementSpeedMultiplier = 0;
        int goldMultiplier = 0;

        // if level is greater than 1 calculates the multipliers
        if (level > 1)
        {
            healthMultiplier = enemyCharacteristic.HealthMultiplierByLevel * level;
            movementSpeedMultiplier = enemyCharacteristic.MovementSpeedMultiplierByLevel * level;
            goldMultiplier = enemyCharacteristic.GoldMultiplierByLevel * level;
        }

        // calculates health
        float health = enemyCharacteristic.Health + healthMultiplier;
        // calculates movement speed
        float movementSpeed = enemyCharacteristic.MovementSpeed + movementSpeedMultiplier;
        // calculates gold
        int gold = enemyCharacteristic.Gold + goldMultiplier;

        // return stats
        return new EnemyStatsResult
        {
            Health = health,
            MovementSpeed = movementSpeed,
            Gold = gold
        };
    }
}