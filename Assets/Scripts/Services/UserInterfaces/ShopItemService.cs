using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopItemService : MonoBehaviour
{
    // public variables
    public TowerCodes towerCode;
    public Sprite sprite;

    // private variables
    private Tower tower;
    private Image itemImage;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemPrice;
    private TextMeshProUGUI itemDescription;
    private TextMeshProUGUI statDamageBoxDescription;
    private TextMeshProUGUI statCooldownBoxDescription;
    private TextMeshProUGUI statAreaDamageBoxDescription;
    private GameManager gameManager;
    private Guid currentUserId;

    // injections
    [Inject] private readonly ITowerDataService towerDataService;
    [Inject] private readonly IPlayerTowerDataService playerTowerDataService;

    // Start is called before the first frame update
    private void Start()
    {
        tower = towerDataService.GetByCodeAsync(towerCode).Result;
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
        gameManager = GameObject.Find(nameof(GameManager)).GetComponent<GameManager>();

        // gets the current user id
        currentUserId = gameManager.GetCurrentPlayerId();

        // Changes the item image for the provided
        SetItemImage();

        // Changes the item name for the provided tower
        SetItemName();

        // Changes the item description for the provided tower
        SetItemDescription();

        // Changes the item price for the provided tower
        SetItemPrice();

        // Changes the stat damage description for the provided tower
        SetStatDamageBoxDescription();

        // Changes the stat cooldown description for the provided tower
        SetStatCooldownBoxDescription();

        // Changes the stat area damage description for the provided tower
        SetStatAreaDamageBoxDescription();
    }

    public void BuyItem()
	{
        GenericResponse<PlayerTower> buy = playerTowerDataService.BuyAsync(currentUserId, tower.Id).Result;

		if (buy.IsSucceeded)
		{
            EventViewerService.Instance.AddEventSuccess($"Compraste '{tower.Name}' por {tower.Characteristic.BuyCost}");
		}
		else
		{
            EventViewerService.Instance.AddEventError(buy.ErrorMessage);
        }
    }

    private void SetItemImage()
	{
        // Changes the item image for the provided
        itemImage.overrideSprite = sprite;
    }

    private void SetItemName()
    {
        // Changes the item name for the provided tower
        itemName.text = tower.Name;
    }

    private void SetItemDescription()
    {
        // Changes the item description for the provided tower
        itemDescription.text = tower.Description;
    }

    private void SetItemPrice()
    {
        // Changes the item price for the provided tower
        itemPrice.text = $"${tower.Characteristic.BuyCost}";
    }

    private void SetStatDamageBoxDescription()
    {
        // Changes the stat damage description for the provided tower
        statDamageBoxDescription.text = tower.Characteristic.Attack.ToString();
    }

    private void SetStatCooldownBoxDescription()
    {
        // Changes the stat cooldown description for the provided tower
        statCooldownBoxDescription.text = tower.Characteristic.AttacksPerSecond.ToString();
    }

    private void SetStatAreaDamageBoxDescription()
    {
        // Changes the stat area damage description for the provided tower
        statAreaDamageBoxDescription.text = tower.Characteristic.DamageInArea ? "SÍ" : "NO";
    }
}