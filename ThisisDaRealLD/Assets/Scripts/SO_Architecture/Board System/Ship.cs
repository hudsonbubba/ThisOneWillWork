﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ship : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    // Describes where in the board state 2D array the ship is
    public int rowPosition;
    public int columnPosition;

    // Describes the action that this ship is planning to take
    public string action;

}
