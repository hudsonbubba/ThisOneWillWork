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
            Debug.Log("Added: " + card.name);
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

    public void AnimateCard(int fromRow, int fromCol, int toRow, int toCol, string dir, string shipString)
    {
        GameObject movingCard = cards[fromCol, fromRow];

        movingCard.GetComponent<Flipper>().FlipCard();
    }

}
