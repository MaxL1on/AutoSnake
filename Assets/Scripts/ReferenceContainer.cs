using UnityEngine;
#pragma warning disable 0649

public class ReferenceContainer : MonoBehaviour
{

    #region Properties
    public static UIManager UiManager { get; set; }
    public static GameManager GameManager { get; set; }
    public static SpawnController SpawnController { get; set; }
    public static CameraController CameraController { get; set; }
    public static SnakeController SnakeController { get; set; }
    #endregion


    #region Lifecycle
    void Awake()
    {
        UiManager = transform.GetComponent<UIManager>();
        SpawnController = transform.GetComponent<SpawnController>();
        CameraController = Camera.main.GetComponent<CameraController>();
    }
    #endregion

}
