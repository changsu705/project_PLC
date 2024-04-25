using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool GameStoped => Time.timeScale == 0f;

    private PlayerController playerController;

    private float fadeDuration = 3f;
    public GameObject decoPortal;
    public GameObject cutWeeds;

    public GameObject portalEffect;
    public GameObject vine;
    public GameObject dust;

    public GameObject escButton;
    public GameObject backToGameButton;
    public GameObject onClickOptionButton;
    public GameObject backToTownButton;

    private bool isEsc;
    private bool allWavesCleared = false;   // 모든 웨이브가 클리어되었는지 여부

    // 각 고블린의 프리팹
    public GameObject swordGoblinPrefab;
    public GameObject bowGoblinPrefab;
    public GameObject shieldGoblinPrefab;
    public GameObject minoPrefab;

    /// <summary>
    /// 회귀 시계
    /// </summary>
    private Timer timer;
    /// <summary>
    /// 타이머 이미지
    /// </summary>
    private GameObject renderImage;

    // 웨이브에 필요한 정보를 가지고 있는 클래스
    [System.Serializable]
    public class Wave
    {
        public Transform[] swordGoblinSpawnPoints; // sword goblin을 스폰할 위치 배열
        public Transform[] bowGoblinSpawnPoints; // bow goblin을 스폰할 위치 배열
        public Transform[] shieldGoblinSpawnPoints; // shield goblin을 스폰할 위치 배열
        public Transform[] minoSpawnPoints;
    }

    public Wave[] waves; // 웨이브 배열
    private int currentWaveIndex = 0;

    [Header("Skill Select")]
    [SerializeField] private GameObject skillSelectCanvas;
    [SerializeField] private List<SkillObject> skillObjects;
    private TextMeshProUGUI[] skillNames;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;

        var obj = Instantiate(Resources.Load<GameObject>("Timer"), new Vector3(0f, -10f, 0f), Quaternion.identity);
        timer = obj.GetComponent<Timer>();

        renderImage = obj.transform.Find("RenderCanvas").gameObject;
        renderImage.SetActive(false);
    }

    private void Start()
    {
        if (decoPortal != null)
        {
            Invoke("PortalOff", 1f);
        }

        Time.timeScale = 1;
        escButton.SetActive(false);

        Action<GameObject> destructionEventHandler = HandleObjectDestruction;
        ObjectDestroyedEvent.OnObjectDestroyed += destructionEventHandler;

        if (SceneManager.GetActiveScene().buildIndex != 0 &&
            SceneManager.GetActiveScene().buildIndex != 1 &&
            SceneManager.GetActiveScene().buildIndex != 2)
        {
            StartWave(currentWaveIndex); 
        }

        if (GameObject.Find("Skill Colliders") == null)
        {
            Instantiate(Resources.Load("Skill Colliders"));
        }

        if (GameObject.Find("Skill Effects") == null)
        {
            Instantiate(Resources.Load("Skill Effects"));
        }

        playerController = FindObjectOfType<PlayerController>();
        skillNames = skillSelectCanvas.GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        onKeyESC();

        if (SceneManager.GetActiveScene().buildIndex != 0 &&
            SceneManager.GetActiveScene().buildIndex != 1 &&
            SceneManager.GetActiveScene().buildIndex != 2)
        {
            CheckEnemy();
        }
    }

    private void PortalOff()
    {
        decoPortal.transform.DOScale(0f, fadeDuration)
     .OnComplete(() => Destroy(decoPortal));
    }

    void HandleObjectDestruction(GameObject destroyedObject)
    {
        if (destroyedObject.CompareTag("Weeds"))
        {
            // 파괴된 오브젝트의 위치 저장
            Vector3 position = destroyedObject.transform.position;

            // 새로운 오브젝트 생성 및 위치 조정
            Instantiate(cutWeeds, position, Quaternion.identity);
        }
        if(destroyedObject.CompareTag("Totem"))
        {
            return;
        }
    }

    public static class ObjectDestroyedEvent
    {
        public static event Action<GameObject> OnObjectDestroyed;

        public static void InvokeObjectDestroyed(GameObject destroyedObject)
        {
            OnObjectDestroyed?.Invoke(destroyedObject);
        }
    }

    private void CheckEnemy()
    {
        GameObject[] noEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (noEnemy.Length == 0)
        {
            if (currentWaveIndex == waves.Length - 1 && !allWavesCleared)
            {
                // 마지막 웨이브가 클리어되면 portal과 vine 작동
                //portalEffect.SetActive(true);
                //dust.SetActive(true);
                //vine.transform.DOLocalMoveY(-8f, 4f);
                SetAllWavesCleared(); // 모든 웨이브가 클리어되었음을 설정
            }
            else
            {
                // 마지막 웨이브가 클리어되지 않았으면 다음 웨이브 시작
                currentWaveIndex++;
                if (currentWaveIndex < waves.Length)
                {
                    StartWave(currentWaveIndex);
                }
            }
        }
        else
        {
            //portalEffect.SetActive(false);
            //dust.SetActive(false);
            //vine.SetActive(true);
        }
    }

    private void onKeyESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isEsc)
        {
            Time.timeScale = 0;
            isEsc = true;
            escButton.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isEsc)
        {
            Time.timeScale = 1;
            escButton.SetActive(false);
            isEsc = false;
        }
    }

    // 웨이브 시작 메소드
    private void StartWave(int waveIndex)
    {
        Wave wave = waves[waveIndex];
        SpawnEnemies(wave);
    }

    // 고블린 스폰 메소드
    private void SpawnEnemies(Wave wave)
    {
        SpawnEnemy(swordGoblinPrefab, wave.swordGoblinSpawnPoints);
        SpawnEnemy(bowGoblinPrefab, wave.bowGoblinSpawnPoints);
        SpawnEnemy(shieldGoblinPrefab, wave.shieldGoblinSpawnPoints);
        SpawnEnemy(minoPrefab, wave.minoSpawnPoints);
    }

    // 고블린 스폰 메소드
    private void SpawnEnemy(GameObject enemyPrefab, Transform[] spawnPoints)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // 모든 웨이브가 클리어되었음을 설정하는 메소드
    public void SetAllWavesCleared()
    {
        allWavesCleared = true;

        ShuffleList(skillObjects);
        for (int idx = 0; idx < 3; idx++)
        {
            skillNames[idx].text = skillObjects[idx].name;
        }

        Time.timeScale = 0f;
        skillSelectCanvas.SetActive(true);
    }

    private void ShuffleList<T>(List<T> list)
    {
        int len = list.Count;
        for (int idx = 0; idx < len; idx++)
        {
            int randIdx = UnityEngine.Random.Range(0, len);
            (list[idx], list[randIdx]) = (list[randIdx], list[idx]);
        }
    }

    public void SelectSkill(int idx)
    {
        playerController.Skills.Add(skillObjects[idx]);
        skillObjects.RemoveAt(idx);
        skillSelectCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator noEnemy()
    {
        yield return null;
    }


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

    public void GameEnd()
    {
        //시계 등장
        timer.Playback();

        renderImage.SetActive(true);
    }
}

