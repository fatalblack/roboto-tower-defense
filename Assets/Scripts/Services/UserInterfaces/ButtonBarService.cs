using UnityEngine;

public class ButtonBarService : MonoBehaviour
{
    // public variables
    public GameObject shopWindow;
    public GameObject towersManagerWindow;
    public GameObject towersSelectionWindow;
    public GameObject towerSelectedWindow;
    public GameObject audioSourceGo;

    // private variables
    private GameManager gameManager;
    private AudioSourceSoundService audioSourceSoundService;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();
        audioSourceSoundService = audioSourceGo.GetComponent<AudioSourceSoundService>();
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
            shopWindow.SetActive(false);
            towersManagerWindow.SetActive(false);
            towersSelectionWindow.SetActive(false);
            towerSelectedWindow.SetActive(false);
        }
    }

    public void ShopToggleVisibility()
	{
        bool active = !shopWindow.activeSelf;
        shopWindow.SetActive(active);

        // play sound
        PlayToggleWindowStatus(active);

        // hides another windows
        if (shopWindow.activeSelf)
		{
            towersManagerWindow.SetActive(false);
            towersSelectionWindow.SetActive(false);
            towerSelectedWindow.SetActive(false);
        }
	}

    public void ShopHide()
    {
        shopWindow.SetActive(false);

        // play sound
        PlayToggleWindowStatus(false);
    }

    public void TowersManagerToggleVisibility()
    {
        bool active = !towersManagerWindow.activeSelf;

        // play sound
        PlayToggleWindowStatus(active);

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towersManagerWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            // hides another windows
            shopWindow.SetActive(false);
            towersSelectionWindow.SetActive(false);
            towerSelectedWindow.SetActive(false);
        }

        towersManagerWindow.SetActive(active);
    }

    public void TowersManagerHide()
    {
        towersManagerWindow.SetActive(false);

        // play sound
        PlayToggleWindowStatus(false);
    }

    public void TowersSelectionToggleVisibility()
    {
        bool active = !towersSelectionWindow.activeSelf;

        // play sound
        PlayToggleWindowStatus(active);

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towersSelectionWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            // hides another windows
            shopWindow.SetActive(false);
            towersManagerWindow.SetActive(false);
            towerSelectedWindow.SetActive(false);
        }

        towersSelectionWindow.SetActive(active);
    }

    public void TowersSelectionHide()
    {
        towersSelectionWindow.SetActive(false);

        // play sound
        PlayToggleWindowStatus(false);
    }

    public void TowerSelectedToggleVisibility()
    {
        bool active = !towerSelectedWindow.activeSelf;

        // play sound
        PlayToggleWindowStatus(active);

        // if its visible refresh tower data
        if (active)
        {
            TowerSelectionService towerSelectionService = towerSelectedWindow.GetComponent<TowerSelectionService>();
            towerSelectionService.RefreshTowerList();

            shopWindow.SetActive(false);
            towersManagerWindow.SetActive(false);
            towersSelectionWindow.SetActive(false);
        }

        towerSelectedWindow.SetActive(active);
    }

    public void TowerSelectedHide()
    {
        towerSelectedWindow.SetActive(false);

        // play sound
        PlayToggleWindowStatus(false);
    }

    private void PlayToggleWindowStatus(bool status)
	{
        if (!status)
		{   
            audioSourceSoundService.PlayCloseWindow();
        }
		else
		{
            audioSourceSoundService.PlayOpenWindow();
        }
	}
}