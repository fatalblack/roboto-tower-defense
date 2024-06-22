using System;
using UnityEngine;
using Zenject;

public class PlayerHitBoxBattleService : MonoBehaviour
{
    // private variables
    private GameManager gameManager;
    private Guid currentUserId;
    private GameObject audioSourceGo;
    private AudioSourceSoundService audioSourceSoundService;

    // injections
    [Inject] private readonly IPlayerDataService playerDataService;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();
        audioSourceGo = GameObject.FindGameObjectWithTag(Tags.AudioSourceSound);
        audioSourceSoundService = audioSourceGo.GetComponent<AudioSourceSoundService>();

        // gets the current user id
        currentUserId = gameManager.GetCurrentPlayerId();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
	{
        // if collides against an enemy destroy it and damage the player
        if (other.gameObject.CompareTag(Tags.Enemy))
		{
            // plays sound
            audioSourceSoundService.PlayError();

            // damages player
            Player player = playerDataService.Damage(currentUserId).Result.Value;

            // updates health on gamemanager
            gameManager.SetHealth(player.Health);

            // destroys enemy
            Destroy(other.gameObject);

			// if players health is lower or equal than 0 set gamelost
			if (player.Health <= 0)
			{
                gameManager.SetGameLost();
            }
		}
	}
}