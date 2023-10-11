using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score1Text;
    [SerializeField] private TextMeshProUGUI score2Text;

    void Start()
    {
        score1Text.text = "Player 1: " + FinishLine.Player1Score.ToString();
        score2Text.text = "Player 2: " + FinishLine.Player2Score.ToString();
    }
}
