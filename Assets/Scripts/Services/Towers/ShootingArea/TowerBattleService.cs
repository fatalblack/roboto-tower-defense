using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerBattleService : MonoBehaviour
{
    // public variables
    public TowerCodes towerCode;
    public int level;
    public GameObject towerAssigned;
    public Tower tower;
    public AudioClip shootingSound;
    public GameObject[] ammoSpawners;
    public GameObject ammoPrefab;

    // private variables
    private int currentLevelSetted = 1;
    private TowerStatsResult towerStats;
    private List<GameObject> enemiesInShootingArea;
    private Animator towerAnimator;
    private AudioSource audioSource;
    private GameManager gameManager;
    private float lastRotatedAngle;

	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
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

		// if the battle is off and remains an enemy in the list must be clean this one
		if (!gameManager.GetInBattle() && enemiesInShootingArea.Any())
		{
            enemiesInShootingArea = new List<GameObject>();
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
        lastRotatedAngle = 0f;

        // recalculate stats, area and others
        RecalculateAll();

        // starts the attack event
        float attackIntervalInSeconds = TowerCalculations.CalculateAttackIntervalInSeconds(towerStats.AttacksPerSecond);
        InvokeRepeating(nameof(Attack), 0f, attackIntervalInSeconds);
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
		if (collider.gameObject.CompareTag(Tags.Enemy))
		{
            UpdateEnemiesInShootingArea(collider.gameObject, true);
        }
    }

    private void EvaluateTargetEnemyOutOfArea(Collider collider)
	{
        // if the current target enemy leaves our shooting area must focus the attack in another one
        if (collider.gameObject.CompareTag(Tags.Enemy))
		{
            UpdateEnemiesInShootingArea(collider.gameObject, false);
        }
    }

    private void Attack()
	{
        GameObject enemyToAttack = enemiesInShootingArea.FirstOrDefault();

        if (enemyToAttack != null)
        {
            // spawn ammo
            SpawnAmmo();

            // rotate tower facing the enemy
            RotateTowerToEnemy();

            // activate the animator trigger to perform an attack
            towerAnimator.SetTrigger(AnimatorParameters.Shoot);

            // plays shooting sound
            PlayShootingAudio();

            // find EnemyBattleService component who manages the enemy damage
            EnemyBattleService enemyBattleService = enemyToAttack.GetComponent<EnemyBattleService>();

            // apply damage equal than tower attack
            float enemyCurrentHealth = enemyBattleService.TakeDamage(towerStats.Attack);

            // if enemyCurrentHealth is equal or lower than 0 the enemy was killed so we need to reset all to attack another one
            if (enemyCurrentHealth <= 0)
            {
                // remove the enemy from the list EnemiesInShootingArea
                UpdateEnemiesInShootingArea(enemyToAttack, false);
            }
        }
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

    private void PlayShootingAudio()
	{
        audioSource.clip = shootingSound;
        audioSource.Play();
	}

    private void RotateTowerToEnemy()
	{
        GameObject currentEnemy = enemiesInShootingArea.First();
        
        // calculates the degree angle rotation
        float angleRotation = RotationCalculations.CalculateTowerAngleByEnemyPosition(
            towerAssigned.transform.position,
            currentEnemy.transform.position);

        // change the tower y axis angle reverting the last angle rotated
        towerAssigned.transform.Rotate(Vector3.up, angleRotation - lastRotatedAngle, Space.World);

        // save the last rotated angle
        lastRotatedAngle = angleRotation;
    }

    private void SpawnAmmo()
	{
		if (ammoSpawners == null || ammoPrefab == null)
		{
            return;
		}

		// if the tower has not enemies to target don't need to do anything
		if (!enemiesInShootingArea.Any())
		{
            return;
		}

        // must spawn an ammo from each spawner
        foreach(GameObject ammoSpawner in ammoSpawners)
		{
            // instance the ammo
            GameObject instantiatePrefab = Instantiate(ammoPrefab, ammoSpawner.transform, false);
            // get AmmoBattleService to set the ammo position
            AmmoBattleService ammoBattleService = instantiatePrefab.GetComponent<AmmoBattleService>();
            Vector3 targetPosition = enemiesInShootingArea.First().transform.position;

            // set speed ammo based in towercode
            float ammoSpeed = towerCode == TowerCodes.ROCKET ? GameDefaults.ammoRocketMoveSpeedFactor : GameDefaults.ammoBulletMoveSpeedFactor;

            // initiate
            ammoBattleService.Initiate(targetPosition, ammoSpeed);
        }
    }
}