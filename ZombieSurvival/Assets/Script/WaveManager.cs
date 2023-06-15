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
    [SerializeField] private Transform zombieSpawnPoints;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI roundNumberText;
    [SerializeField] private TextMeshProUGUI zombieCounterText;



    private float waveCountdown = 0f;
    private int waveCount = 1;
    private int totalZombieWaveCount;
    private int zombiesKilledThisWave = 0;
    private int zombiesLeft;
    private WaveState waveState;
    private int numOfZombies;


    // Start is called before the first frame update
    void Start()
    {
        Health.OnZombieDied += UpdateZombieAmount;
        waveState = WaveState.CoolDown;
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
                countDownText.text = ((int)waveCountdown).ToString();
                if (waveCountdown <= 0)
                {
                    waveCountdown = waveCooldownTime;
                    CalculateZombies();
                    countDownText.gameObject.SetActive(false);
                    waveState = WaveState.Active;
                }
                break;
            
            case WaveState.Active:
                zombieCounterText.gameObject.SetActive(true);
                zombiesLeft = totalZombieWaveCount - zombiesKilledThisWave;
                zombieCounterText.text = "Zombies: " + zombiesLeft.ToString() + "/" + totalZombieWaveCount;
                if (numOfZombies < maxZombiesAtOnce)
                {
                    int zombieSpawnAmount = totalZombieWaveCount - numOfZombies;
                    if(zombieSpawnAmount >= 0)
                    {
                        SpawnZombies(zombieSpawnAmount);
                    }
                    else if (numOfZombies <= 0)
                    {
                        waveCount++;
                        zombiesKilledThisWave = 0;
                        zombieCounterText.gameObject.SetActive(false);
                        waveState = WaveState.CoolDown;
                    }
                }
                break;
        }
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
        float multiplier = waveCount * 5f;
        totalZombieWaveCount = (int)(multiplier * waveCount);
        
    }
    private void UpdateZombieAmount()
    {
        numOfZombies--;
        zombiesKilledThisWave++;
    }
}
