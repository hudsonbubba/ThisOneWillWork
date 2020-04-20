using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPredictor : MonoBehaviour
{
    public BoardCoords boardCoords;
    public StringVariable ReticuleShipString;

    public List<GameObject> cardList = new List<GameObject>();

    List<Vector2> playerPos = new List<Vector2>();
    List<Vector2> s1Pos = new List<Vector2>();
    List<Vector2> s2Pos = new List<Vector2>();
    List<Vector2> s3Pos = new List<Vector2>();
    List<Vector2> s4Pos = new List<Vector2>();
    List<Vector2> s5Pos = new List<Vector2>();
    List<Vector2> s6Pos = new List<Vector2>();
    List<Vector2> s7Pos = new List<Vector2>();
    List<Vector2> s8Pos = new List<Vector2>();
    List<Vector2> s9Pos = new List<Vector2>();
    List<Vector2> s10Pos = new List<Vector2>();
    List<Vector2> debugPos = new List<Vector2>();

    Dictionary<string, List<Vector2>> predictorDic = new Dictionary<string, List<Vector2>>();

    public const int rows = 5;
    public const int columns = 10;
    public GameObject[,] cardsObjs = new GameObject[rows, columns];

    public int[,] savedPos = new int[rows, columns];
    public string shipStringToDisplay;

    Ray ray;
    RaycastHit hit;

    LineRenderer lineRenderer;

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

        InitDict();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }

   
   void InitDict()
    {

        predictorDic.Add("p", playerPos);
        predictorDic.Add("s1", s1Pos);
        predictorDic.Add("s2", s2Pos);
        predictorDic.Add("s3", s3Pos);
        predictorDic.Add("s4", s4Pos);
        predictorDic.Add("s5", s5Pos);
        predictorDic.Add("s6", s6Pos);
        predictorDic.Add("s7", s7Pos);
        predictorDic.Add("s8", s8Pos);
        predictorDic.Add("s9", s9Pos);
        predictorDic.Add("s10", s10Pos);
        predictorDic.Add("debugPos", debugPos);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void e_displayLine()
    {
        List<Vector2> tempList = new List<Vector2>();
        shipStringToDisplay = ReticuleShipString.Value;
        if (predictorDic.TryGetValue(shipStringToDisplay, out tempList))
        {
            lineRenderer.positionCount = tempList.Count;
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 tempVector3 = new Vector3(tempList[i].y, tempList[i].x, 0f);
                lineRenderer.SetPosition(i, tempVector3);
            }
        }
        else
        {
            Debug.LogError("Couldn't find in Dictionary");
        }
        lineRenderer.enabled = true;
    }

    public void e_stopLine()
    {
        lineRenderer.enabled = false;
    }

    public void e_clearDictionary()
    {
        foreach (KeyValuePair<string, List<Vector2>> keyVal in predictorDic)
        {
            keyVal.Value.Clear();
        }
    }

    public void appendPos(string shipString, int fromRow, int fromCol, int targetRow, int targetCol) // To be called by the resolver
    {
        List<Vector2> temp = new List<Vector2>();
        if(predictorDic.TryGetValue(shipString, out temp))
        {
            if (temp.Count == 0)
            {
                Vector2 tempVector2From = new Vector2(fromRow * -1.73f, fromCol * 2.4f); //just how far apart the cards are 
                temp.Add(tempVector2From);
            }
            Vector2 tempVector2Target = new Vector2(targetRow * -1.73f, targetCol * 2.4f); //just how far apart the cards are 
            temp.Add(tempVector2Target);
        } else
        {
            Debug.LogError("Dictionary did not contain value for: " + shipString);
        }
  
    }

}
