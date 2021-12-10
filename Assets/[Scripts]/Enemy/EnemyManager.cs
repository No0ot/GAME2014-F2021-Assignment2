//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 23, 2021
//      File            : EnemyManager.cs
//      Description     : This script contains the list of enemies for the specific type used. Gets/spawns/increments health of the enemys in the list. 
//      History         :   v0.5 - Created the Manager class that accesses the attached factory to instatiate enemies and spawn them into the game.
//                          v0.55 - Added Enemy Type to set the type of enemy the manager will handle
//                          v0.7 - Added Increment health method.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance { get { return instance; } }

    List<GameObject> enemyList;

    private EnemyFactory factory;

    Transform spawnLocation;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        enemyList = new List<GameObject>();
        factory = GetComponent<EnemyFactory>();

        foreach(EnemyScript enemy in GetComponentsInChildren<EnemyScript>(true))
        {
            enemyList.Add(enemy.gameObject);
        }
    }
    /// <summary>
    /// Accesses factory to create a new enemy.
    /// </summary>
    /// <returns></returns>
   private GameObject AddEnemy(EnemyType type)
   {
       GameObject newEnemy = factory.CreateEnemy(type);
       newEnemy.transform.SetParent(transform);
       newEnemy.SetActive(false);
       enemyList.Add(newEnemy);
       
       return newEnemy;
   }
    /// <summary>
    /// Returns a reference to the first available enemy in the list or adds a new one if non available
    /// </summary>
    /// <returns></returns>
   public GameObject GetEnemy(EnemyType type)
   {
       foreach (GameObject enemy in enemyList)
       {
            EnemyScript enemycomp = enemy.GetComponent<EnemyScript>();
            if (enemycomp.type == type)
            {
                if (!enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }
       }
       return AddEnemy(type);
   }
    /// <summary>
    /// Spawns enemy into the gameplay grid and sets there active state to true
    /// </summary>


}
