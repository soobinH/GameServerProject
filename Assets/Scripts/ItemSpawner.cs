using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public Transform playerTransform;

    public float maxDistance = 5f;

    public float timeBetSpawnMax = 7f;
    public float timeBetSpawnMin = 2f;
    private float timeBetSpawn;

    private float lastSpawnTime;

    private void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0f;
    }

    private void Update()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            Spawn();
        }
    }
    private void Spawn()
    {
        Vector3 spawnPoint
        = Utility.GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        spawnPoint += Vector3.up * 0.5f;

        GameObject selectedItem = items[Random.Range(0, items.Length)];
        string itemName = selectedItem.name;
        GameObject item = PhotonNetwork.Instantiate(itemName, spawnPoint, Quaternion.identity);

        Destroy(item, 5f);
    }
}