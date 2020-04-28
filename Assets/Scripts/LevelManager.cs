using UnityEngine;
#pragma warning disable 0649

public static class LevelManager
{

    #region Fields
    private static GameObject field;
    #endregion


    #region Properties
    public static float FieldWidthRange
    {
        get 
        { 
            return (float)FieldWidth / 2 - .5f; 
        }
    } 
    public static float FieldLengthRange
    {
        get
        {
            return (float)FieldLength / 2 - .5f; 
        }
    }
    public static Vector3 RandomFieldPosition
    {
        get
        {
            return new Vector3((FieldWidthRange * -1) + Random.Range(0, FieldWidth), 1, (FieldLengthRange * -1) + Random.Range(0, FieldLength));
        }
    }

    public static int FieldWidth { get; set; }
    public static int FieldLength { get; set; }
    #endregion


    #region Public Methods
    public static void CreateField(int width, int length)
    {
        field = GameObject.CreatePrimitive(PrimitiveType.Cube);
        field.name = "Field";
        field.transform.position = Vector3.zero;
        field.transform.localScale = new Vector3(width, 1, length);
    }
    public static void CreateField()
    {
        if(field != null)
        {
            UnityEngine.Object.Destroy(field);
        }
        CreateField(FieldWidth, FieldLength);
    }
    public static bool IsInFeldRange(Vector3 position)
    {
        if(Mathf.Abs(position.x) <= FieldWidthRange && Mathf.Abs(position.z) <= FieldLengthRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

}
