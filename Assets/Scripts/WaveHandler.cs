using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    [Header("Waves")]
    public Wave[] waves;

    int currentWave = 0;
    float lastWaveTime = 0;
    float nextWaveTime = 1;
    public float projectileCooldownMin = .2f;
    public float projectileCooldownMax = 1.0f;
    public float waveCooldown = 5f;


    [Header("Projectiles")]
    public Projectile book;
    public Projectile brick;
    public Projectile cannonBall;



    void StartWave(int waveIndex)
    {
        void SpawnWave(Wave wave)
        {
            List<float> AssignCooldowns(int amount)
            {
                List<float> cooldownList = new List<float>();

                for (int i = 0; i < amount; i++)
                    cooldownList.Add(Random.Range(projectileCooldownMin, projectileCooldownMax));

                return cooldownList;
            }


            // Gets highest cooldown
            float highestCooldown = 0;
            List<List<float>> lists = new List<List<float>>();



            // Add more wave variables when added
            List<float> cooldownListBook = AssignCooldowns(wave.books);
            List<float> cooldownListBrick = AssignCooldowns(wave.bricks);
            List<float> cooldownListCannonBall = AssignCooldowns(wave.cannonBalls);

            StartCoroutine(SpawnProjectile(book, wave.books, cooldownListBook));
            StartCoroutine(SpawnProjectile(brick, wave.bricks, cooldownListBrick));
            StartCoroutine(SpawnProjectile(cannonBall, wave.cannonBalls, cooldownListCannonBall));


            lists.Add(cooldownListBook);
            lists.Add(cooldownListBrick);
            lists.Add(cooldownListCannonBall);



            foreach (List<float> list in lists)
            {
                float totalCooldown = 0;
                foreach (float cooldown in list)
                    totalCooldown += cooldown;

                if (totalCooldown > highestCooldown)
                    highestCooldown = totalCooldown;
            }

            // Sets next wave time using highest amount
            nextWaveTime = Time.timeSinceLevelLoad + highestCooldown + waveCooldown;
        }

        Wave wave = new Wave();

        if (waves.Length > waveIndex - 1) // Checks if wave is premade
            SpawnWave(waves[waveIndex - 1]);
        else
        {   // No wave configured!
            wave = waves[waves.Length - 1];



            // Add more wave variables when added
            wave.books = (int)(wave.books * (1 + ((float)waveIndex - (float)waves.Length) / 7));
            wave.bricks = (int)(wave.bricks * (1 + ((float)waveIndex - (float)waves.Length) / 7));
            wave.cannonBalls = (int)(wave.cannonBalls * (1 + ((float)waveIndex - (float)waves.Length) / 7));



            SpawnWave(wave);
        }
    }


    IEnumerator SpawnProjectile(Projectile projectile, int amount, List<float> cooldownList)
    {
        for (int i = 0; i < amount; i++)
        {
            Projectile newProjectile = Instantiate<Projectile>(projectile);
            newProjectile.startPosition = new Vector3(Random.Range(-3f, 3f), 1, 10);
            newProjectile.endPosition = new Vector3(Random.Range(-3f, 3f), 1, -4);
            yield return new WaitForSeconds(cooldownList[i]);
        }
    }


    void Update()
    {
        if (Time.timeSinceLevelLoad >= nextWaveTime && nextWaveTime > lastWaveTime) // Checks if should spawn wave
        {
            lastWaveTime = Time.timeSinceLevelLoad;
            currentWave++;
            StartWave(currentWave);
        }
    }
}
