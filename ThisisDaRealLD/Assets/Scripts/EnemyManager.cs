using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public BoardState officialBoardState;
    public AliveEnemyList aliveEnemyList;
    public Ship playerShip;

    public void e_chooseEnemyActions()
    {
        foreach (Ship enemy in aliveEnemyList.aliveList)
        {
            enemy.action = chooseAction(enemy);
        }
    }

    string chooseAction(Ship ship)
    {
        int shipRow = ship.rowPosition;
        int shipColumn = ship.columnPosition;
        int shipSpeed = ship.speed;

        int playerRow = playerShip.rowPosition;
        int playerColumn = playerShip.columnPosition;


        // Check for player to attack
        string playerAction = checkAttackPlayer(shipRow, shipColumn, playerRow, playerColumn);
        if (!string.IsNullOrEmpty(playerAction))
        {
            return playerAction;
        }

        // Check for obstacle to avoid
        string obstacleAction = checkObstacle(shipRow, shipColumn, shipSpeed);
        if (!string.IsNullOrEmpty(obstacleAction))
        {
            return obstacleAction;
        }

        // Default action is just to pass
        return "";
    }

    string checkAttackPlayer(int shipRow, int shipColumn, int playerRow, int playerColumn)
    {
        string chosenAction = "";

        if (shipColumn == playerColumn)
        {
            if (shipRow == (playerRow + 1)) // Below Player
            {
                chosenAction = "up";
            }
            else if (shipRow == (playerRow - 1)) // Above Player
            {
                chosenAction = "down";
            }
        }
        else if (shipRow == playerRow)
        {
            if (shipColumn > playerColumn)
            {
                chosenAction = "missileLeft";
            }
            else
            {
                chosenAction = "missileRight";
            }
        }

        return chosenAction;
    }

    string checkObstacle(int shipRow, int shipColumn, int shipSpeed)
    {
        string chosenAction = "";

        for (int i = 1; i <= shipSpeed; i++)
        {
            if (shipColumn + i <= officialBoardState.board.GetUpperBound(1))
            {
                string targetLocation = officialBoardState.board[shipRow, (shipColumn + i)];
                if (string.Equals(targetLocation, "o"))
                {
                    return avoidObstacleAction(shipRow);
                }
            }
        }

        return chosenAction;
    }

    string avoidObstacleAction(int shipRow)
    {
        int lowerBoundaryRow = BoardState.rows - 1;
        int upperBoundaryRow = 0;

        string chosenAction = "";

        if (shipRow == (lowerBoundaryRow - 1)) // Along bottom row, need to go up
        {
            chosenAction = "up";
        }
        else if (shipRow == (upperBoundaryRow + 1)) // Along upper row, need to go down
        {
            chosenAction = "down";
        }
        else
        {
            int randomDirection = Random.Range(0, 1);
            if (randomDirection == 0) chosenAction = "up";
            else chosenAction = "down";
        }
        return chosenAction;
    }

    /*int getRowPosition()
    {
        return enemyShipSO.rowPosition;
    }

    int getColumnPosition()
    {
        return enemyShipSO.columnPosition;
    }

    void declareAction(string actionName)
    {
        enemyShipSO.action = actionName;
    }*/
}
