using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        Scene nowScene = SceneManager.GetActiveScene();

        if (other.gameObject.CompareTag("Player"))   //오브젝트 태그가 Player일시 씬 불러오기
        {
            switch (nowScene.buildIndex)
            {
                case (0):
                    SceneManager.LoadScene(1);
                    break;

                case (1):
                    SceneManager.LoadScene(0);
                    break;
            }
        }

    }

}


