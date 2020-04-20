using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArtManager : MonoBehaviour
{

    public BoardState boardStateActual;
    public BoardState boardSlideState;

    public List<GameObject> cardList = new List<GameObject>();

    public const int rows = 5;
    public const int columns = 10;
    public GameObject[,] cards = new GameObject[rows, columns];

    private bool animDone = false;
    private int animCounter = 0;
    private int animCounterMax = 2;

    public GameEvent endAnimationProcessEvent;

    public Ship playerShip;

    string currentState;
    string currentStateSlide;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("Card")) 
        {
            cardList.Add(card);
        }

        int z = 0;
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            for (int j = 0; j < cards.GetLength(1); j++)
            {
                cards[i, j] = cardList[z];
                z++;
            }
        }

    }

    public void e_UpdateBoard ()
    {
        //string currentState; - defined globally
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            for (int j = 0; j < cards.GetLength(1); j++)
            {
                //cards[i, j] = cardList[z];
                currentState = boardStateActual.board[i, j];
                cards[i, j].GetComponent<CardArt>().setState(currentState);
            }
        }
    }

    public void e_UpdateSlideBoard()
    {

        for (int i = 0; i < cards.GetLength(0); i++)
        {
            for (int j = 0; j < cards.GetLength(1); j++)
            {
                //cards[i, j] = cardList[z];
                currentState = boardStateActual.board[i, j];
            }
        }

        //string currentState;
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            for (int j = 0; j < cards.GetLength(1); j++)
            {
                //cards[i, j] = cardList[z];
                currentStateSlide = boardSlideState.board[i, j];
                cards[i, j].GetComponent<CardArt>().setState(currentStateSlide);
                if (currentStateSlide == "b")
                {
                    Debug.Log("Card was B");
                    cards[i, j].GetComponent<Flipper>().FlipCard(currentState, "up");
                }
            }
        }

        /*
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            for (int j = 0; j < cards.GetLength(1); j++)
            {
                //cards[i, j] = cardList[z];
                
                if(currentStateSlide == "b")
                {
                    Debug.Log("Card was B");
                    cards[i, j].GetComponent<Flipper>().FlipCard(currentState, "up");
                }

            }
        }
        */
    }

    public void AnimateCard(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString, string targetString, bool isDead)
    {
        // StopCoroutine("AnimateCardEnumurator");
        StartCoroutine(AnimateCardEnumurator(fromRow, fromCol, toRow, toCol, dir, shipString, targetString, isDead));
    }

    IEnumerator AnimateCardEnumurator(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString, string targetString, bool isDead)
    {
        animDone = false;
        animCounter = 0;
        setCounterMax(shipString, targetString);

        if (!(string.Equals(shipString, "mr1") || string.Equals(shipString, "ml1")))
        {
            GameObject movingFromCard = cards[fromRow, fromCol];
            movingFromCard.GetComponent<Flipper>().FlipCard("e", dir); // The card being moved from should always be empty
            yield return new WaitForSeconds(0.25f);
        }


        if (string.Equals(targetString, "o") || string.Equals(targetString, "x") || (shipString[0].Equals('m') && (targetString[0].Equals('s') || targetString[0].Equals('p')))) // Hitting an obstacle (meaning ship or missile hits it) or hitting a barrier (player/enemy) or a missile hits a ship (player or enemy)
        {
            GameObject movingToCard = cards[toRow, toCol];
            movingToCard.GetComponent<Flipper>().FlipCard("d", dir); // Flip to the explosion card first

            yield return new WaitUntil(() => animDone);
            yield return new WaitForSeconds(0.35f); // Show explosion for a time before starting flip back
            postExplosionAnimator(toRow, toCol, dir, shipString, targetString);
        }
        else
        {
            if (!isDead)
            {
                GameObject movingToCard = cards[toRow, toCol];
                movingToCard.GetComponent<Flipper>().FlipCard(shipString, dir);
            }
        }
    }

    void postExplosionAnimator (int toRow, int toCol, string dir, string shipString, string targetString)
    {
        if (string.Equals(targetString, "x"))
        {
            GameObject movingToCard = cards[toRow, toCol];
            movingToCard.GetComponent<Flipper>().FlipCard("x", dir); // Flip back to boundary
        }
        else if (string.Equals(targetString, "o"))
        {
            if (string.Equals(shipString, "p") && !playerShip.isDead) // Ship is a still-alive player
            {
                GameObject movingToCard = cards[toRow, toCol];
                movingToCard.GetComponent<Flipper>().FlipCard("p", dir); // Flip back to player
            }
            else // Otherwise location should always become empty (ship, missile or dead player hitting obstacle)
            {
                GameObject movingToCard = cards[toRow, toCol];
                movingToCard.GetComponent<Flipper>().FlipCard("e", dir); // Flip back to empty
            }
        }
        else if (shipString[0].Equals('m')) // Must be a missile hitting a player or ship
        {
            if (string.Equals(targetString, "p") && !playerShip.isDead) // Ship is a still-alive player (DIFFERENT FROM ABOVE SINCE PLAYER IS TARGET STRING)
            {
                GameObject movingToCard = cards[toRow, toCol];
                movingToCard.GetComponent<Flipper>().FlipCard("p", dir); // Flip back to player
            }
            else // Otherwise location should always become empty (ship, missile or dead player hitting obstacle)
            {
                GameObject movingToCard = cards[toRow, toCol];
                movingToCard.GetComponent<Flipper>().FlipCard("e", dir); // Flip back to empty
            }
        }
    }

    void setCounterMax(string shipString, string targetString)
    {
        animCounterMax = 0;
        if (!(string.Equals(shipString, "mr1") || string.Equals(shipString, "ml1")))
        {
            animCounterMax++;
        }


        if (string.Equals(targetString, "o") || string.Equals(targetString, "x") || (shipString[0].Equals('m') && (targetString[0].Equals('s') || targetString[0].Equals('p'))))
        {
            animCounterMax ++;
        }
    }

    public void e_animationDone()
    {
        animCounter++;
        // endAnimationProcessEvent.Raise();
        if (animCounter >= animCounterMax)
        {
            animCounter = 0;
            animDone = true;
        }
    }

}
