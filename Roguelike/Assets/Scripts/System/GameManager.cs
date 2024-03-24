using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
