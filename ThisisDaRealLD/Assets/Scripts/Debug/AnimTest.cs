using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    public int fromRow;
    public int fromCol;
    public int toRow;
    public int toCol;
    public string dir;
    public string shipString;
    public string targetString;
    public bool isDead;
    //int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString
    public void e_animTest()
    {
        GameObject.Find("CardArtManager").GetComponent<CardArtManager>().AnimateCard(fromRow, fromCol, toRow, toCol, dir, shipString, targetString, isDead);
    }

}
