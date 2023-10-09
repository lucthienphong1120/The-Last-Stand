using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private SliderControl expBarControl;
    private SpawnManager spawnManager;
    private ShopManager shopManager;
    private PlayerStatus playerStatus;
    public Slider expBar;
    public Text levelText;
    public Text waveText;
    public Text timeText;
    public GameObject pausePanel;
    public GameObject itemsPanel;
    public GameObject weaponPanel;
    public GameObject endPanel;
    private AudioSource audioSrc;
    public GameObject Backgrounds;
    public int playerHealthAtEnd = 0;
    public int currentWave; // decide to open shop
    private int currentExp; // get money only
    private int maxExp = 20;
    private int currentLevel; // get momey only
    private float currentTime;
    private int maxTime = 20;
    private int[] maxTimeLevel = new int[] { 20, 30, 40, 50, 60 };
    private int[] maxExpLevel = new int[] { 20, 25, 30, 40, 50, 70, 100 };
    private bool isMusic;
    private string mapName;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            isMusic = DataManager.Instance.GetMusic();
            mapName = DataManager.Instance.GetMap();
        }
        else
        {
            Debug.LogError("DataManager not found an instance!");
        }
        InitData();
    }

    void InitData()
    {
        expBarControl = expBar.GetComponent<SliderControl>();
        spawnManager = GetComponent<SpawnManager>();
        shopManager = GetComponent<ShopManager>();
        audioSrc = GetComponent<AudioSource>();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        expBarControl.UpdateBar(currentExp, maxExp);
        currentWave = 0;
        currentLevel = 0;
        SetupTimeNextLevel();
        levelText.text = "Level: " + currentLevel;
        if (isMusic)
        {
            audioSrc.Play();
        }
        GameObject map = Backgrounds.transform.Find(mapName).gameObject;
        if (map != null)
        {
            map.SetActive(true);
        }
        OpenShop();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0) // when end a wave
        {
            currentWave++;
            waveText.text = "Wave: " + currentWave;
            SetupTimeNextLevel();
            playerStatus.Health(playerHealthAtEnd);
            spawnManager.AddMoreSpawn();
            OpenShop();
        }
        else
        {
            currentTime -= Time.deltaTime;
            SetTimer(currentTime);
        }
    }

    void SetupTimeNextLevel()
    {
        maxTime = maxTimeLevel[currentWave];
        currentTime = maxTime;
        SetTimer(currentTime);
        spawnManager.ClearSpawn();
    }

    void SetTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GetExp(int exp)
    {
        currentExp += exp;
        shopManager.GetMoney(exp); // get exp and money too
        if (currentExp >= maxExpLevel[currentLevel])
        {
            shopManager.GetMoney(maxExpLevel[currentLevel]); // bonus money equal max current exp
            currentLevel++;
            currentExp = 0;
        }
        expBarControl.UpdateBar(currentExp, maxExpLevel[currentLevel]);
        levelText.text = "Level: " + currentLevel;
    }

    void OpenShop()
    {
        Time.timeScale = 0;
        if (currentWave % 2 == 0)
        {
            weaponPanel.SetActive(true);
            itemsPanel.SetActive(false);
        }
        else
        {
            itemsPanel.SetActive(true);
            weaponPanel.SetActive(false);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        endPanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        itemsPanel.SetActive(false);
        weaponPanel.SetActive(false);
    }

    public void Restart()
    {
        //Time.timeScale = 0;
        SceneManager.LoadScene("GamePlay");
    }
    public void Home()
    {
        //Time.timeScale = 0;
        SceneManager.LoadScene("StartMenu");
    }
}
