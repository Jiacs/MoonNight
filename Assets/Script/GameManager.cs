using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject zombiePre;
    [SerializeField] private int zombieCountPerWave;
    [SerializeField] private float zombieSpawnInterval = 1;
    [SerializeField] private Player player;
    [SerializeField] private Transform[] rewardBlockTransform;
    [SerializeField] private GameObject rewardBlockPref;
    [SerializeField] private GameObject bossPre;
    [SerializeField] private Transform bossSpawnPoint;
    public static int zombieCount = 0;

    private static bool gameStart = false;
    public static bool GameStarted => gameStart;
    public static bool chooseRewardPhase = false;

    private Texture2D customCursorTexture;

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log(this.gameObject.name);
            Debug.LogError("More Than One Instance of Singleton!");
        }
    }
    #endregion
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
            if (waveCount % 10 != 0)
            {
                yield return SpawnZombies(zombieCountPerWave + UnityEngine.Random.Range(0, waveCount), waveCount);
                yield return Wave();
                yield return new WaitForSeconds(2f);
                yield return ChooseReward();
            }
            else
            {
                yield return SpawnBoss(waveCount);
                yield return BossFight();
                yield return new WaitForSeconds(2f);
                yield return ChooseRewardBoss();
            }
            waveCount++;
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
        chooseRewardPhase = true;
        List<Buff> buffList = BuffSelector.GetRandomBuffs(3);
        List<GameObject> rewardBlocks = new List<GameObject>();
        for(int i = 0; i < 3; i++)
        {
            rewardBlocks.Add(Instantiate(rewardBlockPref, rewardBlockTransform[i]));
            rewardBlocks[i].GetComponent<OptionBlock>().SetBuff(buffList[i]);
        }
        while (chooseRewardPhase)
        {
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            Destroy(rewardBlocks[i]);
        }
        yield return null;
    }
    private IEnumerator ChooseRewardBoss()
    {
        chooseRewardPhase = true;
        List<Buff> buffList = BuffSelector.GetRandomBuffs(3);
        List<GameObject> rewardBlocks = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            rewardBlocks.Add(Instantiate(rewardBlockPref, rewardBlockTransform[i]));
            rewardBlocks[i].GetComponent<OptionBlock>().SetBuff(buffList[i]);
        }
        while (chooseRewardPhase)
        {
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            Destroy(rewardBlocks[i]);
        }
        yield return null;
    }
    public void EnhancePlayer(Buff buff)
    {
        player.Enhance(buff);
    }
    #region spawnZombie

    private void SpawnZombie(int zombieLevel)
    {
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
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
    public bool BossAlive = false;
    private IEnumerator SpawnBoss(int bossLevel)
    {
        GameObject boss = Instantiate(bossPre, bossSpawnPoint.position, bossSpawnPoint.rotation);
        boss.GetComponent<Enemy>().SetLevel(bossLevel);
        BossAlive = true;
        yield return null;
    }
    private IEnumerator BossFight()
    {
        while(BossAlive && !Player.isDead)
        {
            yield return null;
        }
    }
    #endregion
}
