using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    // public variables
    public IPlayerDataService playerDataService;
    public IEnemyDataService enemyDataService;
    public IPlayerTowerDataService playerTowerDataService;

    // private variables
    private bool gameStarted;
    private bool gameLost;
    private Player player;
    private bool inBattle;
    private int health;
    private int money;
    private Guid currentWorldId;
    private WorldCodes currentWorldCode;
    private string currentWorldName;
    private int currentStageNumber;
    private string currentCountdown;

    // injections
    [Inject] private readonly IPlayerDataService playerDataServiceInjection;
    [Inject] private readonly IEnemyDataService enemyDataServiceInjection;
    [Inject] private readonly IPlayerTowerDataService playerTowerDataServiceInjection;
    [Inject] private readonly IWorldDataService worldDataService;

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

            // sets gameStarted to false
            gameStarted = false;

            // sets gameLost to false
            gameLost = false;

            // sets in battle to false
            inBattle = false;

            // sets health
            SetHealth(player.Health);

            // sets money
            SetMoney(player.Gold);

            // set instance
            Instance = this;
        }
    }

    public Guid GetCurrentPlayerId()
	{
        return player.Id;
	}

    public void StartGame()
	{
        // sets inBattle status to true
        inBattle = true;

        // sets gameStarted status to true
        gameStarted = true;

        // syncs world data
        SyncWorldData();
    }

    public bool GetGameStarted()
    {
        // return gameStarted status
        return gameStarted;
    }

    public void SetGameStarted()
    {
        // sets gameStarted status to true
        gameStarted = true;
    }

    public bool GetGameLost()
    {
        // return gameLost status
        return gameLost;
    }

    public void SetGameLost()
	{
        // sets gameLost status to true
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

    public int GetHealth()
	{
        // return current health
        return health;
    }

    public void SetHealth(int health)
    {
        // sets current health
        this.health = health;

		// if health is equal or lower than 0 must mark the game as gamelost
		if (this.health <= 0)
		{
            gameLost = true;
        }
    }

    public int GetMoney()
    {
        // return current money
        return money;
    }

    public void SetMoney(int money)
    {
        // sets current money
        this.money = money;
    }

    public void AddMoneyReward(int money)
    {
        // adds reward
        Player playerResponse = playerDataServiceInjection.AddReward(player.Id, money).Result.Value;

        // sets current money
        this.money = playerResponse.Gold;
    }

    public void SyncMoney()
    {
        // sets current money
        this.money = playerDataServiceInjection.GetByIdAsync(player.Id).Result.Gold;
    }

    public Guid GetCurrentWorldId()
	{
        // return currentWorldId
        return currentWorldId;
	}

    public WorldCodes GetCurrentWorldCode()
    {
        // return currentWorldCode
        return currentWorldCode;
    }

    public int GetCurrentStageNumber()
    {
        // return currentStageNumber
        return currentStageNumber;
    }

    public string GetCurrentWorldText()
	{
        // return formated current world info only if game has started
        if (gameStarted)
		{
            return $"{currentWorldName} - Nivel {currentStageNumber}";
        }

        // return empty string
        return string.Empty;
	}

    public string GetCurrentCountdown()
	{
        // return formated current countdown info only if game has started
        if (gameStarted)
        {
            return currentCountdown;
        }

        // return empty string
        return string.Empty;
    }

    public void SetCurrentCountdown(DateTime time)
    {
        // sets and format currentCountdown
        currentCountdown = string.Format("Próxima oleada en {0:mm:ss}", time);
    }

    public void MoveToNextStage()
	{
		if (!gameLost)
		{
            // sets in battle as false
            inBattle = false;

            // initates the countdown
            StartCoroutine(StartCountdown());
        }
    }

    private void CreatePlayer()
	{
        // create new player
        player = playerDataService.AddAsync("Luna").Result;
    }

    private IEnumerator StartCountdown()
	{
		for (int i = GameDefaults.countdownTimeInSeconds; i >= 0; i--)
		{
            // if the countdown reach 0 move to next stage
			if (i == 0)
			{
                // move to next stage
                player = playerDataServiceInjection.MoveToNextStage(player.Id).Result.Value;

                // syncs world data
                SyncWorldData();

                // resets countdown value
                currentCountdown = string.Empty;

                // start battle again
                inBattle = true;
			}
            // if the countdown does not reach 0 keeps countdown active
			else
			{
                DateTime time = DateTime.MinValue.AddSeconds(i);
                SetCurrentCountdown(time);

                yield return new WaitForSeconds(1f);
            }
        }
	}

    private void SyncWorldData()
	{
        // syncs world data
        currentWorldId = player.CurrentWorldId;
        World currentWorld = worldDataService.GetByIdAsync(player.CurrentWorldId).Result;
        currentWorldCode = currentWorld.Code;
        currentWorldName = currentWorld.Name;
        currentStageNumber = player.CurrentStageNumber;
    }
}