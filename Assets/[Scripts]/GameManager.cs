using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject UIControls;
    public GameObject endPanel;
    public GameObject finalRoom;
    public GameObject player;

    public int finalScore;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                SoundManager.Instance.PlayMenuMusic();
                break;
            case 1:
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        UIControls.SetActive(true);
                        break;
                }
                SoundManager.Instance.PlayGameplayMusic();
                break;
            case 2:
                StopAllCoroutines();
                SoundManager.Instance.PlayEndSceneMusic();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            endPanel.SetActive(true);
            finalRoom.SetActive(true);
        }
    }

    public void DeathScene()
    {
        finalScore = player.GetComponent<PlayerProgressionScript>().scoreNum;
        SceneManager.LoadScene("EndScene");
    }
}
