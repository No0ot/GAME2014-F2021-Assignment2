using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private static LootManager instance;
    public static LootManager Instance { get { return instance; } }

    List<GameObject> lootList;

    private LootFactory factory;

    Transform spawnLocation;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        lootList = new List<GameObject>();
        factory = GetComponent<LootFactory>();
    }
    /// <summary>
    /// Accesses factory to create a new enemy.
    /// </summary>
    /// <returns></returns>
    public GameObject AddLoot(LootType type)
    {
        GameObject newLoot = factory.CreateLoot(type);
        newLoot.transform.SetParent(transform);
        newLoot.SetActive(false);
        lootList.Add(newLoot);

        return newLoot;
    }
    /// <summary>
    /// Returns a reference to the first available enemy in the list or adds a new one if non available
    /// </summary>
    /// <returns></returns>
    public GameObject GetLoot(LootType type)
    {
        foreach (GameObject loot in lootList)
        {
            LootScript lootcomponent = loot.transform.GetChild(0).GetComponent<LootScript>();
            if (lootcomponent.type == type)
            {
                if (!loot.activeInHierarchy)
                {
                    return loot;
                }
            }
        }
        return AddLoot(type);
    }
}
