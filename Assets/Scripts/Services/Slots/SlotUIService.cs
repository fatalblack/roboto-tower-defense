using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUIService : MonoBehaviour
{
    // public variables
    public GameObject towersSelectionWindow;
    public GameObject towerSelectedWindow;
    public GameObject shopWindow;
    public GameObject towersManagerWindow;
    public int indexColumn = 1;
    public int indexRow = 1;

    // private variables
    private SlotBattleService slotBattleService;
    private GameManager gameManager;
    private bool inBattle;
    private bool isEmptySlot;

    // Start is called before the first frame update
    private void Start()
    {
        slotBattleService = GetComponent<SlotBattleService>();
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();
        inBattle = gameManager.GetInBattle();
        isEmptySlot = slotBattleService.GetIsEmptySlot();
    }

	private void OnMouseDown()
	{
        // just can manage towers if the player is not in battle 
		if (!inBattle && !IsPointerOverUIObject())
		{
            // refresh info about if the slot is empty
            isEmptySlot = slotBattleService.GetIsEmptySlot();

			// if the slot is empty show the tower selection window
			if (isEmptySlot)
			{
                TowerSelectionService towerSelectionService = towersSelectionWindow.GetComponent<TowerSelectionService>();

                // set the index of the slot on the grid
                towerSelectionService.indexColumn = indexColumn;
                towerSelectionService.indexRow = indexRow;
                towerSelectionService.onlySelected = false;

                // refreshs tower list
                towerSelectionService.RefreshTowerList();

                // configure the current slot
                towerSelectionService.currentSlot = gameObject;

                // shows the list
                towersSelectionWindow.SetActive(true);

                // hides another windows
                towerSelectedWindow.SetActive(false);
            }
			else
			{
                TowerSelectionService towerSelectionService = towerSelectedWindow.GetComponent<TowerSelectionService>();

                // set the index of the slot on the grid
                towerSelectionService.indexColumn = indexColumn;
                towerSelectionService.indexRow = indexRow;
                towerSelectionService.onlySelected = true;

                // refreshs tower list
                towerSelectionService.RefreshTowerList();

                // configure the current slot
                towerSelectionService.currentSlot = gameObject;

                // shows the list
                towerSelectedWindow.SetActive(true);

                // hides another windows
                towersSelectionWindow.SetActive(false);
            }

            // hides another windows
            shopWindow.SetActive(false);
            towersManagerWindow.SetActive(false);
        }
	}

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		return results.Count > 0;
    }
}