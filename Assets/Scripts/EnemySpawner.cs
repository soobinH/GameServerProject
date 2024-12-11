using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public Transform playerPosition;
    public Enemy enemyPrefab;
    public Transform[] firstSpawnWavePoint;
    public Transform[] spawnPoints;
    public float damageMax = 40f;
    public float damageMin = 20f;

    public float healthMax = 100f;
    public float healthMin = 50f;

    public float speedMax = 3f;
    public float speedMin = 1f;

    public Color enemyColor = Color.red;
    private List<Enemy> enemies = new List<Enemy>();
    private int wave;
    private float gen_distance = 20.0f;

    private void Start()
    {
        for (int i = 0; i < firstSpawnWavePoint.Length; i++)
        {
            //좀비의 세기를 설정
            float enemyIntensity = Random.Range(0f, 1f);
            FirstCreateEnemy(enemyIntensity);
        }
        wave++;
    }

    private void Update()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        if (enemies.Count <= 0) SpawnWave();
    }

    public IEnumerator GenerateEnemy(Vector3 pos)
    {
        float enemyIntensity = Random.Range(0f, 1f);
        CreateEnemy(enemyIntensity);
        yield return new WaitForSeconds(5);//시간 조절을 하려면 매개변수 time값 조절
    }

    private void SpawnWave()
    {
        wave++;
        int spawnCount = Mathf.RoundToInt(wave * 2f);
        for (int i = 0; i < spawnCount; i++)
        {
            //좀비의 세기를 설정
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, enemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(health, damage, speed, skinColor);
        enemies.Add(enemy);

        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);
    }

    private void FirstCreateEnemy(float intensity)
    {
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, enemyColor, intensity);

        Transform spawnPoint = firstSpawnWavePoint[Random.Range(0, firstSpawnWavePoint.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(health, damage, speed, skinColor);
        enemies.Add(enemy);

        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);
    }

}