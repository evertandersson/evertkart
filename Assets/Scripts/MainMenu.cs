using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource welcomeSound;

    public void Start()
    {
        welcomeSound.Play();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level01()
    {
        FinishLine.currentLevel = 0;
        SceneManager.LoadScene("Level01");
    }
    public void Level02()
    {
        FinishLine.currentLevel = 1;
        SceneManager.LoadScene("Level02");
    }
    public void Level03()
    {
        FinishLine.currentLevel = 2;
        SceneManager.LoadScene("Level03");
    }
}
