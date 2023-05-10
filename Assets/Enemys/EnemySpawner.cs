using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private float minSpawnDelay = 0.5f;
    private float maxSpawnDelay = 3f;
    private float spawnXLimit = 6.3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayBeforeSpawning());
    }

    //Spawn the enemy
    public void Spawn()
    {
        //Create the enemy in random position on X axis
        Vector2 randomSpawnPos = GenerateSpawnPosition();
        int randomIndex = Random.Range(0, enemyPrefab.Length);
        Instantiate(enemyPrefab[randomIndex], randomSpawnPos, enemyPrefab[randomIndex].transform.rotation);

        Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    private Vector2 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnXLimit, spawnXLimit);
        Vector2 randomPos = new Vector2(spawnPosX, 10);
        return randomPos;
    }

    IEnumerator DelayBeforeSpawning()
    {
        yield return new WaitForSeconds(2f);
        Spawn();
    }
}
