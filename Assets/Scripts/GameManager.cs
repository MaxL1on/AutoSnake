using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private float snakesSpeed;
    [SerializeField]
    private int fieldWidth;
    [SerializeField]
    private int fieldLength;
    [SerializeField]
    private float secondsBetweenSpawn;
    [SerializeField]
    private int enemiesToSpawn;
    #endregion


    #region Properties
    public float SnakesSpeed { get => snakesSpeed; set => snakesSpeed = value; }
    public int FieldWidth { get => fieldWidth; set => fieldWidth = value; }
    public int FieldLength { get => fieldLength; set => fieldLength = value; }
    public float SecondsBetweenSpawn { get => secondsBetweenSpawn; set => secondsBetweenSpawn = value; }
    public int EnemiesToSpawn { get => enemiesToSpawn; set => enemiesToSpawn = value; }
    #endregion


    #region Lyfecycle
    void Start()
    {
        SnakeMove.Speed = SnakesSpeed;

        LevelManager.FieldWidth = FieldWidth;
        LevelManager.FieldLength = FieldLength;

        SpawnController.SecondsBetweenSpawn = SecondsBetweenSpawn;
        SpawnController.EnemiesToSpawn = EnemiesToSpawn;
    }
    #endregion

}
