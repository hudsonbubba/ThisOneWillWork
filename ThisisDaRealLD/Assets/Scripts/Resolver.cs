using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolver : MonoBehaviour
{
    public BoardState officialBoardState;
    public BoardState telegraphedBoardState;
    
    public AliveEnemyList enemyShips;
    public Ship playerShip;

    // Events
    public GameEvent endOfTurnEvent;
    public GameEvent enemyDestroyedEvent;

    public int MAX_SPEED;

    public void e_previewTurn()
    {
        telegraphedBoardState.board = officialBoardState.board.Clone() as string[,];

        resetShip(playerShip);
        foreach (Ship enemy in enemyShips.aliveList)
        {
            resetShip(enemy);
        }

        // First resolve the player
        actionInterpreter(playerShip);

        // Next loop through the enemies and resolve
        foreach (Ship enemy in enemyShips.aliveList)
        {
            if (!enemy.isDead)
            {
                actionInterpreter(enemy);
            }
        }

        // Then the player moves forward according to speed
        for (int i = playerShip.speedTelegraph; i > 0; i--)
        {
            actionInterpreter(playerShip, "right");
        }

        // Then the enemies move forward according to speed
        foreach (Ship enemy in enemyShips.aliveList)
        {
            if (!enemy.isDead)
            {
                for (int i = enemy.speedTelegraph; i > 0; i--)
                {
                    actionInterpreter(enemy, "right");
                }
            }
        }
    }

    public void e_commitTurn()
    {
        officialBoardState.board = telegraphedBoardState.board.Clone() as string[,];

        // Update row/column positions of all ships
        updateShip(playerShip);
        foreach (Ship enemy in enemyShips.aliveList)
        {
            updateShip(enemy);
        }

        // check if player isDead
        if (playerShip.isDead)
        {
            Debug.Log("Game Over!");
        }

        // Remove enemies from the alive list if they are marked as isDead
        for (int i = enemyShips.aliveList.Count - 1; i >= 0; i--)
        {
            if (enemyShips.aliveList[i].isDead)
            {
                enemyShips.aliveList.RemoveAt(i);
                enemyDestroyedEvent.Raise();
            }
        }

        endOfTurnEvent.Raise();
    }

    void actionInterpreter(Ship ship, string optionalDirection = null)
    {
        string action;
        if (!(optionalDirection is null))
        {
            action = optionalDirection;
        }
        else
        {
            action = ship.action;
        }

        switch (action)
        {
            case "up":
                moveUp(ship);
                break;
            case "down":
                moveDown(ship);
                break;
            case "left":
                moveLeft(ship);
                break;
            case "right":
                moveRight(ship);
                break;
            case "accelerate":
                accelerate(ship);
                break;
            default:
                // Takes no action
                break;
        }
    }

    void moveUp(Ship ship)
    {
        int targetRow = ship.rowPositionTelegraph - 1;
        int targetColumn = ship.columnPositionTelegraph;
        moveToTarget(ship, targetRow, targetColumn, "up");

    }

    void moveDown(Ship ship)
    {
        int targetRow = ship.rowPositionTelegraph + 1;
        int targetColumn = ship.columnPositionTelegraph;
        moveToTarget(ship, targetRow, targetColumn, "down");
    }

    void moveLeft(Ship ship)
    {
        int targetRow = ship.rowPositionTelegraph;
        int targetColumn = ship.columnPositionTelegraph - 1;
        moveToTarget(ship, targetRow, targetColumn, "left");
    }

    void moveRight(Ship ship)
    {
        int targetRow = ship.rowPositionTelegraph;
        int targetColumn = ship.columnPositionTelegraph + 1;
        moveToTarget(ship, targetRow, targetColumn, "right");
    }

    void accelerate(Ship ship)
    {
        if (ship.speedTelegraph != MAX_SPEED)
        {
            ship.speedTelegraph++;
        }
        else
        {
            Debug.Log("Already at max speed!");
        }
    }

    void moveToTarget(Ship ship, int targetRow, int targetColumn, string direction)
    {
        int shipRow = ship.rowPositionTelegraph;
        int shipColumn = ship.columnPositionTelegraph;
        string shipString = ship.shipTypeString;

        

        if (targetRow < 0 || targetRow > 4 || targetColumn < 0 || targetColumn > 9)
        {
            Debug.Log("Trying to move out of bounds");
            if (string.Equals(shipString, "p")) // Player hits boundary
            {
                // You die, game over
                ship.isDead = true;
                telegraphedBoardState.board[shipRow, shipColumn] = "e";
            }
            else if (shipString[0].Equals('s')) // Enemy hits boundary
            {
                Debug.Log("Enemy goes out of bounds");
                ship.isDead = true;
                telegraphedBoardState.board[shipRow, shipColumn] = "e";
            }
        }
        else
        {
            string targetCurrentContents = telegraphedBoardState.board[targetRow, targetColumn];
            char targetFirstChar = targetCurrentContents[0];
            switch (targetFirstChar)
            {
                case 'e': // Empty spot
                    telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    ship.rowPositionTelegraph = targetRow;
                    ship.columnPositionTelegraph = targetColumn;
                    break;
                case 'x': // Boundary
                    if (string.Equals(shipString, "p")) // Player hits boundary
                    {
                        // You die, game over
                        ship.isDead = true;
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    }
                    else if (shipString[0].Equals('s')) // Enemy hits boundary
                    {
                        // destroyEnemy(ship);
                        ship.isDead = true;
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    }
                    break;
                case 'o': // Obstacle (meteor, etc.)
                    if (string.Equals(shipString, "p")) // Player hits obstacle
                    {
                        telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                        ship.rowPositionTelegraph = targetRow;
                        ship.columnPositionTelegraph = targetColumn;

                        takeDamage(1);
                    }
                    else if (shipString[0].Equals('s')) // Enemy hits obstacle
                    {
                        // destroyEnemy(ship);
                        ship.isDead = true;
                        telegraphedBoardState.board[targetRow, targetColumn] = "e";
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    }
                    break;
                case 'p': // Player Ship
                    actionInterpreter(playerShip, direction);
                    telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    ship.rowPositionTelegraph = targetRow;
                    ship.columnPositionTelegraph = targetColumn;
                    break;
                case 's': // Enemy Ship
                    Ship hitShip = findEnemyShipByTypeString(targetCurrentContents);
                    actionInterpreter(hitShip, direction);
                    telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    ship.rowPositionTelegraph = targetRow;
                    ship.columnPositionTelegraph = targetColumn;
                    break;
                default:
                    Debug.Log("Encountered unknown character in the board state");
                    break;
            }
        }
    }

    void takeDamage(int damageAmount)
    {
        Debug.Log("Damage taken!");
        playerShip.speedTelegraph -= damageAmount;

        if (playerShip.speedTelegraph <= 0)
        {
            playerShip.isDead = true;
            telegraphedBoardState.board[playerShip.rowPosition, playerShip.columnPosition] = "e";
        }
    }

    void updateShip(Ship ship)
    {
        ship.rowPosition = ship.rowPositionTelegraph;
        ship.columnPosition = ship.columnPositionTelegraph;
        ship.speed = ship.speedTelegraph;
    }

    void resetShip(Ship ship)
    {
        ship.rowPositionTelegraph = ship.rowPosition;
        ship.columnPositionTelegraph = ship.columnPosition;
        ship.speedTelegraph = ship.speed;
        ship.isDead = false;
    }

    Ship findEnemyShipByTypeString(string targetString)
    {
        Ship targetShip = null;
        foreach (Ship ship in enemyShips.aliveList)
        {
            if (string.Equals(ship.shipTypeString, targetString))
            {
                targetShip = ship;
                break;
            }
        }

        if (targetShip is null)
        {
            Debug.Log("Ship in board state is not found in the alive array");
        }

        return targetShip;
    }
}
