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

    /*
    public void AnimateCard(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString)
    {

        //Debug.Log("Called Animate Card, " + shipString.ToString());
        GameObject movingFromCard = cards[fromRow, fromCol];
        GameObject movingToCard = cards[toRow, toCol];

        movingFromCard.GetComponent<Flipper>().FlipCard("e", dir); // The card being moved from should always be empty
        movingToCard.GetComponent<Flipper>().FlipCard(shipString, dir);
    }
    */

    public void AnimateCard(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString)
    {
        StopCoroutine("AnimateCardEnumurator");
        StartCoroutine(AnimateCardEnumurator(fromRow, fromCol, toRow, toCol, dir, shipString));
    }

    IEnumerator AnimateCardEnumurator(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString)
    {

        //Debug.Log("Called Animate Card, " + shipString.ToString());
        GameObject movingFromCard = cards[fromRow, fromCol];
        GameObject movingToCard = cards[toRow, toCol];

        movingFromCard.GetComponent<Flipper>().FlipCard("e", dir); // The card being moved from should always be empty
        yield return new WaitForSeconds(0.25f);
        movingToCard.GetComponent<Flipper>().FlipCard(shipString, dir);
    }

}
