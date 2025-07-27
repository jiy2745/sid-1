using System.Collections;
using UnityEngine;

public class DodgeMinigameSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // Prefab of slashable minigame objects
    public Transform[] spawnPoints;     // Points where minigame objects spawn

    [Tooltip("다음 오브젝트 생성까지의 최소 시간")]
    public float minSpawnDelay = 0.5f;
    [Tooltip("다음 오브젝트 생성까지의 최대 시간")]
    public float maxSpawnDelay = 1f;

    // Minimum and maximum speed of each object
    public float minSpeed = 10f;
    public float maxSpeed = 15f;

    // Minimum and maximum lifetime of each object
    public float minLifetime = 2f;
    public float maxLifetime = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public IEnumerator SpawnDodgeObject()
    {
        while (true)
        {
            // Wait for random delay before spawning next object
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // Choose a random spawn point and Instantiate a random prefab
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            GameObject dodgeObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation, transform.parent);

            // Set speed, direction and lifetime of created prefab instance
            DodgeMinigameObject obj = dodgeObject.GetComponent<DodgeMinigameObject>();
            obj.moveSpeed = Random.Range(minSpeed, maxSpeed);
            obj.moveDirection = Vector3.left;
            obj.lifetime = Random.Range(minLifetime, maxLifetime);
        }
    }
}
