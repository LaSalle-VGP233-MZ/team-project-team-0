using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum WaveState
    {
        Active,
        Transitioning,
        CoolDown
    }

    [SerializeField] private GameObject zombiePool;
    [SerializeField] private int waveCooldownTime;
    [SerializeField] private int maxZombiesAtOnce;
    [SerializeField] private float spawnDelay = 3f;

    [SerializeField] private Transform zombieSpawnPoints;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI roundNumberText;
    [SerializeField] private TextMeshProUGUI zombieCounterText;
    [SerializeField] private TextMeshProUGUI recordText;

    private float waveCountdown = 0f;
    private int waveCount = 0;
    private int totalZombieWaveCount;
    private int zombiesKilledThisWave = 0;
    private int zombiesLeft;
    private WaveState waveState;
    private int numOfZombies;
    private float spawnDelayCounter;

    private int record;
    private string keyRecord;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        Health.OnZombieDied += UpdateZombieAmount;
    }
    private void OnDisable()
    {
        Health.OnZombieDied -= UpdateZombieAmount;
    }
    void Start()
    {
        LoadHighScore();
        recordText.text = "PR: " + record.ToString();
        waveState = WaveState.CoolDown;
        waveCountdown = waveCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        roundNumberText.text = "Round: " + waveCount.ToString();
        switch (waveState)
        {
            case WaveState.CoolDown:
                waveCountdown -= Time.deltaTime;
                countDownText.gameObject.SetActive(true);
                countDownText.text = ("Time until next wave: " + ((int)waveCountdown).ToString() + "s");
                if (waveCountdown <= 0)
                {
                    waveCount++;
                    waveCountdown = waveCooldownTime;
                    CalculateZombies();
                    countDownText.gameObject.SetActive(false);
                    waveState = WaveState.Active;
                }
                break;
            
            case WaveState.Active:
                zombieCounterText.gameObject.SetActive(true);
                zombiesLeft = totalZombieWaveCount - zombiesKilledThisWave;
                zombieCounterText.text = "Zombies Left: " + zombiesLeft.ToString() + "/" + totalZombieWaveCount;
                if (numOfZombies < maxZombiesAtOnce)
                {
                    if(zombiesLeft > 0 && spawnDelayCounter <= 0)
                    {
                        SpawnZombies(Mathf.Clamp((maxZombiesAtOnce - numOfZombies),1,5));
                        spawnDelayCounter = spawnDelay;
                    }
                    else if (zombiesLeft <= 0)
                    {
                        spawnDelayCounter = 0;
                        zombiesKilledThisWave = 0;
                        zombieCounterText.gameObject.SetActive(false);
                        waveState = WaveState.CoolDown;
                    }
                    spawnDelayCounter -= Time.deltaTime;
                }
                break;
        }

        RecordUpdate();
    }

    private void OnDestroy()
    {
        SaveRecord();
    }

    private void SpawnZombies(int zombieSpawnAmount) 
    {
        int index;
        Vector2 spawnPoint;
        for(int i =0; i < zombiePool.transform.childCount; i++)
        {
            if(zombieSpawnAmount <= 0 || numOfZombies + zombiesKilledThisWave >= totalZombieWaveCount)
            {
                return;
            }
            
            index = Random.Range(0, zombieSpawnPoints.transform.childCount);
            spawnPoint = zombieSpawnPoints.transform.GetChild(index).position;
            zombiePool.GetComponent<ObjPool>().GiveMeAnObjAt(spawnPoint);
            zombieSpawnAmount--;
            numOfZombies++;
        }
    }
    private void CalculateZombies()
    {
        float multiplier = waveCount * 0.15f;
        totalZombieWaveCount = (int)(multiplier * maxZombiesAtOnce);
    }
    private void UpdateZombieAmount()
    {
        numOfZombies--;
        zombiesKilledThisWave++;
    }

    private void RecordUpdate()
    {
        if (record < waveCount)
        {
            record = waveCount;
            recordText.text = "PR: " + record.ToString();
        }
    }

    private void SaveRecord()
    {
        PlayerPrefs.SetInt(keyRecord, record);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        if(PlayerPrefs.HasKey(keyRecord))
        {
            record = PlayerPrefs.GetInt(keyRecord);
            Debug.Log("Not found");
        }
        else
        {
            record = 0;
        }
    }
}
