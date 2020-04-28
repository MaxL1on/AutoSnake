using UnityEngine;
#pragma warning disable 0649

public class UIManager : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private GameObject snakeLengthText;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject playButton;
    private CameraController cameraController;
    private SpawnController spawnController;
    #endregion


    #region Lifecycle
    public void Start()
    {
        cameraController = ReferenceContainer.CameraController;
        spawnController = ReferenceContainer.SpawnController;
        Time.timeScale = 0;
        snakeLengthText.SetActive(false);
        restartButton.SetActive(false);
        playButton.SetActive(true);
    }
    #endregion


    #region Public Methods
    public void UpdateLengthText(int length)
    {
        snakeLengthText.GetComponent<TMPro.TextMeshProUGUI>().text = $"Snake length: {length}";
    }

    public void SetDieUI()
    {
        restartButton.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartButton_Click()
    {
        restartButton.SetActive(false);
        LevelManager.CreateField();
        cameraController.CorrectCameraPosition();
        spawnController.Respawn();
        Time.timeScale = 1;
    }

    public void PlayButton_Click()
    {
        playButton.SetActive(false);
        snakeLengthText.SetActive(true);
        LevelManager.CreateField();
        spawnController.StartSpawn();
        Time.timeScale = 1;
    }
    #endregion

}
