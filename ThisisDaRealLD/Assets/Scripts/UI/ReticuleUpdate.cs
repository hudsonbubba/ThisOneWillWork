using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticuleUpdate : MonoBehaviour
{

    public BoardState telegraphedBoardState;
    SpriteRenderer spriteRenderer;
    public Sprite playerReticule;
    public Sprite enemyReticule;
    string myShipString;

    GameObject myChild;

    private void Start()
    {
        myShipString = GetComponent<MouseOverReticule>().myShipString;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myChild = gameObject.transform.Find("ReticuleArt").gameObject;

        if(myShipString == "p")
        {
            spriteRenderer.sprite = playerReticule;
        }
        else
        {
            spriteRenderer.sprite = enemyReticule;
        }
    }

    public void e_updatePosition()
    {
        for (int row = 0; row < telegraphedBoardState.board.GetLength(0); row++)
        {
            for (int column = 0; column < telegraphedBoardState.board.GetLength(1); column++)
            {
                string indexValue = telegraphedBoardState.board[row, column];
                if (string.Equals(indexValue, myShipString))
                {
                    myChild.SetActive(true);
                    transform.position = new Vector3(column * 2.4f, row * -1.73f, 0f); //THIS IS HOW IT SHOULD BE
                }
                else
                {
                    myChild.SetActive(false);
                }
             
            }
        }
    }
}
