using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolver : MonoBehaviour
{
    public BoardState officialBoardState;
    public BoardState telegraphedBoardState;

    public Ship playerShip;
    public AliveEnemyList enemyShips;
    public Ship missileShip;

    CardArtManager cardArtManager;

    // Events
    public GameEvent endOfTurnEvent;
    public GameEvent enemyDestroyedEvent;

    public int MAX_SPEED;
    public int objectDamage;
    public int missileDamage;

    private int MAX_ROWS;
    private int MAX_COLUMNS;
    private bool animDone = false;
    private int animCounter = 0;
    private int animCounterMax = 2;

    void Start()
    {
        MAX_ROWS = officialBoardState.board.GetLength(0);
        MAX_COLUMNS = officialBoardState.board.GetLength(1);
        cardArtManager = GameObject.Find("CardArtManager").GetComponent<CardArtManager>();
    }

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
            case "missileLeft":
                shootMissile(ship, "left");
                break;
            case "missileRight":
                shootMissile(ship, "right");
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

    void shootMissile(Ship ship, string direction)
    {
        missileShip.rowPosition = ship.rowPosition;
        missileShip.columnPosition = ship.columnPosition;
        resetShip(missileShip);
        missileShip.shipTypeString = "m1"; // m1 signifies it is the first movement of the missle, so it shouldnt clear the card behind it

        if (string.Equals(direction, "right"))
        {
            while(!missileShip.isDead)
            {
                moveRight(missileShip);
                missileShip.shipTypeString = "m";
            }
        } 
        else
        {
            while (!missileShip.isDead)
            {
                moveLeft(missileShip);
                missileShip.shipTypeString = "m";
            }
        }
    }

    void moveToTarget(Ship ship, int targetRow, int targetColumn, string direction)
    {
        int shipRow = ship.rowPositionTelegraph;
        int shipColumn = ship.columnPositionTelegraph;
        string shipString = ship.shipTypeString;

        

        if (targetRow < 0 || targetRow >= MAX_ROWS || targetColumn < 0 || targetColumn >= MAX_COLUMNS)
        {
            Debug.Log("Trying to move out of bounds");
            if (string.Equals(shipString, "p")) // Player hits boundary
            {
                // You die, game over
                ship.isDead = true;
            }
            else if (shipString[0].Equals('s') || shipString[0].Equals('m')) // Enemy or missile hits boundary
            {
                Debug.Log("Enemy or missile went out of bounds");
                ship.isDead = true;
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
                    ship.rowPositionTelegraph = targetRow;
                    ship.columnPositionTelegraph = targetColumn;
                    break;

                case 'x': // Boundary
                    if (string.Equals(shipString, "p")) // Player hits boundary
                    {
                        // You die, game over
                        ship.isDead = true;
                    }
                    else if (shipString[0].Equals('s') || shipString[0].Equals('m')) // Enemy hits boundary (or missile, technically not possible now but maybe later)
                    {
                        ship.isDead = true;
                    }
                    break;

                case 'o': // Obstacle (meteor, etc.)
                    if (string.Equals(shipString, "p")) // Player hits obstacle
                    {
                        telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                        
                        ship.rowPositionTelegraph = targetRow;
                        ship.columnPositionTelegraph = targetColumn;
                        takeDamage(objectDamage);
                    }
                    else if (shipString[0].Equals('s') || shipString[0].Equals('m')) // Enemy or missile hits obstacle
                    {
                        ship.isDead = true;
                        telegraphedBoardState.board[targetRow, targetColumn] = "e";
                    }
                    break;

                case 'p': // Player Ship
                    if (shipString[0].Equals('m'))
                    {
                        ship.isDead = true; // Missile is now dead
                        takeDamage(missileDamage); // Player takes damage
                    }
                    else
                    {
                        actionInterpreter(playerShip, direction);
                        telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                        ship.rowPositionTelegraph = targetRow;
                        ship.columnPositionTelegraph = targetColumn;
                    }
                    break;

                case 's': // Enemy Ship
                    Ship hitShip = findEnemyShipByTypeString(targetCurrentContents);
                    if (shipString[0].Equals('m'))
                    {
                        ship.isDead = true; // Missile is now dead
                        hitShip.isDead = true; // Hit ship is now dead
                        telegraphedBoardState.board[targetRow, targetColumn] = "e";
                    }
                    else
                    {
                        actionInterpreter(hitShip, direction);
                        telegraphedBoardState.board[targetRow, targetColumn] = shipString;
                        ship.rowPositionTelegraph = targetRow;
                        ship.columnPositionTelegraph = targetColumn;
                    }
                    break;

                default:
                    Debug.Log("Encountered unknown character in the board state");
                    return;

            }
        }
        if (!string.Equals(shipString, "m1"))
        {
            // All actions require clearing the previous space unless it is the first movement of a missile
            telegraphedBoardState.board[shipRow, shipColumn] = "e";
        }

        StartCoroutine(updateArt(shipRow, shipColumn, targetRow, targetColumn, direction, shipString, ship.isDead));
    }

    IEnumerator updateArt(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString, bool isDead)
    {
        animDone = false;
        animCounterMax = 0;
        if (!string.Equals(shipString, "m1"))
        {
            animCounterMax++;
        }
        if (!isDead)
        {
            animCounterMax++;
        }
        
        cardArtManager.AnimateCard(fromRow, fromCol, toRow, toCol, dir, shipString, isDead);
        yield return new WaitUntil(() => animDone);
    }
    
    public void e_animationDone()
    {
        Debug.Log("Animation Done! Count is: " + animCounter);
        animCounter++;
        if (animCounter >= animCounterMax)
        {
            animCounter = 0;
            animDone = true;
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
