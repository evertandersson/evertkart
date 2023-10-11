using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] bool touchingFinishLine = false;

    public static int currentLevel = 0;
    public static int Player1Score = 0;
    public static int Player2Score = 0;
    public static bool displayPlayer1 = false;
    public static bool displayPlayer2 = false;

    public void Start()
    {
        Debug.Log("Level " + currentLevel);
    }

    public void PlayerScored()
    {
        Debug.Log("Finished");
        touchingFinishLine = true; 
    }

    private void OnTriggerStay(Collider collision)
    {
        if (touchingFinishLine == true)
        {
            if (collision.gameObject.name == "Player")
            {
                Player1Score++;
                displayPlayer1 = true;
            }
            else if (collision.gameObject.name == "Player2")
            {
                Player2Score++;
                displayPlayer2 = true;
            }
            Debug.Log("Player 1: " + Player1Score);
            Debug.Log("Player 2: " + Player2Score);
            NextScene();
        }  
    }

    public void NextScene()
    {
        FindObjectOfType<CarController>().readyToFinish = false;
        currentLevel++;
        SceneManager.LoadScene("PlayerWins");
    }
}
