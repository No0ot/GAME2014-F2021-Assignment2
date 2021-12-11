using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFactory : MonoBehaviour
{
    [Header("Loot Types")]
    public GameObject coinLootPrefab;
    public GameObject gemLootPrefab;


    public GameObject CreateLoot(LootType type)
    {
        GameObject tempLoot = null;
        switch (type)
        {
            case LootType.COIN:
                tempLoot = Instantiate(coinLootPrefab);
                break;
            case LootType.GEM:
                tempLoot = Instantiate(gemLootPrefab);
                break;

        }
        tempLoot.transform.parent = transform;
        tempLoot.SetActive(false);
        return tempLoot;
    }
}
