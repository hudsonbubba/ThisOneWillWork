using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArtManager : MonoBehaviour
{

    public BoardState boardStateActual;

    public List<GameObject> cardList = new List<GameObject>();

    public const int rows = 5;
    public const int columns = 10;
    public GameObject[,] cards = new GameObject[rows, columns];

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
        string currentState;
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

    public void AnimateCard(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString, string targetString, bool isDead)
    {
        // StopCoroutine("AnimateCardEnumurator");
        StartCoroutine(AnimateCardEnumurator(fromRow, fromCol, toRow, toCol, dir, shipString, targetString, isDead));
    }

    IEnumerator AnimateCardEnumurator(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString, string targetString, bool isDead)
    {

        //Debug.Log("Called Animate Card, " + shipString.ToString());



        if (!(string.Equals(shipString, "mr1") || string.Equals(shipString, "ml1")))
        {
            GameObject movingFromCard = cards[fromRow, fromCol];
            movingFromCard.GetComponent<Flipper>().FlipCard("e", dir); // The card being moved from should always be empty
            yield return new WaitForSeconds(0.25f);
        }

        if (!isDead)
        {
            GameObject movingToCard = cards[toRow, toCol];
            movingToCard.GetComponent<Flipper>().FlipCard(shipString, dir);
        } else if (string.Equals(targetString, "o") || (shipString[0].Equals('m') && shipString[0].Equals('s'))) // Hitting an obstacle (meaning ship or missile hits it) or a missile hits a enemy ship, then need to set target location to be empty
        {
            GameObject movingToCard = cards[toRow, toCol];
            movingToCard.GetComponent<Flipper>().FlipCard("e", dir);
        }
    }

}
