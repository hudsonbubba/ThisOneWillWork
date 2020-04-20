using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardState : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public const int rows = 5;
    public const int columns = 10;
    public string[,] board = new string[rows, columns]
    {
        { "x", "x", "x", "x", "x", "x", "x", "x", "x", "x" },
        { "e", "e", "e", "e", "e", "e", "e", "e", "e", "e" },
        { "e", "e", "e", "p", "e", "e", "o", "e", "e", "e" },
        { "e", "e", "s1", "e", "e", "e", "e", "e", "e", "e" },
        { "x", "x", "x", "x", "x", "x", "x", "x", "x", "x" },
    };
}
