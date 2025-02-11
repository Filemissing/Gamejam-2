using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Statistics")]
    public int score;
    public int headSize;
    public float playerSpeed;
    public float timeSpeed;


    [Header("Waves")]
    public Wave[] waves;

    int currentWave = 0;
    float lastWaveTime = 0;
    float nextWaveTime = 1;
    float projectileCooldown = .8f;
    float waveCooldown = 5f;


    [Header("Projectiles")]
    public Projectile book;



    void SpawnWave(int waveIndex)
    {
        Wave wave = new Wave();
        bool waveAssigned = false;

        if (waves.Length > waveIndex - 1) // Checks if wave is premade
        {
            wave = waves[waveIndex - 1];
            waveAssigned = true;
        }

        if (waveAssigned)
        {
            StartCoroutine(SpawnProjectile(book, wave.books));

            // Gets highest amount
            int highestAmount = 0;
            List<int> list = new List<int>();
            list.Add(wave.books); // Add more wave variables when added

            foreach (int amount in list)
            {
                if (amount > highestAmount)
                    highestAmount = amount;
            }

            // Sets next wave time using highest amount
            nextWaveTime = Time.timeSinceLevelLoad + highestAmount * projectileCooldown + waveCooldown;
        }
        else
        {   // some weird algorithm
            Debug.LogWarning("No waves found!");
        }
    }


    IEnumerator SpawnProjectile(Projectile projectile, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Projectile newProjectile = Instantiate<Projectile>(projectile);
            newProjectile.startPosition = new Vector3(Random.Range(-3f, 3f), 1, 10);
            newProjectile.endPosition = new Vector3(Random.Range(-3f, 3f), 1, -4);
            yield return new WaitForSeconds(projectileCooldown);
        }
    }


    void Update()
    {
        if (instance == null) instance = this;

        if (Time.timeSinceLevelLoad >= nextWaveTime && nextWaveTime > lastWaveTime) // Checks if should spawn wave
        {
            lastWaveTime = Time.timeSinceLevelLoad;
            currentWave++;
            SpawnWave(currentWave);
        }
    }
}
