using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public CardCollection cardCollection;
    public StringArrayVariable cardIndex;

    public Ship playerShip;

    public int startHandSize;
    public int maxHandSize;
    public List<GameObject> cardButtonList = new List<GameObject>();
    public IntegerVariable handIndexPlayed;

    // Events
    public GameEvent cardDiscardedEvent;
    public GameEvent previewCardEvent;

    void Start()
    {
        shuffleDeckToDraw();
        for (int i = 0; i < startHandSize; i++)
        {
            drawCard();
        }
        updateHand();
    }

    void shuffleDeckToDraw()
    {
        cardCollection.drawPile = shuffle(cardCollection.deck);
    }

    void shuffleDiscardToDraw()
    {
        cardCollection.drawPile = shuffle(cardCollection.discardPile);
    }

    void drawCard()
    {
        if (cardCollection.drawPile.Count <= 0)
        {
            shuffleDiscardToDraw();
        }
        int nextCard = cardCollection.drawPile[0];
        cardCollection.hand.Add(nextCard);
        cardCollection.drawPile.RemoveAt(0);
    }

    public void e_previewCard()
    {
        int handIndex = handIndexPlayed.Value;
        int cardToPreview = cardCollection.hand[handIndex];
        string cardAction = cardIndex.array[cardToPreview];
        playerShip.action = cardAction;
        previewCardEvent.Raise();

        discardCard(handIndex);
    }

    /*void playCard(int handIndex)
    {
        int cardToPlay = cardCollection.hand[handIndex];
        // Do card effect or talk to whatever manager handles playing the card
        discardCard(handIndex);
    }*/

    void discardCard(int handIndex) // handIndex is what index the card was in your hand
    {
        int cardToDiscard = cardCollection.hand[handIndex];
        cardCollection.discardPile.Add(cardToDiscard);
        cardCollection.hand.RemoveAt(handIndex);

        cardDiscardedEvent.Raise();
        updateHand();
    }

    public void e_endTurn()
    {
        drawCard();
        updateHand();
    }
    
    void updateHand()
    {
        int i = 0;
        foreach (GameObject cardButton in cardButtonList)
        {
            cardButton.SetActive(false);
            if (i < cardCollection.hand.Count)
            {
                int cardNumber = cardCollection.hand[i];
                string cardAction = cardIndex.array[cardNumber];
                cardButton.GetComponentInChildren<Text>().text = cardAction;
                cardButton.SetActive(true);
            }
            i++;
        }
    }

    List<int> shuffle(List<int> listToShuffle)
    {
        List<int> listCopy = new List<int>(listToShuffle);
        int deckSize = listCopy.Count;
        for (int i = 0; i < deckSize; i++)
        {
            int temp = listCopy[i];
            int randomIndex = Random.Range(i, deckSize);
            listCopy[i] = listCopy[randomIndex];
            listCopy[randomIndex] = temp;
        }
        return listCopy;
    }
}
