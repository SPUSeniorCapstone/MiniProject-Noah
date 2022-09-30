using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnInterval = 5;

    private float spawnTime = 0;
    public int enemyCap = 10;
    public float maxSpawnDistance = 30;
    public float minSpawnDistance = 15;

    public Enemy prefab;

    private GameController gc;

    private void Start()
    {
        if(prefab == null)
        {
            Debug.LogError("Missing Enemy Prefab");
        }
        gc = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (ReadyToSpawn())
        {
            var randomDir = Random.insideUnitCircle.normalized;
            Vector3 r = new Vector3(randomDir.x, 0, randomDir.y);
            float dist = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 spawnPos = gc.playerController.model.transform.position + (r * dist);
            spawnPos.y = 1;

            spawnTime = Time.time + SpawnInterval;

            Instantiate(prefab, spawnPos, prefab.transform.rotation, transform);
        }
    }

    private bool ReadyToSpawn()
    {
        if(Time.time > spawnTime)
        {
            if(FindObjectsOfType<Enemy>().Length < enemyCap)
            {
                return true;
            }
        }
        return false;
    }
}
