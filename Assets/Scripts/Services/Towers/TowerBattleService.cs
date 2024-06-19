using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TowerBattleService : MonoBehaviour
{
    // public variables
    public TowerCodes towerCode;
    public int level;
    public GameObject towerAssigned;
    public Tower tower;

    // private variables
    private bool attacking;
    private int currentLevelSetted;
    private int? currentEnemyAttackedHashCode;
    //private Tower tower;
    private TowerStatsResult towerStats;
    private GameObject currentEnemyTargeted;
    private List<GameObject> enemiesInShootingArea;
    private Animator towerAnimator;

    // injections
    //[Inject] private readonly ITowerDataService towerDataService;

    // Start is called before the first frame update
    private void Start()
    {
        /*tower = towerDataService.GetByCodeAsync(towerCode).Result;
        enemiesInShootingArea = new List<GameObject>();
        towerAnimator = towerAssigned.GetComponent<Animator>();

        // by default we don't targeted an enemy
        currentEnemyTargeted = null;

        // recalculate stats, area and others
        RecalculateAll();*/
    }

    // Update is called once per frame
    private void Update()
    {
        // if the tower level changed must recalculate stats and set the radio
		if (currentLevelSetted != level)
		{
            // recalculate stats, area and others
            RecalculateAll();
        }   
    }

	private void OnTriggerEnter(Collider other)
	{
        // evaluates if can attack an enemy
        EvaluateEnemyCollition(other);
    }

	private void OnTriggerExit(Collider other)
	{
        // evaluates if can finish an attack
        EvaluateTargetEnemyOutOfArea(other);
    }

    public void SetInitialConfiguration()
	{
        enemiesInShootingArea = new List<GameObject>();
        towerAnimator = towerAssigned.GetComponent<Animator>();

        // by default we don't targeted an enemy
        currentEnemyTargeted = null;

        // recalculate stats, area and others
        RecalculateAll();
    }

	private void UpdateShootingAreaSize()
    {
        // sets the new value for scale
        transform.localScale = new Vector3(towerStats.Range, transform.localScale.y, towerStats.Range);
    }

    private void RecalculateAll()
	{
        // calculates tower stats
        towerStats = TowerCalculations.CalculateStats(level, tower.Characteristic);

        // register the last level setted
        currentLevelSetted = level;

        // sets the range to shooting area
        UpdateShootingAreaSize();
    }

    private void EvaluateEnemyCollition(Collider collider)
	{
        // if an enemy collitioned with the tower and is not attacking yet, can attack
		if (collider.gameObject.CompareTag(Tags.Enemy) && !attacking)
		{
            // sets the last enemy attacked hash code to identy in the OnTriggerExit event
            currentEnemyAttackedHashCode = collider.GetHashCode();

            // mark attacking as true
            attacking = true;

            // sets the current enemy targeted
            currentEnemyTargeted = collider.gameObject;

            // attack
            StartCoroutine(Attack());
        }
        // if is an enemy but the tower is busy, must add the enemy to the list EnemiesInShooting
        else if (collider.gameObject.CompareTag(Tags.Enemy) && attacking)
		{
            UpdateEnemiesInShootingArea(collider.gameObject, true);
        }
    }

    private void EvaluateTargetEnemyOutOfArea(Collider collider)
	{
        // if the current target enemy leaves our shooting area must focus the attack in another one
        if (collider.gameObject.CompareTag(Tags.Enemy) && currentEnemyAttackedHashCode == collider.GetHashCode() && attacking)
        {
            // if we have another enemy in the list must attack it
            if (enemiesInShootingArea.Any())
            {
                // set the first enemy in the list EnemiesInShootingArea as currentEnemyTargeted
                currentEnemyTargeted = enemiesInShootingArea.First();

                // remove the enemy from the list EnemiesInShootingArea
                UpdateEnemiesInShootingArea(currentEnemyTargeted, false);
            }
            else
            {
                // when no enemies are in list EnemiesInShootingArea must reset the attack status
                ResetAttackStatus();
            }
        }
        else if (collider.gameObject.CompareTag(Tags.Enemy))
		{
            // removes the enemy from the list EnemiesInShooting
            UpdateEnemiesInShootingArea(collider.gameObject, false);
        }
    }

    private IEnumerator Attack()
	{
		while (currentEnemyTargeted != null)
        {
            // activate the animator trigger to perform an attack
            towerAnimator.SetTrigger(AnimatorParameters.Shoot);

            // find EnemyBattleService component who manages the enemy damage
            EnemyBattleService enemyBattleService = currentEnemyTargeted.GetComponent<EnemyBattleService>();

            // apply damage equal than tower attack
            float enemyCurrentHealth = enemyBattleService.TakeDamage(towerStats.Attack);

            // if enemyCurrentHealth is equal or lower than 0 the enemy was killed so we need to reset all to attack another one
            if (enemyCurrentHealth <= 0)
            {
				// if we have another enemy in the list must attack it
				if (enemiesInShootingArea.Any())
				{
                    // set the first enemy in the list EnemiesInShootingArea as currentEnemyTargeted
                    currentEnemyTargeted = enemiesInShootingArea.First();

                    // remove the enemy from the list EnemiesInShootingArea
                    UpdateEnemiesInShootingArea(currentEnemyTargeted, false);
				}
				else
				{
                    // when no enemies are in list EnemiesInShootingArea must reset the attack status
                    ResetAttackStatus();
                }
            }

            // calculates AttackIntervalInSeconds
            float attackIntervalInSeconds = TowerCalculations.CalculateAttackIntervalInSeconds(towerStats.AttacksPerSecond);

            // delay so many seconds as tower attack per seconds
            yield return new WaitForSeconds(attackIntervalInSeconds);
        }
    }

    private void ResetAttackStatus()
	{
        // reset all the variables related to attack
        attacking = false;
        currentEnemyTargeted = null;
        currentEnemyAttackedHashCode = null;
    }

    private void UpdateEnemiesInShootingArea(GameObject enemy, bool isEnter)
	{
        // if its Enter trigger event adds the enemy to the list
		if (isEnter)
		{
            enemiesInShootingArea.Add(enemy);
		}
        // if its Exit trigger event remove the enemy from the list
		else
		{
            enemiesInShootingArea.Remove(enemy);
        }
    }
}