using System.Collections.Generic;
using UnityEngine;

public abstract class SnakeMove : MonoBehaviour
{

    #region Properties
    public static float Speed { get; set; }
    public int TailsLength { get; set; }
    public List<GameObject> Tails { get; set; }
    public LinkedList<Vector3> Targets { get; set; }
    #endregion


    #region Lifecycle
    protected void Awake()
    {
        Tails = new List<GameObject>();
        Targets = new LinkedList<Vector3>();
        TailsLength = 0;
    }
    #endregion


    #region Pulic Methods
    public abstract void ChangeDirection();

    public void Move()
    {
        if (transform.position == Targets.First.Value)
        {
            ChangeDirection();
        }
        transform.position = Vector3.MoveTowards(transform.position, Targets.First.Value, Time.deltaTime * Speed);

        for (int i = 0; i < TailsLength; i++)
        {
            LinkedListNode<Vector3> node = Targets.First;
            for (int j = 0; j <= i; j++)
            {
                node = node.Next;
            }
            Tails[i].transform.position = Vector3.MoveTowards(Tails[i].transform.position, node.Value, Time.deltaTime * Speed);
        }
    }

    public void AddTail(GameObject tail)
    {
        if (Tails.Count == 0)
        {
            tail.transform.position = transform.position + Vector3.left;
            tail.name = "Tail 1";
        }
        else
        {
            tail.transform.position = Tails[Tails.Count - 1].transform.position + Vector3.left;
            tail.name = $"Tail {Tails.Count + 1}";
        }
        Tails.Add(tail);
        TailsLength++;
    }
    #endregion

}
