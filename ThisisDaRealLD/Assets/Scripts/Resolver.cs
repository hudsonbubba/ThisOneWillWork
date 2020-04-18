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

    void previewTurns()
    {
        telegraphedBoardState.board = officialBoardState.board;

        // First resolve the player
        actionInterpreter(playerShip);

        // Next loop through the enemies and resolve
        enemyShips.aliveList.ForEach(ship => actionInterpreter(ship));

        // When actually applying the turns:
        // 1) Make the officialBoardState = telegraphedBoardState
        // 2) Set all ships' shipRow and shipColumn to their new spot
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
            }
            else if (shipString[0].Equals('s')) // Enemy hits boundary
            {
                destroyEnemy(ship);
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
                    break;
                case 'x': // Boundary
                    if (string.Equals(shipString, "p")) // Player hits boundary
                    {
                        // You die, game over
                    }
                    else if (shipString[0].Equals('s')) // Enemy hits boundary
                    {
                        destroyEnemy(ship);
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    }
                    break;
                case 'o': // Obstacle (meteor, etc.)
                    if (string.Equals(shipString, "p")) // Player hits obstacle
                    {
                        telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                        // Take Speedometer Damage
                    }
                    else if (shipString[0].Equals('s')) // Enemy hits obstacle
                    {
                        destroyEnemy(ship);
                        telegraphedBoardState.board[targetRow, targetColumn] = "e";
                        telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    }
                    break;
                case 'p': // Player Ship
                    actionInterpreter(playerShip, direction);
                    telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    break;
                case 's': // Enemy Ship
                    Ship hitShip = findEnemyShipByTypeString(targetCurrentContents);
                    actionInterpreter(hitShip, direction);
                    telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
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

    void destroyEnemy(Ship targetEnemy)
    {
        int shipIndex = -1; // <-- Cant leave null, ideally there should be no case where the ship isn't found so it should always be correct number
        int i = 0;
        foreach (Ship enemy in enemyShips.aliveList)
        {
           if (string.Equals(enemy.shipTypeString, targetEnemy.shipTypeString))
            {
                shipIndex = i;
            }
            i++;
        }

        if (shipIndex == -1)
        {
            Debug.Log("Destroyed ship is not in the alive ship array");
        }

        enemyShips.aliveList.RemoveAt(shipIndex);
    }
}
