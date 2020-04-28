using UnityEngine;

public class SnakeColliderController : MonoBehaviour
{

    #region Lifecycle
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Head"))
        {
            SnakeController.Die(transform.parent.gameObject);
        }
        else if (collider.CompareTag("PickUp"))
        {
            SnakeController.BonusLength(ReferenceContainer.SnakeController, collider.gameObject);
        }
    }
    #endregion

}