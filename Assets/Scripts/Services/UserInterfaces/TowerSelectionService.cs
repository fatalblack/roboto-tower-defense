using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TowerSelectionService : MonoBehaviour
{
    // public variables
    public ScrollRect viewScroller;
    public GameObject contentBox;
    public GameObject itemPrefab;
    public GameObject currentSlot;
    public int indexColumn = 1;
    public int indexRow = 1;
    public float paddingBetweenItems = 10f;
    public bool onlySelected;

    // private variables
    private GameManager gameManager;
    private Guid currentUserId;
    private List<PlayerTower> playerTowers;
    private RectTransform itemPrefabRT;
    private RectTransform contentBoxRT;
    private PlayerTower currentTower;
    private PrefabManager prefabManager;

    // injections
    [Inject] private readonly ITowerDataService towerDataService;
    [Inject] private readonly IPlayerTowerDataService playerTowerDataService;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();
        itemPrefabRT = itemPrefab.GetComponent<RectTransform>();
        contentBoxRT = contentBox.GetComponent<RectTransform>();
        prefabManager = GameObject.Find(Tags.PrefabManager).GetComponent<PrefabManager>();

        // gets the current user id
        currentUserId = gameManager.GetCurrentPlayerId();

        // refresh player towers list
        RefreshTowerList();
    }

    public void RefreshTowerList()
	{
        // gets player towers
        playerTowers = playerTowerDataService.GetAllByPlayerIdAsync(currentUserId).Result.ToList();

		// if onlySelected is checked must filter the list
		if (onlySelected)
		{
            playerTowers = playerTowers
                .Where(playerTower => playerTower.IndexColumn == indexColumn && playerTower.IndexRow == indexRow)
                .ToList();
		}
		else
		{
            // filter only the one without a slot assigned
            playerTowers = playerTowers.Where(tower => !tower.IndexColumn.HasValue && !tower.IndexRow.HasValue).ToList();
        }

        // clean content
        CleanContentBox();

        // declare a counter to help us to set the 'y' axis position for each playerTower
        int itemsAddedCounter = 0;

        // instance each playerTower
        foreach (PlayerTower playerTower in playerTowers)
		{
            InstancePlayerTowerItem(playerTower, itemsAddedCounter);

            // increment in 1 the itemsAddedCounter
            itemsAddedCounter++;
        }

        // set new contentBox height
        if (contentBoxRT != null)
		{
            contentBoxRT.sizeDelta = GetContentBoxNewSize();
        }
    }

    public void HideWindow()
	{
        gameObject.SetActive(false);
	}

    private void CleanContentBox()
	{
        // remove children from contentbox
        foreach (Transform child in contentBox.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstancePlayerTowerItem(PlayerTower playerTower, int index)
	{
        // gets towerSelectionItemService from itemPrefab
        TowerSelectionItemService towerSelectionItemService = itemPrefab.GetComponent<TowerSelectionItemService>();

        // sets the tower data
        Tower tower = towerDataService.GetByIdAsync(playerTower.TowerId).Result;
        towerSelectionItemService.tower = tower;
        towerSelectionItemService.playerTower = playerTower;

        // sets initial configurations
        towerSelectionItemService.SetInitialConfiguration();

        // instance the prefab
        GameObject instantiatedPrefab = Instantiate(itemPrefab, contentBox.transform);

        // calculates and sets the instantiated prefab
        instantiatedPrefab.transform.localPosition = GetItemNewPosition(index);

        // set the sell callback to the instantiated prefab
        TowerSelectionItemService instantiatedTowerSelectionItemService = instantiatedPrefab.GetComponent<TowerSelectionItemService>();
        instantiatedTowerSelectionItemService.sellCallback = () => CallbackSellItem(playerTower, tower);

        // set the setPosition callback to the instantiated prefab
        instantiatedTowerSelectionItemService.setTowerPositionCallback = () => CallbackSetTowerPosition(playerTower, tower);

        // set the removePosition callback to the instantiated prefab
        instantiatedTowerSelectionItemService.removeTowerPositionCallback = () => CallbackRemoveTowerPosition(playerTower);

        // set the upgradeTower callback to the instantiated prefab
        instantiatedTowerSelectionItemService.upgradeTowerCallback = () => CallbackUpgradeTower(playerTower);
    }

    private Vector3 GetItemNewPosition(int index)
	{
        float yPosition = (itemPrefabRT.sizeDelta.y / 2) + paddingBetweenItems;
        float xPosition = (itemPrefabRT.sizeDelta.x / 2);

        if (index == 0)
		{
            return new Vector3(xPosition, -yPosition, 0f);
        }

        yPosition += (itemPrefabRT.sizeDelta.y + paddingBetweenItems) * index;

        return new Vector3(xPosition, -yPosition, 0f);
    }

    private Vector2 GetContentBoxNewSize()
	{
        // calculates x axis
        float width = contentBoxRT.sizeDelta.x;
        // calculates y axis
        float height = playerTowers.Count * (itemPrefabRT.sizeDelta.y + paddingBetweenItems);

        // return new position
        return new Vector2(width, height);
	}

    private void CallbackSellItem(PlayerTower playerTower, Tower tower)
	{
        // sells tower
        GenericResponse sell = playerTowerDataService.SellAsync(playerTower.Id).Result;
        // syncs money player
        gameManager.SyncMoney();

        if (sell.IsSucceeded)
        {
            // calculates selling price
            int sellingPrice = PlayerTowerCalculations.CalculateSellingPrice(
                playerTower.TowerLevel,
                tower.Characteristic.BuyCost,
                tower.Characteristic.UpgradeCost,
                tower.Characteristic.UpgradeCostMultiplierByLevel);

            EventViewerService.Instance.AddEventSuccess($"Vendiste '{tower.Name}' por {sellingPrice}");

            // refresh the list
            RefreshTowerList();

            // if no elements left, close the window
			if (!playerTowers.Any() || onlySelected)
			{
                gameObject.SetActive(false);

				// if is the current tower in slot, must be removed from it
				if (onlySelected)
				{
                    CallbackRemoveTowerPosition(playerTower);
                }
            }
        }
        else
        {
            // shows the error
            EventViewerService.Instance.AddEventError(sell.ErrorMessage);
        }
    }

    private void CallbackSetTowerPosition(PlayerTower playerTower, Tower tower)
    {
        GenericResponse<PlayerTower> setPosition = playerTowerDataService.SetPositionAsync(playerTower.Id, indexColumn, indexRow).Result;

        if (setPosition.IsSucceeded)
        {
            // sets the tower on the scene
            GameObject towerPrefab = GetTowerPrefab(tower.Code);

            // instance the prefab
            GameObject instantiatedPrefab = Instantiate(towerPrefab, currentSlot.transform, true);

            // sets the selected tower as currentTower
            currentTower = setPosition.Value;

            // sets tower battle service configuration
            TowerBattleService towerBattleService = instantiatedPrefab.GetComponentInChildren<TowerBattleService>();
            towerBattleService.tower = tower;
            towerBattleService.level = playerTower.TowerLevel;
            towerBattleService.SetInitialConfiguration();

            // calculates and sets the instantiated prefab
            instantiatedPrefab.transform.localPosition = new Vector3(0f, 0.1f, 0f);

            // hides the tower selection window
            gameObject.SetActive(false);
        }
        else
        {
            // shows the error
            EventViewerService.Instance.AddEventError(setPosition.ErrorMessage);
        }
    }

    private void CallbackRemoveTowerPosition(PlayerTower playerTower)
    {
        GenericResponse<PlayerTower> removePosition = playerTowerDataService.RemovePositionAsync(playerTower.Id).Result;

        // gets the tower gameobject from the slot
        Transform currentTowerInSlot = currentSlot.GetComponentsInChildren<Transform>().FirstOrDefault(tower => tower.CompareTag(Tags.Tower));
        GameObject currentTowerGo = currentTowerInSlot?.gameObject;

        // removes the tower from the scene
        GameObject.Destroy(currentTowerGo);

        // sets the currentTower as null, now the slot is ready to set another tower
        currentTower = null;

        // hides the tower selection window
        gameObject.SetActive(false);
    }

    private void CallbackUpgradeTower(PlayerTower playerTower)
    {
        // upgrades the tower
        GenericResponse<PlayerTower> upgradedTower = playerTowerDataService.UpgradeAsync(playerTower.Id).Result;
        // syncs money player
        gameManager.SyncMoney();

        if (upgradedTower.IsSucceeded)
        {
            // sets the upgraded currentTower
            currentTower = upgradedTower.Value;

            // gets the tower gameobject from the slot
            Transform currentTowerInSlot = currentSlot.GetComponentsInChildren<Transform>().FirstOrDefault(tower => tower.CompareTag(Tags.Tower));
			if (currentTowerInSlot != null)
			{
                // gets the shooting area and the TowerBattleService
                GameObject currentTowerGo = currentTowerInSlot.gameObject;
                TowerBattleService towerBattleService = currentTowerGo.GetComponentInChildren<TowerBattleService>();

                // refresh the gameobject tower level on the slot
                towerBattleService.level = upgradedTower.Value.TowerLevel;
            }

            // refresh the list
            RefreshTowerList();
        }
        else
        {
            // shows the error
            EventViewerService.Instance.AddEventError(upgradedTower.ErrorMessage);
        }
    }

    private GameObject GetTowerPrefab(TowerCodes towerCode)
	{
        GameObject tower = null;

        // gets the tower prefab by code
		switch (towerCode)
		{
            case TowerCodes.RIFLE:
                tower = prefabManager.rifleTower;
                break;
            case TowerCodes.PULSE:
                tower = prefabManager.pulseTower;
                break;
            case TowerCodes.MACHINEGUN:
                tower = prefabManager.machinegunTower;
                break;
            case TowerCodes.ROCKET:
                tower = prefabManager.rocketTower;
                break;
        }

        // return tower
        return tower;
	}
}