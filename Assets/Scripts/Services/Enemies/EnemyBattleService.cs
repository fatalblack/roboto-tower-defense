using System;
using UnityEngine;

public class EnemyBattleService : MonoBehaviour
{
    // public variables
    public EnemyCodes enemyCode;
    public int level;

    // private variables
    private Enemy enemy;
    private EnemyStatsResult enemyStats;
    private GameManager gameManager;
    private Directions? lastRotationDirection;

    // Start is called before the first frame update
    private void Awake()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();

        enemy = gameManager.enemyDataService.GetByCodeAsync(enemyCode).Result;

        // recalculate stats
        RecalculateAll();
    }

    // Update is called once per frame
    private void Update()
    {
        // evaluates if we need to kill the enemy
        EvaluateKillEnemy();
    }

    private void OnTriggerStay(Collider other)
	{
        // evaluates if we need to move
        EvaluateRoadCollision(other);
    }

	public float TakeDamage(float damage)
	{
        EventViewerService.Instance.AddEventInfo($"Dañaste a '{enemy.Name}' por {damage}");
        // damage the enemy for the given
        enemyStats.Health -= damage;

        // returns current health
        return enemyStats.Health;
	}
    
    private void RecalculateAll()
    {
        // calculates enemy stats
        enemyStats = EnemyCalculations.CalculateStats(level, enemy.Characteristic);
    }

    private void EvaluateKillEnemy()
	{
        // if the enemy health is 0 must be destroyed
		if (enemyStats.Health <= 0)
		{
            // gives to the player the reward gold
            gameManager.AddMoneyReward(enemyStats.Gold);

            // destroy the enemy
            Destroy(gameObject);
		}
	}

    private void EvaluateRoadCollision(Collider other)
	{
        GameObject prefab = other.gameObject;

        // if collides with a road must move from this one to the next road
        if (prefab.CompareTag(Tags.Road))
		{
            // gets road and next road
            RoadBattleService roadBattleService = prefab.GetComponent<RoadBattleService>();
            GameObject nextRoad = roadBattleService.nextRoad;

            // calculates the destiny position to move the enemy
            Vector3 destinyPosition = nextRoad.transform.position - transform.position;
            // moves the enemy
            transform.Translate(destinyPosition * 0.03f, Space.World);

            // rotates the enemy to the required position if this change of direction
            SetRotationDirection(roadBattleService);
        }
    }

    private void SetRotationDirection(RoadBattleService roadBattleService)
	{
        // if the last rotation direction is equal than the road we don't need to change it
		if (lastRotationDirection != roadBattleService.nextRoadDirection)
		{
            // set the new direction as last rotation direction
            lastRotationDirection = roadBattleService.nextRoadDirection;

            // reset the enemy current rotation
            transform.rotation = Quaternion.identity;

            // set the new rotation
            float equivalentDegrees;

			switch (roadBattleService.nextRoadDirection)
			{
                case Directions.FORWARD:
                    equivalentDegrees = -90f;
                    break;
                case Directions.RIGHT:
                    equivalentDegrees = 0f;
                    break;
                case Directions.BACKWARD:
                    equivalentDegrees = -270f;
                    break;
                case Directions.LEFT:
                default:
                    equivalentDegrees = -180;
                    break;
            }

            transform.Rotate(Vector3.up, equivalentDegrees);
        }
	}
}