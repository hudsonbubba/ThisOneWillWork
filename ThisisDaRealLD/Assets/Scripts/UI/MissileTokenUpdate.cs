using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTokenUpdate : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    public Sprite leftMissile;
    public Sprite rightMissile;
    public Ship myShip;
    public AliveEnemyList aliveEnemyList;

    GameObject myChild;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myChild = gameObject.transform.Find("MissileTokenArt").gameObject;
    }

    public void e_updateMissileToken()
    {
        foreach (Ship ship in aliveEnemyList.aliveList)
        {
            Debug.Log("Checking " + myShip.shipTypeString + " against " + ship.shipTypeString);
            if (string.Equals(ship.shipTypeString, myShip.shipTypeString))
            {
                string shipAction = myShip.action;
                Debug.Log("Ship found in alive arry! " + ship.shipTypeString + " with action " + shipAction);
                if (string.Equals(shipAction, "missileLeft"))
                {
                    spriteRenderer.sprite = leftMissile;
                    myChild.SetActive(true);
                    transform.position = new Vector3((myShip.columnPosition * 2.4f) - 1.2f, myShip.rowPosition * -1.73f, 0f);
                }
                else if (string.Equals(shipAction, "missileRight"))
                {
                    spriteRenderer.sprite = rightMissile;
                    myChild.SetActive(true);
                    transform.position = new Vector3((myShip.columnPosition * 2.4f) + 1.2f, myShip.rowPosition * -1.73f, 0f);
                }
                else
                {
                    myChild.SetActive(false);
                }
            }
            else
            {
                myChild.SetActive(false);
            }
        }
    }
}
