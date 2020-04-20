using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BoardState officialBoardState;
    public int obstacleDifficulty;
    public int enemyDifficulty;
    public int targetDifficultyIncreasePerTurn;
    public int catchUpAmount;

    public IntegerVariable targetDifficulty;
    public AliveEnemyList aliveEnemyList;
    public AliveEnemyList deadEnemyList;

    public Ship playerShip;

    public GameEvent enemyDestroyedEvent;
    public GameEvent updateArtEvent;
    public GameEvent startTurnEvent;



    public void e_createNewBoardState()
    {
        int columnsToAdd = 0;
        if (playerShip.columnPosition > 3)
        {
            columnsToAdd = playerShip.columnPosition - 3;
            string[,] newBoard = shiftBoardState(columnsToAdd);
            shiftShips(columnsToAdd);

            int randomNewRow = Random.Range(1, newBoard.GetUpperBound(0) - 1);
            int randomNewColumn = Random.Range(newBoard.GetUpperBound(1) - (columnsToAdd - 1), newBoard.GetUpperBound(1));

            int numDeadShips = deadEnemyList.aliveList.Count;
            bool shipsAvailable = true;
            if (numDeadShips <= 0)
            {
                shipsAvailable = false;
            }

            newBoard[randomNewRow, randomNewColumn] = itemToSpawn(randomNewRow, randomNewColumn, shipsAvailable);

            officialBoardState.board = newBoard;
            updateArtEvent.Raise();
        }
        else
        {
            Debug.Log("Player is behind the desired column: not shifting board");
        }
        startTurnEvent.Raise();
    }

    string shipAssigner(int row, int column)
    {
        Ship shipToSpawn = deadEnemyList.aliveList[0];
        deadEnemyList.aliveList.RemoveAt(0);
        aliveEnemyList.aliveList.Add(shipToSpawn);

        shipToSpawn.rowPosition = row;
        shipToSpawn.columnPosition = column;
        shipToSpawn.action = "";
        shipToSpawn.isDead = false;

        return shipToSpawn.shipTypeString;
    }
    
    string itemToSpawn(int row, int column, bool shipsAvailable)
    {
        int difficulty = currentDifficulty();
        increaseTargetDifficulty();
        int target = targetDifficulty.Value;

        string spawnItem = "e";

        if (difficulty < target)
        {
            if (!shipsAvailable)
            {
                spawnItem = "o";
            }
            else if (difficulty + catchUpAmount < target)
            {
                spawnItem = shipAssigner(row, column);
            }
            else
            {
                int randomSpawn = Random.Range(0, 1);
                if (randomSpawn == 0)
                {
                    spawnItem = "o";
                }
                else
                {
                    spawnItem = shipAssigner(row, column);
                }
            }
        }
        return spawnItem;
    }
    
    int currentDifficulty()
    {
        int difficulty = 0;

        for (int row = 1; row < (officialBoardState.board.GetLength(0) - 1); row++)
        {
            for (int column = 0; column < officialBoardState.board.GetLength(1); column++)
            {
                string indexValue = officialBoardState.board[row, column];
                if (string.Equals(indexValue, "o"))
                {
                    difficulty += obstacleDifficulty;
                }
                else if (indexValue[0].Equals('s'))
                {
                    difficulty += enemyDifficulty;
                }
            }
        }

        return difficulty;
    }

    void increaseTargetDifficulty()
    {
        int currentTarget = targetDifficulty.Value;
        int newTarget = currentTarget + targetDifficultyIncreasePerTurn;
        targetDifficulty.SetValue(newTarget);
    }

    string[,] shiftBoardState(int numColumns)
    {
        string[,] newBoard = officialBoardState.board.Clone() as string[,];

        for (int j = numColumns; j < newBoard.GetLength(1) + numColumns; j++)
        {
            for (int i = 0; i < newBoard.GetLength(0); i++)
            {
                if (j >= newBoard.GetLength(1)) // New row
                {
                    if (i == 0 || i == (newBoard.GetLength(0) - 1))
                    {
                        newBoard[i, j - numColumns] = "x";
                    }
                    else
                    {
                        newBoard[i, j - numColumns] = "e";
                    }
                }
                else // Overwrite old row
                {
                    newBoard[i, j - numColumns] = newBoard[i, j];
                }
            }
        }
        return newBoard;
    }

    void shiftShips(int columnsShifted)
    {
        for (int i = aliveEnemyList.aliveList.Count - 1; i >= 0; i--)
        {
            Ship enemy = aliveEnemyList.aliveList[i];
            enemy.columnPosition -= columnsShifted;

            if (enemy.columnPosition < 0)
            {
                enemy.isDead = true;
                deadEnemyList.aliveList.Add(enemy);
                aliveEnemyList.aliveList.Remove(enemy);
                // enemyDestroyedEvent.Raise(); Shouldn't trigger energy gain
            }
        }

        playerShip.columnPosition -= columnsShifted;
    }
}
