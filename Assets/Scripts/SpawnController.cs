using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class SpawnController : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private float timeToNextBonusSpawn;
    [SerializeField]
    private GameObject BonusObject;
    [SerializeField]
    private GameObject Snake;
    [SerializeField]
    private GameObject SnakeTail;
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject EnemyTail;
    private bool canSpawnBonus;
    private List<GameObject> spawnedObjs;
    #endregion


    #region Properties
    public static float SecondsBetweenSpawn { get; set; }
    public static int EnemiesToSpawn { get; set; }
    #endregion


    #region Lifecycle
    void Start()
    {
        canSpawnBonus = false;
        spawnedObjs = new List<GameObject>();
        timeToNextBonusSpawn = SecondsBetweenSpawn;
    }

    private void Update()
    {
        if (canSpawnBonus)
        {
            timeToNextBonusSpawn -= Time.deltaTime;

            if (timeToNextBonusSpawn <= 0)
            {
                timeToNextBonusSpawn = SecondsBetweenSpawn;

                spawnedObjs.Add(RandomSpawn(BonusObject));
            }
        }
    }
    #endregion


    #region Public Methods
    public void StartSpawn()
    {
        spawnedObjs.Add(SpawnWithTail(Snake, SnakeTail, 1));
        spawnedObjs.AddRange(SpawnEnemies());

        spawnedObjs.Add(RandomSpawn(BonusObject));
        canSpawnBonus = true;
    }

    public void Respawn()
    {
        canSpawnBonus = false;
        for (int i = 0; i < spawnedObjs.Count; i++)
        {
            Destroy(spawnedObjs[i]);
        }
        spawnedObjs.Clear();

        StartSpawn();
    }
    #endregion


    #region Private Methods
    private GameObject[] SpawnEnemies()
    {
        GameObject[] enemies = new GameObject[EnemiesToSpawn];
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            enemies[i] = SpawnWithTail(Enemy, EnemyTail, 2);
        }
        return enemies;
    }

    private GameObject SpawnWithTail(GameObject headPrototype, GameObject tailPrototype, int tailsCount, bool isNeedToUpdateUI = false)
    {
        GameObject head = Instantiate(headPrototype);
        head.transform.position = LevelManager.RandomFieldPosition;
        ISnake headController = head.GetComponent<ISnake>();
        
        for (int i = 0; i < tailsCount; i++)
        {
            GameObject tail = Instantiate(tailPrototype);
            headController.AddTail(tail);
            if (isNeedToUpdateUI)
            {
                ReferenceContainer.UiManager.UpdateLengthText(headController.TailsLength + 1);
            }
        }

        return head;
    }

    private GameObject RandomSpawn(GameObject go)
    {
        GameObject gameObject = Instantiate(go);
        gameObject.transform.position = LevelManager.RandomFieldPosition;
        return gameObject;
    }
    #endregion

}
