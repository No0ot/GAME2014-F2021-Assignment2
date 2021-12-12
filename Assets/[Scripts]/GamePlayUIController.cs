using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayUIController : MonoBehaviour
{
    public GameObject player;
    PlayerController playerController;
    PlayerProgressionScript progress;
    public Slider healthBar;
    public Slider energyBar;
    public TMP_Text score;


    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        progress = player.GetComponent<PlayerProgressionScript>();
    }
    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerController.health / playerController.maxHealth;
        energyBar.value = playerController.energy / playerController.maxEnergy;
        score.text = progress.scoreNum.ToString();
    }

}
