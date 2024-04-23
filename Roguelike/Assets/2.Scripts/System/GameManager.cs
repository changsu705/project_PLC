using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject portalEffect;
    public GameObject vine;
    public GameObject dust;

    public GameObject escButton;
    public GameObject backToGameButton;
    public GameObject onClickOptionButton;
    public GameObject backToTownButton;

    private bool isEsc;
    private bool allWavesCleared = false; // 모든 웨이브가 클리어되었는지 여부

    // 각 고블린의 프리팹
    public GameObject swordGoblinPrefab;
    public GameObject bowGoblinPrefab;
    public GameObject shieldGoblinPrefab;

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
    }

    public Wave[] waves; // 웨이브 배열
    private int currentWaveIndex = 0;

    private void Awake()
    {
        Instance = this;

        timer = GetComponentInChildren<Timer>();        //회귀 시계
        if (timer == null)
        {
            timer = Instantiate(
                Resources.Load<GameObject>("Timer"),    //object
                new Vector3(0f, -10f, 0f),              //position
                Quaternion.identity,                    //rotation
                transform)                              //parent; this object
                .GetComponentInChildren<Timer>();
        }

        renderImage = GameObject.Find("Reset Timer Image");
        renderImage.SetActive(false);
    }

    private void Start()
    {
        Time.timeScale = 1;
        escButton.SetActive(false);

        StartWave(currentWaveIndex); // 시작할 웨이브 시작
    }

    private void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        onKeyESC();

        if (SceneManager.GetActiveScene().buildIndex != 0 &&
            SceneManager.GetActiveScene().buildIndex != 1 &&
            SceneManager.GetActiveScene().buildIndex != 2)
        {
            if (currentSceneName == "HouseScene" && currentSceneName == "Village")
            {
                return;
            }
            CheckEnemy();
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
                portalEffect.SetActive(true);
                dust.SetActive(true);
                vine.transform.DOLocalMoveY(-8f, 4f);
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
            /*portalEffect.SetActive(false);
            dust.SetActive(false);
            vine.SetActive(true);*/
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
        SpawnGoblins(wave);
    }

    // 고블린 스폰 메소드
    private void SpawnGoblins(Wave wave)
    {
        SpawnGoblin(swordGoblinPrefab, wave.swordGoblinSpawnPoints);
        SpawnGoblin(bowGoblinPrefab, wave.bowGoblinSpawnPoints);
        SpawnGoblin(shieldGoblinPrefab, wave.shieldGoblinSpawnPoints);
    }

    // 고블린 스폰 메소드
    private void SpawnGoblin(GameObject goblinPrefab, Transform[] spawnPoints)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(goblinPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // 모든 웨이브가 클리어되었음을 설정하는 메소드
    public void SetAllWavesCleared()
    {
        allWavesCleared = true;
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

