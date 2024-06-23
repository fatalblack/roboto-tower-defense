using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionItemService : MonoBehaviour
{
    // public variables
    public PlayerTower playerTower;
    public Tower tower;
    public Action sellCallback;
    public Action setTowerPositionCallback;
    public Action removeTowerPositionCallback;
    public Action upgradeTowerCallback;

    // private variables
    private Sprite sprite;
    private Image itemImage;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemPrice;
    private TextMeshProUGUI itemDescription;
    private TextMeshProUGUI statDamageBoxDescription;
    private TextMeshProUGUI statCooldownBoxDescription;
    private TextMeshProUGUI statAreaDamageBoxDescription;
    private GameManager gameManager;
    private Guid currentUserId;
    private SpriteManager spriteManager;
    private int sellingPrice;
    private int upgradePrice;
    private Guid playerTowerId;
    private int level;

    public void SetInitialConfiguration()
	{
        itemImage = gameObject.GetComponentsInChildren<Image>().First(component => component.CompareTag(Tags.ItemImage));
        itemName = gameObject.GetComponentsInChildren<TextMeshProUGUI>().First(component => component.CompareTag(Tags.ItemName));
        itemDescription = gameObject.GetComponentsInChildren<TextMeshProUGUI>().First(component => component.CompareTag(Tags.ItemDescription));
        itemPrice = gameObject.GetComponentsInChildren<TextMeshProUGUI>().First(component => component.CompareTag(Tags.ItemPrice));
        statDamageBoxDescription = gameObject.GetComponentsInChildren<TextMeshProUGUI>()
            .First(component => component.transform.parent.gameObject.CompareTag(Tags.StatDamageBox));
        statCooldownBoxDescription = gameObject.GetComponentsInChildren<TextMeshProUGUI>()
            .First(component => component.transform.parent.gameObject.CompareTag(Tags.StatCooldownBox));
        statAreaDamageBoxDescription = gameObject.GetComponentsInChildren<TextMeshProUGUI>()
            .First(component => component.transform.parent.gameObject.CompareTag(Tags.StatAreaDamageBox));
        gameManager = GameManager.Instance;
        spriteManager = GameObject.Find(Tags.SpriteManager).GetComponent<SpriteManager>();
        playerTowerId = playerTower.Id;
        level = playerTower.TowerLevel;

        // gets the current user id
        currentUserId = gameManager.GetCurrentPlayerId();

        // calculates and set selling price
        SetSellingPrice();

        // calculates and set upgrade price
        SetUpgradePrice();

        // Sets the current sprite
        SetSprite();

        // Changes the item image for the provided
        SetItemImage();

        // Changes the item name for the provided tower
        SetItemName();

        // Changes the item description for the provided tower
        SetItemDescription();

        // Changes the item price for the provided tower
        SetItemPrice();

        // calculates the damage by level
        TowerStatsResult towerStatsResult = TowerCalculations.CalculateStats(level, tower.Characteristic);

        // Changes the stat damage description for the provided tower
        SetStatDamageBoxDescription(towerStatsResult);

        // Changes the stat cooldown description for the provided tower
        SetStatCooldownBoxDescription(towerStatsResult);

        // Changes the stat area damage description for the provided tower
        SetStatAreaDamageBoxDescription(towerStatsResult);
    }

    public void SellItem()
    {
        sellCallback();
    }

    public void SetTowerPosition()
    {
        setTowerPositionCallback();
    }

    public void RemoveTowerPosition()
    {
        removeTowerPositionCallback();
    }

    public void UpgradeTower()
    {
        upgradeTowerCallback();
    }

    private void SetSellingPrice()
	{
        // calculates selling price
        sellingPrice = PlayerTowerCalculations.CalculateSellingPrice(
            playerTower.TowerLevel,
            tower.Characteristic.BuyCost,
            tower.Characteristic.UpgradeCost,
            tower.Characteristic.UpgradeCostMultiplierByLevel);
    }

    private void SetUpgradePrice()
    {
        // calculates selling price
        upgradePrice = PlayerTowerCalculations.CalculateUpgradeCost(
            playerTower.TowerLevel + 1,
            tower.Characteristic.UpgradeCost,
            tower.Characteristic.UpgradeCostMultiplierByLevel);
    }

    private void SetSprite()
	{
        switch (tower.Code)
		{
            case TowerCodes.RIFLE:
                sprite = spriteManager.TowerRifle;
                break;
            case TowerCodes.PULSE:
                sprite = spriteManager.TowerPulse;
                break;
            case TowerCodes.MACHINEGUN:
                sprite = spriteManager.TowerMachinegun;
                break;
            case TowerCodes.ROCKET:
                sprite = spriteManager.TowerRocket;
                break;
        }
	}

    private void SetItemImage()
    {
        // Changes the item image for the provided
        itemImage.sprite = sprite;
    }

    private void SetItemName()
    {
        // Changes the item name for the provided tower
        itemName.text = $"<size=90%>{tower.Name} <size=70%>nv{level} (nv{level+1} ${upgradePrice})";
    }

    private void SetItemDescription()
    {
        // Changes the item description for the provided tower
        itemDescription.text = tower.Description;
    }

    private void SetItemPrice()
    {
        // Changes the item price for the provided tower
        itemPrice.text = $"${sellingPrice}";
    }

    private void SetStatDamageBoxDescription(TowerStatsResult towerStatsResult)
    {
        // Changes the stat damage description for the provided tower
        statDamageBoxDescription.text = towerStatsResult.Attack.ToString();
    }

    private void SetStatCooldownBoxDescription(TowerStatsResult towerStatsResult)
    {
        // Changes the stat cooldown description for the provided tower
        statCooldownBoxDescription.text = towerStatsResult.AttacksPerSecond.ToString();
    }

    private void SetStatAreaDamageBoxDescription(TowerStatsResult towerStatsResult)
    {
        // Changes the stat area damage description for the provided tower
        //statAreaDamageBoxDescription.text = towerStatsResult.DamageInArea ? "SÍ" : "NO";
        statAreaDamageBoxDescription.text = towerStatsResult.Range.ToString();
    }
}