using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private int score;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject zombiePre;
    [SerializeField] private int maxZombieCount;
    [SerializeField] private int zombieCountPerWave;
    [SerializeField] private float zombieSpawnInterval_perWave;
    private float spawnTimer;
    private int zombieCount_hasSpawned = 0;
    private float zombieSpawnInterval = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(zombieCount_hasSpawned >= maxZombieCount)
        { return; }
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= zombieSpawnInterval_perWave)
        {
            StartCoroutine(SpawnZombies(zombieCountPerWave));
            spawnTimer = 0;
        }
    }
    public void ChangeScore(int changeAmount)
    {
        score += changeAmount;
        text.text = score.ToString();
    }
    #region spawnZombie

    private void SpawnZombie()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePre, spawnPoints[index].position, spawnPoints[index].rotation);
        zombieCount_hasSpawned++;
    }
    private IEnumerator SpawnZombies(int spawnAmount)
    {
        int currentSpawnZombie = 0;
        while (currentSpawnZombie < spawnAmount)
        {
            SpawnZombie();
            currentSpawnZombie++;
            yield return new WaitForSeconds(zombieSpawnInterval);
        }
    }
    #endregion
}
