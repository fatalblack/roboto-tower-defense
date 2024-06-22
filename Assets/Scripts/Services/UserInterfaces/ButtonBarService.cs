using UnityEngine;

public class ButtonBarService : MonoBehaviour
{
    // public variables
    public GameObject shopWindow;
    public GameObject towersManagerWindow;
    public GameObject towersSelectionWindow;
    public GameObject towerSelectedWindow;

    // private variables
    GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
		// if we press S will toggles shop window visibility
		if (Input.GetKeyDown(KeyCode.S))
		{
            ShopToggleVisibility();
        }

        // if we press T will toggles tower manager window visibility
        if (Input.GetKeyDown(KeyCode.T))
        {
            TowersManagerToggleVisibility();
        }

        // if the battle is on, must hide everything
        if (gameManager.GetInBattle())
		{
            ShopHide();
            TowersManagerHide();
            TowersSelectionHide();
            TowerSelectedHide();
        }
    }

    public void ShopToggleVisibility()
	{
        shopWindow.SetActive(!shopWindow.activeSelf);

        // hides another windows
		if (shopWindow.activeSelf)
		{
            TowersManagerHide();
            TowersSelectionHide();
            TowerSelectedHide();
        }
	}

    public void ShopHide()
    {
        shopWindow.SetActive(false);
    }

    public void TowersManagerToggleVisibility()
    {
        bool active = !towersManagerWindow.activeSelf;

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towersManagerWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            // hides another windows
            ShopHide();
            TowersSelectionHide();
            TowerSelectedHide();
        }

        towersManagerWindow.SetActive(active);
    }

    public void TowersManagerHide()
    {
        towersManagerWindow.SetActive(false);
    }

    public void TowersSelectionToggleVisibility()
    {
        bool active = !towersSelectionWindow.activeSelf;

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towersSelectionWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            // hides another windows
            ShopHide();
            TowersManagerHide();
            TowerSelectedHide();
        }

        towersSelectionWindow.SetActive(active);
    }

    public void TowersSelectionHide()
    {
        towersSelectionWindow.SetActive(false);
    }

    public void TowerSelectedToggleVisibility()
    {
        bool active = !towerSelectedWindow.activeSelf;

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towerSelectedWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            ShopHide();
            TowersManagerHide();
            TowersSelectionHide();
        }

        towerSelectedWindow.SetActive(active);
    }

    public void TowerSelectedHide()
    {
        towerSelectedWindow.SetActive(false);
    }
}