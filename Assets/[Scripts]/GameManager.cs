using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject UIControls;

    // Start is called before the first frame update
    void Start()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                UIControls.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
