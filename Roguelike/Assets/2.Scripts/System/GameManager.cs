using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject escButton;
    public GameObject backToGameButton;
    public GameObject onClickOptionButton;
    public GameObject backToTownButton;

    private bool isEsc;
    
    

    private void Start()
    {
        Time.timeScale = 1;
        escButton.SetActive(false);
        
    }

    private void Update()
    {
        onKeyESC();
    }

    private void onKeyESC()
    {
       
            if (Input.GetKeyDown(KeyCode.Escape) && !isEsc) 
            {
                Time.timeScale = 0;
                isEsc = true;
                escButton.SetActive(true);
            }
            
            else if(Input.GetKeyDown(KeyCode.Escape) && isEsc)
            {
                Time.timeScale = 1;
                escButton.SetActive(false);
                isEsc=false;
            }
        
    }
    
   
    
   


    // public enum GoblinType { Sword, Bow, Shield };
    //
    // public GameObject swordGoblinPrefab;
    // public GameObject bowGoblinPrefab;
    // public GameObject shieldGoblinPrefab;
    //
    // public Transform[] swordGoblinSpawnPoints;
    // public Transform[] bowGoblinSpawnPoints;
    // public Transform[] shieldGoblinSpawnPoints;
    //
    // public void SpawnGoblin(GoblinType goblinType)
    // {
    //     GameObject goblinPrefab = null;
    //     Transform[] spawnPoints = null;
    //
    //     switch (goblinType)
    //     {
    //         case GoblinType.Sword:
    //             goblinPrefab = swordGoblinPrefab;
    //             spawnPoints = swordGoblinSpawnPoints;
    //             break;
    //         case GoblinType.Bow:
    //             goblinPrefab = bowGoblinPrefab;
    //             spawnPoints = bowGoblinSpawnPoints;
    //             break;
    //         case GoblinType.Shield:
    //             goblinPrefab = shieldGoblinPrefab;
    //             spawnPoints = shieldGoblinSpawnPoints;
    //             break;
    //     }
    //
    //     if (goblinPrefab != null && spawnPoints != null && spawnPoints.Length > 0)
    //     {
    //         foreach (Transform spawnPoint in spawnPoints)
    //         {
    //             Instantiate(goblinPrefab, spawnPoint.position, Quaternion.identity);
    //         }
    //     }
    // }
    //
    // private void Start()
    // {
    //     if (swordGoblinSpawnPoints.Length > 0)
    //     {
    //         SpawnGoblin(GoblinType.Sword);
    //     }
    //
    //     if (bowGoblinSpawnPoints.Length > 0)
    //     {
    //         SpawnGoblin(GoblinType.Bow);
    //     }
    //
    //     if (shieldGoblinSpawnPoints.Length > 0)
    //     {
    //         SpawnGoblin(GoblinType.Shield);
    //     }
    // }
    //
    // public GameObject portalObject;
    // private void Update()
    // {   //적이 없을때 포탈 열리게
    //     GameObject[] noEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    //
    //     if (noEnemy.Length == 0)
    //     {
    //         portalObject.SetActive(true);
    //     }
    //     else
    //         portalObject.SetActive(false);
    // }

    
    #region UI

    /// <summary>
    /// 일시 정지
    /// </summary>
    public void Esc()
    {
        
    }
    
    /// <summary>
    /// 게임으로 돌아가기
    /// </summary>
    public void BackToGame()
    {
        Time.timeScale = 1;
        escButton.SetActive(false);
        isEsc=false;
    }
    
    /// <summary>
    /// 옵션 들어가기
    /// </summary>
    public void OnClickOption()
    {
        
    }
    
    /// <summary>
    /// 마을로 돌아가기
    /// </summary>
    public void BackToTown()
    {
        
    }

   
    
    
    
    
    
    #endregion

}

