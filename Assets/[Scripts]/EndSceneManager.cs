using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text killsText;
    private void Start()
    {
        killsText.SetText(GameManager.Instance.finalScore.ToString());
        SoundManager.Instance.PlayEndSceneMusic();
    }
}
