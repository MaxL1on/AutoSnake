using System.Collections.Generic;
using UnityEngine;

public interface ISnake
{

    #region Properties
    int TailsLength { get; set; }
    List<GameObject> Tails { get; set; }
    LinkedList<Vector3> Targets { get; set; }
    #endregion


    #region Methods
    void AddTail(GameObject tail);
    #endregion

}

