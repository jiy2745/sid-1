using System.Collections;
using UnityEngine;

public class SlashMinigameSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // Prefab of slashable minigame objects
    public Transform[] spawnPoints;     // Points where minigame objects spawn

    [Tooltip("다음 오브젝트 생성까지의 최소 시간")]
    public float minSpawnDelay = 0.3f;
    [Tooltip("다음 오브젝트 생성까지의 최대 시간")]
    public float maxSpawnDelay = 1f;

    // Minimum and Maximum force applied to slashable objects
    public float minForce = 12f;
    public float maxForce = 18f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnSlashObject());
    }

    public IEnumerator SpawnSlashObject()
    {
        while (true)
        {
            // Wait for random delay before spawning next object
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // Choose a random spawn point and Instantiate a random prefab
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            GameObject slashObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation, transform.parent);

            // Shoot the newly created slashObject upwards
            Rigidbody2D rb = slashObject.GetComponent<Rigidbody2D>();
            rb.AddForce(spawnPoint.up * Random.Range(minForce, maxForce), ForceMode2D.Impulse);
        }
    }
}
