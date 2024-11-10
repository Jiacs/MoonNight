using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject zombiePre;
    [SerializeField] private int zombieCountPerWave;
    [SerializeField] private float zombieSpawnInterval = 1;
    [SerializeField] private Player player;
    public static int zombieCount = 0;

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
    }
    public void StartGame()
    {
        PostEffectsManager.Instance.InitializePostEffects();
        gameStart = true;
        Cursor.SetCursor(customCursorTexture, new Vector2(customCursorTexture.width / 2, customCursorTexture.height / 2), CursorMode.ForceSoftware);
        StartCoroutine(GameLoop());
    }
    private IEnumerator GameLoop()
    {
        int waveCount = 0;
        yield return new WaitForSeconds(2f);
        while(!Player.isDead)
        {
            yield return SpawnZombies(zombieCountPerWave + Random.Range(0, waveCount), waveCount);
            yield return Wave();
            yield return ChooseReward();
        }
    }
    private IEnumerator Wave()
    {
        while(zombieCount > 0)
        {
            yield return null;
        }
    }
    private IEnumerator ChooseReward()
    {
        player.Enhance(Buff.BulletCapIncrease);
        player.Enhance(Buff.HPCountIncrease);
        player.Enhance(Buff.DamageIncrease);
        player.Enhance(Buff.RecoverAllHP);

        yield return null;
    }
    #region spawnZombie

    private void SpawnZombie(int zombieLevel)
    {
        int index = Random.Range(0, spawnPoints.Length);
        GameObject zombie = Instantiate(zombiePre, spawnPoints[index].position, spawnPoints[index].rotation);
        zombie.GetComponent<Enemy>().SetLevel(zombieLevel);
        zombieCount++;
    }
    private IEnumerator SpawnZombies(int spawnAmount, int zombieLevel)
    {
        int currentSpawnZombie = 0;
        while (currentSpawnZombie < spawnAmount)
        {
            SpawnZombie(zombieLevel);
            currentSpawnZombie++;
            yield return new WaitForSeconds(zombieSpawnInterval);
        }
    }
    #endregion
}
