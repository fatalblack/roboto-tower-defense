using TMPro;
using UnityEngine;

public class HeaderInfoService : MonoBehaviour
{
    // public variables
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveBoxLine1Text;
    public TextMeshProUGUI waveBoxLine2Text;
    public GameObject startButton;

    // private variables
    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();

        // sets default waveBoxLine1Text value
        waveBoxLine1Text.text = string.Empty;
        // sets default waveBoxLine2Text value
        waveBoxLine2Text.text = string.Empty;
    }

    // Update is called once per frame
    private void Update()
    {
        // gets updated values and set to text labels
        SetHealthValue();
        SetMoneyValue();
        SetWaveBoxLine1TextValue();
        SetWaveBoxLine2TextValue();
    }

    public void StartGame()
	{
        // starts the game
        gameManager.StartGame();

        // hides start button
        startButton.SetActive(false);
    }

    private void SetHealthValue()
	{
        healthText.text = gameManager.GetHealth().ToString();
    }

    private void SetMoneyValue()
    {
        moneyText.text = gameManager.GetMoney().ToString();
    }

    private void SetWaveBoxLine1TextValue()
    {
        waveBoxLine1Text.text = gameManager.GetCurrentWorldText();
    }

    private void SetWaveBoxLine2TextValue()
    {
        waveBoxLine2Text.text = gameManager.GetCurrentCountdown();
    }
}