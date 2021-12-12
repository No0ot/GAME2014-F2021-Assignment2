using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlScript : MonoBehaviour
{
    public void PlayButtonPressed()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void TempButton()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void MainMenuButton()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }


}
