using UnityEngine;

public class ButtonBarService : MonoBehaviour
{
    // public variable
    public GameObject shopWindow;
    public GameObject towersSelectionWindow;
    public GameObject towerSelectedWindow;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
		// if we press S will toggles shop window visibility
		if (Input.GetKeyDown(KeyCode.S))
		{
            ShopToggleVisibility();
        }
    }

    public void ShopToggleVisibility()
	{
        shopWindow.SetActive(!shopWindow.activeSelf);
	}

    public void ShopHide()
    {
        shopWindow.SetActive(false);
    }

    public void TowersSelectionToggleVisibility()
    {
        bool active = !towersSelectionWindow.activeSelf;

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towersSelectionWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();
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
        }

        towerSelectedWindow.SetActive(active);
    }

    public void TowerSelectedHide()
    {
        towerSelectedWindow.SetActive(false);
    }
}