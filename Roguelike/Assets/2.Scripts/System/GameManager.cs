using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GoblinType { Sword, Bow, Shield };

    public GameObject swordGoblinPrefab;
    public GameObject bowGoblinPrefab;
    public GameObject shieldGoblinPrefab;

    public Transform[] swordGoblinSpawnPoints;
    public Transform[] bowGoblinSpawnPoints;
    public Transform[] shieldGoblinSpawnPoints;

    public void SpawnGoblin(GoblinType goblinType)
    {
        GameObject goblinPrefab = null;
        Transform[] spawnPoints = null;

        switch (goblinType)
        {
            case GoblinType.Sword:
                goblinPrefab = swordGoblinPrefab;
                spawnPoints = swordGoblinSpawnPoints;
                break;
            case GoblinType.Bow:
                goblinPrefab = bowGoblinPrefab;
                spawnPoints = bowGoblinSpawnPoints;
                break;
            case GoblinType.Shield:
                goblinPrefab = shieldGoblinPrefab;
                spawnPoints = shieldGoblinSpawnPoints;
                break;
        }

        if (goblinPrefab != null && spawnPoints != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(goblinPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        if (swordGoblinSpawnPoints.Length > 0)
        {
            SpawnGoblin(GoblinType.Sword);
        }

        if (bowGoblinSpawnPoints.Length > 0)
        {
            SpawnGoblin(GoblinType.Bow);
        }

        if (shieldGoblinSpawnPoints.Length > 0)
        {
            SpawnGoblin(GoblinType.Shield);
        }
    }

    public GameObject portalObject;
    private void Update()
    {   //적이 없을때 포탈 열리게
        GameObject[] noEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (noEnemy.Length == 0)
        {
            portalObject.SetActive(true);
        }
        else
            portalObject.SetActive(false);
    }

}

