using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SpawnerBattleService : MonoBehaviour
{
    // public variables
    public WorldCodes worldCode;
    public int stageNumber;
    public List<GameObject> enemiesPrefabs;

    // private variables
    private GameManager gameManager;
    private IEnumerable<Wave> waves;
    private bool waveStarted;
    private bool countdownToNextStageStarted;

    // injections
    [Inject] private readonly IWaveDataService waveDataService;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();

        waves = waveDataService.GetByWorldCodeAndStageNumberAsync(worldCode, stageNumber).Result;

        // sets wave started as false by default
        waveStarted = false;

        // sets countdownToNextStageStarted as false by default
        countdownToNextStageStarted = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // if conditions match starts the wave
		if (
            gameManager.GetGameStarted() &&
            gameManager.GetCurrentWorldCode() == worldCode &&
            gameManager.GetCurrentStageNumber() == stageNumber &&
            !waveStarted)
		{
            StartWave();
        }

        // if conditions match must evaluate move to next stage
        if (
            gameManager.GetGameStarted() &&
            gameManager.GetCurrentWorldCode() == worldCode &&
            gameManager.GetCurrentStageNumber() == stageNumber &&
            waveStarted &&
            !countdownToNextStageStarted)
        {
            MoveToNextStage();
        }
    }

    private void StartWave()
	{
        // spawn enemies
        StartCoroutine(SpawnEnemies());

        // sets wave started as true
        waveStarted = true;
    }

    private void MoveToNextStage()
	{
        // gets if any enemy is alive
        bool anyEnemyAlive = gameObject.GetComponentsInChildren<Transform>().Any(child => child.gameObject.CompareTag(Tags.Enemy));

        // if the spawner has not enemies must move to next stage
        if (!anyEnemyAlive)
        {
            // sets countdownToNextStageStarted as true
            countdownToNextStageStarted = true;

            // moves to next stage
            gameManager.MoveToNextStage();
        }
    }

    private IEnumerator SpawnEnemies()
	{
        // explore each wave and spawn the enemies
        foreach (Wave wave in waves)
        {
            // gets the enemy
            GameObject enemyPrefab = GetEnemyPrefab(wave.Enemy.Code);

            // instantiate x enemies where x is EnemyQuantity
            for (int i = 0; i < wave.EnemyQuantity; i++)
			{
                // sets enemy position
                enemyPrefab.transform.position = GetSpawnPosition();

                // instantiate the enemy into the spawn
                Instantiate(enemyPrefab, transform);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private GameObject GetEnemyPrefab(EnemyCodes code)
	{
        // find and return the enemy by code in the gamemanager
        return enemiesPrefabs.FirstOrDefault(enemyPrefab => enemyPrefab.GetComponent<EnemyBattleService>().enemyCode == code);
    }

    private Vector3 GetSpawnPosition()
	{
        // Generates random Z position
        float zPosition = Random.Range(-1.5f, 1.5f);

        // return position
        return new Vector3(0f, 0f, zPosition);
	}
}