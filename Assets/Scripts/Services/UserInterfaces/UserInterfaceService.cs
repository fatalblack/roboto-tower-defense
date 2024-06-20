using UnityEngine;

public class UserInterfaceService : MonoBehaviour
{
    // public variables
    public GameObject gameOverWindow;

    //private variables
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find(Tags.GameManager).GetComponent<GameManager>();

        // sets inactive as default
        gameOverWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if the player lost the game and the game over window is not visible yet, must turn on
        if (gameManager.GetGameLost() && !gameOverWindow.activeSelf)
        {
            // sets active as default
            gameOverWindow.SetActive(true);
        }
    }
}