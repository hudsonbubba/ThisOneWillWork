using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDebugger : MonoBehaviour
{
    /*public int fromRow;
    public int fromCol;
    public int toRow;
    public int toCol;
    public string dir;
    public string shipString;
    public string targetString;
    public bool isDead;*/
    //int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString

    public BoardState officialBoardState;
    public Ship playerShip;

    public AliveEnemyList aliveEnemyList;
    public AliveEnemyList deadEnemyList;
    public AliveEnemyList allEnemyList;
    
    public CardCollection cardCollection;
    public IntegerVariable cardPlayed;
    public BoardCoords boardCoords;

    public int playerStartSpeed;
    public int enemyStartSpeed;
    public int startTargetDifficulty;
    public List<int> testDeck = new List<int>();
    public BoolVariable isFirstTurn;
    public IntegerVariable targetDifficulty;

    public GameEvent startOfTurnEvent;
    public GameEvent updateArt;

    void Awake()
    {
        isFirstTurn.SetValue(true);
        // Player Reset
        playerShip.action = "";
        playerShip.speed = playerStartSpeed;
        playerShip.isDead = false;

        // Enemy lists reset
        aliveEnemyList.aliveList.Clear();

        List<Ship> deadList = new List<Ship>(allEnemyList.aliveList);
        deadEnemyList.aliveList = deadList;

        foreach (Ship enemy in deadEnemyList.aliveList)
        {
            enemy.isDead = true;
        }

        Ship enemyShip = deadEnemyList.aliveList[0];
        deadEnemyList.aliveList.RemoveAt(0);
        aliveEnemyList.aliveList.Add(enemyShip);

        // Enemy Reset
        enemyShip.action = "";
        enemyShip.speed = enemyStartSpeed;
        enemyShip.isDead = false;

        // Card collection reset
        cardCollection.deck.Clear();
        cardCollection.deck = testDeck;
        cardCollection.drawPile.Clear();
        cardCollection.discardPile.Clear();
        cardCollection.hand.Clear();

        // Reset card played tracker
        cardPlayed.SetValue(-1);

        targetDifficulty.SetValue(startTargetDifficulty);

        // Grab positions of player and enemy from BoardState
        for (int row = 1; row < (officialBoardState.board.GetLength(0) - 1); row++)
        {
            for (int column = 0; column < officialBoardState.board.GetLength(1); column++)
            {
                string indexValue = officialBoardState.board[row, column];
                if (string.Equals(indexValue, "p"))
                {
                    playerShip.rowPosition = row;
                    playerShip.columnPosition = column;
                }
                else if (indexValue[0].Equals('s'))
                {
                    enemyShip.rowPosition = row;
                    enemyShip.columnPosition = column;
                }
            }
        }
    }

    void Start()
    {
        updateArt.Raise();
        startOfTurnEvent.Raise();
    }

    public void e_printBoardState()
    {
        string[,] matrix = officialBoardState.board;
        string result = "";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                result += matrix[i, j] + " ";
            }
            result += "\n";
        }
        Debug.Log(result);
    }

    public void e_printBoardCoords()
    {
        Vector3[,] matrix = boardCoords.board;
        string result = "";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                result += matrix[i, j] + " ";
            }
            result += "\n";
        }
        Debug.Log(result);
    }

    /*public void e_animTest()
    {
        GameObject.Find("CardArtManager").GetComponent<CardArtManager>().AnimateCard(fromRow, fromCol, toRow, toCol, dir, shipString, targetString, isDead);
    }*/

}
