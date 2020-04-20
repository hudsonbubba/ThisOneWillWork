using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTokenUpdate : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    public Sprite leftMissile;
    public Sprite rightMissile;
    public Ship myShip;

    GameObject myChild;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myChild = gameObject.transform.Find("MissileTokenArt").gameObject;
    }

    public void e_updateMissileToken()
    {
        if (!myShip.isDead)
        {
            string shipAction = myShip.action;
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
