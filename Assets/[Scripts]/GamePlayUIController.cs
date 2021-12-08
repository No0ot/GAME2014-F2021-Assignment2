using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    public PlayerController player;

    public Slider healthBar;
    public Slider energyBar;


    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.health / player.maxHealth;
        energyBar.value = player.energy / player.maxEnergy;
    }
}
