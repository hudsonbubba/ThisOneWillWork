using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PredictedPosition : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public const int rows = 5;
    public const int columns = 10;
    //public List<Vector3> savedPos = new List<Vector3>();
    Dictionary<string, List<Vector3>> predictorDic = new Dictionary<string, List<Vector3>>();
    public int[,] savedPos = new int[rows, columns];
    public string shipString;


    public void appendPos(string shipString, int target)
    {
        if(!predictorDic.ContainsKey(shipString)) // If the key does not exist, add it and append first value
        {

        }
    }
}
