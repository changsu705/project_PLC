using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject warriorGoblin;
    public GameObject archerGoblin;
    public GameObject tankerGoblin;
    public Transform spawnPoint; // 스폰할 위치

    void Start()
    {
        SpawnGoblin();
    }

    void SpawnGoblin()
    {
        if (warriorGoblin != null && spawnPoint != null)
        {
            Instantiate(warriorGoblin, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Goblin prefab or spawn point is not assigned!");
        }
    }
}
