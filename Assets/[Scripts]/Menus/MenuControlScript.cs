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

    public void QuitButtonPressed()
    {
        Application.Quit();
    }


}
