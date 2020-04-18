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

    void resolveTurns()
    {
        // First resolve the player
        actionInterpreter(playerShip);
        // Next loop through the enemies and resolve
        enemyShips.aliveList.ForEach(ship => actionInterpreter(ship));
    }

    void actionInterpreter(Ship ship)
    {
        switch (ship.action)
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

    Ship findEnemyShipByTypeString(string targetString)
    {
        Ship targetShip = null;
        bool stopLoop = false;
        enemyShips.aliveList.ForEach(ship =>
        {
            if (stopLoop)
            {
                return;
            }
            else if (string.Equals(ship.shipTypeString, targetString))
            {
                targetShip = ship;
                stopLoop = true;
            }
        });

        if (targetShip is null)
        {
            Debug.Log("Ship in board state is not found in the alive array");
        }

        return targetShip;
    }

    void destroyEnemy(Ship targetEnemy)
    {
        int shipIndex = 0; // <-- Cant leave null, ideally there should be no case where the ship isn't found so it should always be correct number
        bool stopLoop = false;
        int i = 0;
        enemyShips.aliveList.ForEach(enemy =>
        {
            if (stopLoop)
            {
                return;
            }
            else if (string.Equals(enemy.shipTypeString, targetEnemy.shipTypeString))
            {
                shipIndex = i;
                stopLoop = true;
            }
            i++;
        });
        enemyShips.aliveList.RemoveAt(shipIndex);
    }
    
    void moveUp(Ship ship)
    {
        int shipRow = ship.rowPosition;
        int shipColumn = ship.columnPosition;

        int targetRow = shipRow - 1;
        int targetColumn = shipColumn;

        string shipString = ship.shipTypeString;

        string targetCurrentContents = telegraphedBoardState.board[targetRow, targetColumn];
        char targetFirstChar = targetCurrentContents[0];

        switch (targetFirstChar)
        {
            case 'e': // Empty spot
                telegraphedBoardState.board[targetRow, targetColumn] = ship.shipTypeString;
                telegraphedBoardState.board[shipRow, shipColumn] = "e";
                ship.rowPosition = targetRow;
                ship.columnPosition = targetColumn;
                break;
            case 'x': // Boundary
                if (string.Equals(shipString, "p")) // Player hits boundary
                {
                    // You stay in your current lane, no change to telegraphedBoardState
                    // Take Speedometer Damage
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
                    telegraphedBoardState.board[targetRow, targetColumn] = ship.shipTypeString;
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                    ship.rowPosition = targetRow;
                    ship.columnPosition = targetColumn;
                    // Take Speedometer Damage
                }
                else if (shipString[0].Equals('s')) // Enemy hits obstacle
                {
                    destroyEnemy(ship);
                    telegraphedBoardState.board[targetRow, targetColumn] = "e";
                    telegraphedBoardState.board[shipRow, shipColumn] = "e";
                }
                break;
            case 's': // Enemy Ship
                Ship hitShip = findEnemyShipByTypeString(targetCurrentContents);
                moveUp(hitShip);
                telegraphedBoardState.board[targetRow, targetColumn] = ship.shipTypeString;
                telegraphedBoardState.board[shipRow, shipColumn] = "e";
                ship.rowPosition = targetRow;
                ship.columnPosition = targetColumn;
                break;
        }
    }

    void moveDown(Ship ship)
    {

    }

    void moveLeft(Ship ship)
    {

    }

    void moveRight(Ship ship)
    {

    }
}
