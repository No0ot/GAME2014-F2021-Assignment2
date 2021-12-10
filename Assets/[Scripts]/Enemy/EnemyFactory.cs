//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 22, 2021
//      File            : EnemyFactory.cs
//      Description     : This script contains the methods used to instantiate different enemys along with the method to Increment health of any enemys to be instantiated.
//      History         :   v0.5 - Added Create Enemy method.
//                          v0.7 - Added Increment health method.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Header("Enemy Types")]
    public GameObject goblinEnemyPrefab;
    public GameObject flyingEnemyPrefab;
    public GameObject skeletonEnemyPrefab;

    /// <summary>
    /// Creates an enemy based off of the passed in enemytype
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject CreateEnemy(EnemyType type)
    {
        GameObject tempEnemy = null;
        switch (type)
        {
            case EnemyType.GOBLIN:
                tempEnemy = Instantiate(goblinEnemyPrefab);
                break;
            case EnemyType.FLYINGEYE:
                tempEnemy = Instantiate(flyingEnemyPrefab);
                break;
            case EnemyType.SKELETON:
                tempEnemy = Instantiate(skeletonEnemyPrefab);
                break;
        }
        tempEnemy.transform.parent = transform;
        tempEnemy.SetActive(false);
        return tempEnemy;
    }
}
