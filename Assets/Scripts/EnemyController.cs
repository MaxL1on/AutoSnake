using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SnakeMove, ISnake
{

    #region Fields
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
        Targets.AddFirst(transform.position);
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PickUp"))
        {
            nearbyObj.Add(collider.gameObject);
        }
        if ((collider.CompareTag("Enemy") && collider.gameObject != gameObject) || collider.CompareTag("Snake"))
        {
            NearbyEnemies.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("PickUp"))
        {
            nearbyObj.Remove(collider.gameObject);
        }
        if ((collider.CompareTag("Enemy") && collider.gameObject != gameObject) || collider.CompareTag("Snake"))
        {
            NearbyEnemies.Remove(collider.gameObject);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < Tails.Count; i++)
        {
            Destroy(Tails[i]);
        }
    }
    #endregion


    #region Public Methods
    public override void ChangeDirection()
    {
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
        }
        possibleMovement.RemoveAll(item => item == Vector3.zero);

        Vector3 offset;
        if (possibleMovement.Count != 0)
        {
            offset = possibleMovement[Random.Range(0, possibleMovement.Count)];
        }
        else
        {
            return;
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
            if (NearbyEnemies[i].CompareTag("Snake"))
            {
                LinkedList<Vector3> snakeTargets = ReferenceContainer.SnakeController.Targets;
                Vector3[] snakePath = new Vector3[snakeTargets.Count];
                snakeTargets.CopyTo(snakePath,0);
                if(snakePath.Length != 0)
                {
                    snakePath[0] = snakePath[1];
                    enemiesPath.AddRange(snakePath);
                }
            }
            else
            {
                enemiesPath.AddRange(NearbyEnemies[i].GetComponent<EnemyController>().Targets);
            }
        }
        for (int i = 0; i < enemiesPath.Count; i++)
        {
            if (transform.position + offset == enemiesPath[i])
            {
                return false;
            }
        }

        bool flag = CheckNearby(offset);
        return flag;
    }

    private bool CheckNearby(Vector3 offset)
    {
        //запрет на движение к ближним объектам
        for (int i = 0; i < nearbyObj.Count; i++)
        {
            if (nearbyObj[i] == null)
            {
                nearbyObj.RemoveAt(i);
                CheckNearby(offset);
                break;
            }
            if (transform.position + offset == nearbyObj[i].transform.position)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

}
