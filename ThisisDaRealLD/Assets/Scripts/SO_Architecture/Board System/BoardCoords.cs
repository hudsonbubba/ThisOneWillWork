using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardCoords : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public const int rows = 5;
    public const int columns = 10;
    public Vector3[,] board = new Vector3[rows, columns];
}
