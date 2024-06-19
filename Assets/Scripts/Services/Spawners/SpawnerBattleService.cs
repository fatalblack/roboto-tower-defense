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

    // injections
    [Inject] private readonly IWaveDataService waveDataService;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();

        waves = waveDataService.GetByWorldCodeAndStageNumberAsync(worldCode, stageNumber).Result;

        // spawn enemies
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
	{
        // explore each wave and spawn the enemies
        foreach (Wave wave in waves)
        {
            // gets the enemy
            GameObject enemyPrefab = GetEnemyPrefab(wave.Enemy.Code);

            // sets enemy position
            enemyPrefab.transform.position = GetSpawnPosition();

            // instantiate x enemies where x is EnemyQuantity
            for (int i = 0; i < wave.EnemyQuantity - 1; i++)
			{
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