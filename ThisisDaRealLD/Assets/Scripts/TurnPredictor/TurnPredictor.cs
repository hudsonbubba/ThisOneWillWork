using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPredictor : MonoBehaviour
{
    public BoardCoords boardCoords;

    public List<GameObject> cardList = new List<GameObject>();

    public List<Vector3> playerPos = new List<Vector3>();
    public List<Vector3> s1Pos = new List<Vector3>();
    public List<Vector3> s2Pos = new List<Vector3>();
    public List<Vector3> s3Pos = new List<Vector3>();
    public List<Vector3> s4Pos = new List<Vector3>();
    public List<Vector3> s5Pos = new List<Vector3>();
    public List<Vector3> s6Pos = new List<Vector3>();
    public List<Vector3> s7Pos = new List<Vector3>();
    public List<Vector3> s8Pos = new List<Vector3>();
    public List<Vector3> s9Pos = new List<Vector3>();
    public List<Vector3> s10Pos = new List<Vector3>();

    public const int rows = 5;
    public const int columns = 10;
    public GameObject[,] cardsObjs = new GameObject[rows, columns];

    public int[,] savedPos = new int[rows, columns];
    public string shipString;

    //public List<Vector3> savedPos1 = new List<Vector3>();


    void Start()
    {
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            cardList.Add(card);
        }

        int z = 0;
        for (int i = 0; i < cardsObjs.GetLength(0); i++)
        {
            for (int j = 0; j < cardsObjs.GetLength(1); j++)
            {
                cardsObjs[i, j] = cardList[z];
                z++;
            }
        }

        for (int i = 0; i < cardsObjs.GetLength(0); i++)
        {
            for (int j = 0; j < cardsObjs.GetLength(1); j++)
            {

                //cards[i, j] = cardList[z];
                boardCoords.board[i, j] = cardsObjs[i, j].GetComponent<Transform>().position;
            }
        }
        /*
        Dictionary<string, List<Vector3>> predictorDic = new Dictionary<string, List<Vector3>>()
        {
            {"s1",  }
            //INITILAIZE DIC HERE
        }
        */
    }

   
   



    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void appendPos(string shipString, int target)
    {
        if (!predictorDic.ContainsKey(shipString)) // If the key does not exist, add it and append first value
        {
            //predictorDic.Add(shipString, new List)

        }
  
    }
    */
}
