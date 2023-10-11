using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;
    public AudioSource countDownSound;


    private void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        countDownSound.time = 0.3f;
        countDownSound.Play();
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1f);

        countdownDisplay.gameObject.SetActive(false);
    }
}
