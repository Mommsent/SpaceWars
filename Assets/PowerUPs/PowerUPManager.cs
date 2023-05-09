using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUPManager : MonoBehaviour
{
    public GameObject[] powerUpPrefab;
    public float minSpawnDelay = 5f;
    public float maxSpawnDelay = 10f;
    public float spawnXLimit = 12f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayBeforeSpawning());
    }

    void Spawn()
    {
        //Create the enemy in random position on X axis
        Vector2 random = GenerateSpawnPosition();
        int randomIndex = Random.Range(0, powerUpPrefab.Length);
        Instantiate(powerUpPrefab[randomIndex], random, powerUpPrefab[randomIndex].transform.rotation);

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
        yield return new WaitForSeconds(4f);
        Spawn();
    }
}
