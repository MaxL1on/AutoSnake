using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region Lifecycle
    void Start()
    {
        CorrectCameraPosition();
    }
    #endregion


    #region Public Methods
    public void CorrectCameraPosition()
    {
        transform.position = new Vector3(0, 10, -8.5f);
        if(LevelManager.FieldLength - 12 > 0 && LevelManager.FieldLength - 12 > 0)
        {
            transform.position += new Vector3(0, (LevelManager.FieldWidth + LevelManager.FieldLength - 24) / 8, (LevelManager.FieldWidth + LevelManager.FieldLength - 24) / -2.5f);
        }
        else if(LevelManager.FieldLength - 12 > 0)
        {
            transform.position += new Vector3(0, (LevelManager.FieldLength - 12) / 8, (LevelManager.FieldLength - 12) / -2.5f);
        }
        else if(LevelManager.FieldWidth - 12 > 0)
        {
            transform.position += new Vector3(0, (LevelManager.FieldWidth - 12) / 8, (LevelManager.FieldWidth - 12) / -2.5f);
        }
    }
    #endregion

}
