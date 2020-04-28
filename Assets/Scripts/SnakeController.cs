using System.Collections.Generic;
using UnityEngine;

public class SnakeController : SnakeMove, ISnake
{

    #region Fields
    private Vector3 addTailPossition;
    private UIManager uiManager;
    private List<GameObject> nearbyObj;
    #endregion


    #region Properties
    public List<GameObject> NearbyEnemies { get; set; }
    #endregion


    #region Lifecycle
    void Start()
    {
        nearbyObj = new List<GameObject>();
        NearbyEnemies = new List<GameObject>();
        uiManager = ReferenceContainer.UiManager;
        ReferenceContainer.SnakeController = this;
        Targets.AddFirst(transform.position);
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
        {
            NearbyEnemies.Add(collider.gameObject);
        }
        if (collider.CompareTag("PickUp"))
        {
            nearbyObj.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
        {
            NearbyEnemies.Remove(collider.gameObject);
        }
        if (collider.CompareTag("PickUp"))
        {
            nearbyObj.Remove(collider.gameObject);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < Tails.Count; i++)
        {
            Destroy(Tails[i]);
        }
        ReferenceContainer.SnakeController = null;
    }
    #endregion


    #region Public Methods
    public static void Die(GameObject gameObject)
    {
        Destroy(gameObject);
        ReferenceContainer.SnakeController.NearbyEnemies.ForEach(c => c.GetComponent<EnemyController>().NearbyEnemies.Remove(gameObject));
        ReferenceContainer.UiManager.SetDieUI();
    }

    public static void BonusLength(SnakeController gameObject, GameObject bonus)
    {
        bonus.transform.position = gameObject.addTailPossition;
        bonus.name = $"Tail {gameObject.Tails.Count + 1}";
        gameObject.Targets.AddLast(gameObject.Tails[gameObject.Tails.Count - 1].transform.position);
        gameObject.Tails.Add(bonus);
        gameObject.TailsLength++;
        gameObject.uiManager.SendMessage("UpdateLengthText", gameObject.TailsLength + 1);
    }

    public override void ChangeDirection()
    {
        Vector3 offset = Vector3.zero;
        bool isBonusNear = false;

        List<Vector3> possibleMovement = new List<Vector3>()
        {
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < possibleMovement.Count; i++)
        {
            bool isCorrect = CheckPossitionAccess(possibleMovement[i]);
            if (!isCorrect)
            {
                possibleMovement[i] = Vector3.zero;
            }
            if (nearbyObj.Find(p => p.transform.position == transform.position + possibleMovement[i]) != null)
            {
                isBonusNear = true;
                offset = possibleMovement[i];
            }
        }
        if (!isBonusNear)
        {
            possibleMovement.RemoveAll(item => item == Vector3.zero);
            if (possibleMovement.Count != 0)
            {
                offset = possibleMovement[Random.Range(0, possibleMovement.Count)];
            }
            else
            {
                SnakeController.Die(gameObject);
                return;
            }
        }

        Targets.AddFirst(transform.position + offset);
        if (Targets.Count == 2)
        {
            Targets.RemoveLast();
            LinkedListNode<Vector3> node = Targets.First;
            for (int i = 0; i < TailsLength; i++)
            {
                if (i == 0)
                {
                    Targets.AddAfter(node, transform.position);
                }
                else
                {
                    Targets.AddAfter(node, Tails[i - 1].transform.position);
                }
                node = node.Next;
            }
        }
        if (Targets.Count > TailsLength + 1)
        {
            addTailPossition = Targets.Last.Value;
            Targets.RemoveLast();
        }
    }
    #endregion


    #region Private Methods
    private bool CheckPossitionAccess(Vector3 offset)
    {
        //запрет на движение за пределы зоны
        if (!LevelManager.IsInFeldRange(transform.position + offset))
        {
            return false;
        }
        //запрет на движение к своим хвостам
        if (Targets.Count == TailsLength + 1)
        {
            LinkedListNode<Vector3> checkNode = Targets.First;
            for (int i = 0; i < Targets.Count - 1; i++)
            {
                checkNode = checkNode.Next;
                if (transform.position + offset == checkNode.Value)
                {
                    return false;
                }
            }
        }
        //checkEnemyPath
        List<Vector3> enemiesPath = new List<Vector3>();
        for (int i = 0; i < NearbyEnemies.Count; i++)
        {
            enemiesPath.AddRange(NearbyEnemies[i].GetComponent<EnemyController>().Targets);
        }
        for (int i = 0; i < enemiesPath.Count; i++)
        {
            if (transform.position + offset == enemiesPath[i])
            {
                return false;
            }
        }

        return true;
    }
    #endregion

}
