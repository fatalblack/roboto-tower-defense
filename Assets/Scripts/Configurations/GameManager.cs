using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    // public variables
    public IPlayerDataService playerDataService;
    public IEnemyDataService enemyDataService;
    public IPlayerTowerDataService playerTowerDataService;

    // private variables
    private bool gameLost;
    private Player player;
    private bool inBattle;

    // injections
    [Inject] private readonly IPlayerDataService playerDataServiceInjection;
    [Inject] private readonly IEnemyDataService enemyDataServiceInjection;
    [Inject] private readonly IPlayerTowerDataService playerTowerDataServiceInjection;

    // instance
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
		if (Instance == null)
        {
            // set injections to expose
            playerDataService = playerDataServiceInjection;
            enemyDataService = enemyDataServiceInjection;
            playerTowerDataService = playerTowerDataServiceInjection;

            // creates player
            CreatePlayer();

            // sets gamelost to false
            gameLost = false;

            // sets in battle to false
            inBattle = false;

            // set instance
            Instance = this;
        }
    }

    public Guid GetCurrentPlayerId()
	{
        return player.Id;
	}

    public void SetGameLost()
	{
        // sets gamelost status to true
        gameLost = true;
	}

    public void SetInBattle()
    {
        // sets inBattle status to true
        inBattle = true;
    }

    public bool GetInBattle()
    {
        // return inBattle status
        return inBattle;
    }

    private void CreatePlayer()
	{
        // create new player
        player = playerDataService.AddAsync("Luna").Result;
    }
}