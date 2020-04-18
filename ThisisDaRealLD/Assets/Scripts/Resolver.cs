using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolver : MonoBehaviour
{
    public BoardState officialBoardState;
    public BoardState telegraphedBoardState;
    
    public AliveEnemyList enemyShips;
    public Ship playerShip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void e_previewTurn()
    {
        telegraphedBoardState.board = officialBoardState.board.Clone() as string[,];

        // First resolve the player
        actionInterpreter(playerShip);

        // Next loop through the enemies and resolve
        foreach (Ship enemy in enemyShips.aliveList)
        {
            enemy.isDead = false;
            actionInterpreter(enemy);
        }

        commitTurn();
    }

    void commitTurn()
    {
        officialBoardState.board = telegraphedBoardState.board.Clone() as string[,];

        // Remove enemies from the alive list if they are marked as isDead
        for (int i = enemyShips.aliveList.Count - 1; i >= 0; i--)
        {
            if (enemyShips.aliveList[i].isDead)
            {
                enemyShips.aliveList.RemoveAt(i);
            }
        }

        // check if player isDead
        if (playerShip.isDead)
        {
            Debug.Log("Game Over!");
        }

        // Update row/column positions of all ships
        updatePosition(playerShip);
        foreach (Ship enemy in enemyShips.aliveList)
        {
            updatePosition(enemy);
        }
    }

    void updatePosition(Ship ship)
    {
        ship.rowPosition = ship.rowPositionTelegraph;
        ship.columnPosition = ship.columnPositionTelegraph;
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
            default:
                // Takes no action
                break;
        }
    }

    void moveUp(Ship ship)
    {
        int targetRow = ship.rowPosition - 1;
        int targetColumn = ship.columnPosition;
        moveToTarget(ship, targetRow, targetColumn, "up");

    }

    void moveDown(Ship ship)
    {
        int targetRow = ship.rowPosition + 1;
        int targetColumn = ship.columnPosition;
        moveToTarget(ship, targetRow, targetColumn, "down");
    }

    void moveLeft(Ship ship)
    {
        int targetRow = ship.rowPosition;
        int targetColumn = ship.columnPosition - 1;
        moveToTarget(ship, targetRow, targetColumn, "left");
    }

    void moveRight(Ship ship)
    {
        int targetRow = ship.rowPosition;
        int targetColumn = ship.columnPosition + 1;
        moveToTarget(ship, targetRow, targetColumn, "right");
    }

    void moveToTarget(Ship ship, int targetRow, int targetColumn, string direction)
    {
        int shipRow = ship.rowPosition;
        int shipColumn = ship.columnPosition;
        string shipString = ship.shipTypeString;

        string targetCurrentContents = telegraphedBoardState.board[targetRow, targetColumn];
        char targetFirstChar = targetCurrentContents[0];

        if (targetRow < 0 || targetRow > 4 || targetColumn < 0 || targetColumn > 9)
        {
            Debug.Log("Trying to move out of bounds");
            if (string.Equals(shipString, "p")) // Player hits boundary
            {
                // You die, game over
                ship.isDead = true;
            }
            else if (shipString[0].Equals('s')) // Enemy hits boundary
            {
                ship.isDead = true;
                telegraphedBoardState.board[shipRow, shipColumn] = "e";
            }
        }
        else
        {
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
                        // Take Speedometer Damage
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
