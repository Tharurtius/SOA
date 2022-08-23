using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static Text scoreText;
    public static GameStates currentGameState;
    public GameObject[] uiScreens;
    public Text highScore;
    private PlayerHandler player;
    public Text endText;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        highScore = GameObject.FindGameObjectWithTag("HighScore").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        endText = GameObject.FindGameObjectWithTag("EndText").GetComponent<Text>();
        scoreText.text = "" + 0;
    }
    private void Start()
    {
        //if we do not have a saved high score
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            //create the save file and set it to 0
            PlayerPrefs.SetInt("HighScore", 0);
        }
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        player.enabled = false;
        uiScreens[1].SetActive(false);
        uiScreens[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameStates.PreGame)
        {
            //if playerhandler is on/ turn it off
            //display the UI screens for a main menu
            //a button or key press  to start the game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiScreens[1].SetActive(true);
                uiScreens[0].SetActive(false);
                currentGameState = GameStates.Game;
                player.enabled = true;
            }
        }
        else if (currentGameState == GameStates.Game)
        {
            //if player handler is off / turn it on
            //display hud
        }
        else // PostGame
        {
            if (currentGameState == GameStates.PostGame && uiScreens[1].activeSelf)
            {
                if (player.curHealth <= 0)
                {
                    endText.text = "GAME OVER";
                }
                else
                {
                    endText.text = "VICTORY";
                }
                player.enabled = false;
                if (score > PlayerPrefs.GetInt("HighScore"))
                {
                    PlayerPrefs.SetInt("HighScore", score);
                }
                uiScreens[2].SetActive(true);
                uiScreens[1].SetActive(false);
            }
            //if playerhandler is on/ turn it off
            //end game ui
            //allow restart
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //restart game
                Restart();
            }
        }
    }
    public void Restart()
    {
        for (int i = 0; i < uiScreens.Length; i++)
        {
            uiScreens[i].SetActive(true);
        }
        score = 0;
        currentGameState = GameStates.PreGame;
        Start();
        SceneManager.LoadScene(0);
    }
}
//is a comma seperated list of identigiers
public enum GameStates 
{
    PreGame,
    Game,
    PostGame
}