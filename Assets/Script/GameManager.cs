using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject zombiePre;
    [SerializeField] private int maxZombieCount;
    [SerializeField] private int zombieCountPerWave;
    [SerializeField] private float zombieSpawnIntervalPerWave;
    private float spawnTimer;
    private int zombieCount_hasSpawned = 0;
    private float zombieSpawnInterval = 1;

    private static bool gameStart = false;
    public static bool GameStarted => gameStart;

    private Texture2D customCursorTexture;

    private void Start()
    {
        customCursorTexture = Resources.Load<Texture2D>("Art/Shot");
    }
    void Update()
    {
        if (!gameStart)
        {
            return;
        }
        PostEffectsManager.Instance.RemoveRadialBlur(0.2f);
        if(zombieCount_hasSpawned >= maxZombieCount)
        { return; }
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= zombieSpawnIntervalPerWave)
        {
            StartCoroutine(SpawnZombies(zombieCountPerWave));
            spawnTimer = 0;
        }
    }
    #region spawnZombie

    public void StartGame()
    {
        PostEffectsManager.Instance.InitializePostEffects();
        gameStart = true;
        Cursor.SetCursor(customCursorTexture, new Vector2(customCursorTexture.width/2, customCursorTexture.height/2), CursorMode.ForceSoftware);
    }
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
