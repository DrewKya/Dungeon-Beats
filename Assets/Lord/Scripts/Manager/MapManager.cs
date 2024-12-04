using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemyContainer;

    [Header("Player settings")]
    [SerializeField] private List<Transform> spawnList = new List<Transform>();

    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabList = new List<GameObject>();
    private List<Enemy> activeEnemy = new List<Enemy>();
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
    public float despawnDistance = 25f;
    public int maxEnemy = 15;

    [Header("Loot Settings")]
    [SerializeField] public LootTable lootTable;


    private void Start()
    {
        if(player == null)
        {
            Debug.LogWarning("Player reference not set to MapManager!");
            this.enabled = false;
            return;
        }

        SetPlayerSpawn();
        lootTable.CalculateTotalWeight();

        StartCoroutine(SpawnEnemyCoroutine(3f));
        StartCoroutine(DespawnEnemyCoroutine(10f));
    }

    private void SetPlayerSpawn()
    {
        if(spawnList.Count > 0)
        {
            Transform spawnPos = spawnList[UnityEngine.Random.Range(0, spawnList.Count)];

            player.position = new Vector3(spawnPos.position.x, 0f, spawnPos.position.z);
        }
    }

    public void NotifyAllEnemyToTakeAction()
    {
        //Debug.Log(activeEnemy.Count);
        for(int i = activeEnemy.Count - 1 ; i > 0 ; i--)
        {
            var enemy = activeEnemy[i];
            if(enemy != null && enemy.isActiveAndEnabled)
            {
                enemy.TakeAction();
            }
        }
    }

    private IEnumerator SpawnEnemyCoroutine(float interval)
    {
        SpawnEnemy();
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnEnemyCoroutine(interval));
    }

    private void SpawnEnemy()
    {
        if (activeEnemy.Count >= maxEnemy) return;

        int attempts = 10;
        Vector3 spawnPosition = Vector3.zero;

        do
        {
            float distance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            //set enemy position in a fixed radius from the player
            spawnPosition = player.position + new Vector3(  Mathf.Round(Mathf.Cos(angle) * distance), 
                                                            0,
                                                            Mathf.Round(Mathf.Sin(angle) * distance)    );

            attempts--;

        } while (attempts > 0 && !CheckValidSpawnPosition(spawnPosition));

        if (CheckValidSpawnPosition(spawnPosition))
        {
            Debug.Log($"Spawning enemy at position : {spawnPosition}");

            GameObject enemyObject = Instantiate(GetEnemyFromList(), spawnPosition, Quaternion.identity, enemyContainer);
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            if (enemy != null) 
            {
                activeEnemy.Add(enemy);
            }
            else
            {
                Debug.LogWarning("Cannot find enemy component. Make sure enemyPrefabList only contain enemies");
            }
            
        }
    }

    private bool CheckValidSpawnPosition(Vector3 position)
    {
        RaycastHit hit;
        Vector3 checkPos = position + (Vector3.up * 2f);

        if(Physics.Raycast(checkPos, Vector3.down, out hit, 3f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetEnemyFromList()
    {
        int random = UnityEngine.Random.Range(0, enemyPrefabList.Count);
        return enemyPrefabList[random];
    }

    private IEnumerator DespawnEnemyCoroutine(float interval)
    {
        DespawnEnemies();
        yield return new WaitForSeconds(interval);
        StartCoroutine(DespawnEnemyCoroutine(interval));
    }

    private void DespawnEnemies()
    {
        float despawnDistanceSquared = despawnDistance * despawnDistance;

        for (int i = activeEnemy.Count - 1; i >= 0; i--)
        {
            Enemy enemy = activeEnemy[i];

            if (enemy == null)
            {
                activeEnemy.RemoveAt(i);
                continue;
            }

            float sqrDistance = (player.position - enemy.gameObject.transform.position).sqrMagnitude;
            if (sqrDistance > despawnDistanceSquared)
            {
                Destroy(enemy.gameObject);
                activeEnemy.RemoveAt(i);
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}