using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArt : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    public Sprite[] faces;
    public Sprite cardBack;

    public int cardIndex;

    //ORDER of sprite import: Back, Empty, Player, EnemyShip, Obstacle, Boundary

    public void toggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = faces[cardIndex];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

#pragma warning disable IDE1006 // Naming Styles
    public void setState(string state)
#pragma warning restore IDE1006 // Naming Styles
    {
        switch (state[0])
        {
            case 'b':
                cardIndex = 0;
                break;
            case 'e':
                cardIndex = 1;
                break;
            case 'p':
                cardIndex = 2;
                break;
            case 's':
                cardIndex = 3;
                break;
            case 'o':
                cardIndex = 4;
                break;
            case 'x':
                cardIndex = 5;
                break;
            case 'm':
                if (state[1].Equals('l'))
                {
                    cardIndex = 6;
                }
                else
                {
                    cardIndex = 7;
                }
                break;
            case 'd':
                cardIndex = 8;
                break;
            default:
                cardIndex = 0; // Default to the card back if something goes wrong and we dont find a valid key
                break;
        }
        toggleFace(true);
    }




}
