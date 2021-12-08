using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    [Header("Button Control Events")]
    public static bool jumpButtonDown;
    public static bool lightAttackButtonDown;
    public static bool heavyAttackButtonDown;
    public void OnJumpButton_Down()
    {
        jumpButtonDown = true;
    }

    public void OnJumpButton_Up()
    {
        jumpButtonDown = false;
    }
    public void OnLightAttackButton_Down()
    {
        lightAttackButtonDown = true;
    }

    public void OnLightAttackButton_Up()
    {
        lightAttackButtonDown = false;
    }
    public void OnHeavyAttackButton_Down()
    {
        heavyAttackButtonDown = true;
    }

    public void OnHeavyAttackButton_Up()
    {
        heavyAttackButtonDown = false;
    }
}
