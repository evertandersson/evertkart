using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerWinText;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start()
    {
        if (FinishLine.displayPlayer1)
        {
            playerWinText.text = "Player 1 wins!";
        }
        else if (FinishLine.displayPlayer2)
        {
            playerWinText.text = "Player 2 wins!";
        }

        if (FinishLine.currentLevel == 3)
        {
            buttonText.text = "Exit";
        }
        else
        {
            buttonText.text = "Next";
        }
    }

    public void NextGame()
    {
        FinishLine.displayPlayer1 = false;
        FinishLine.displayPlayer2 = false;

        if (FinishLine.currentLevel == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (FinishLine.currentLevel == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        if (FinishLine.currentLevel == 3)
        {
            FinishLine.currentLevel = 0;
            FinishLine.Player1Score = 0;
            FinishLine.Player2Score = 0;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
