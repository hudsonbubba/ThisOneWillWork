using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardCollection cardCollection;

    // Events
    public GameEvent cardDiscardedEvent;

    void shuffleDeckToDraw()
    {
        cardCollection.drawPile = shuffle(cardCollection.deck);
    }

    void shuffleDiscardToDraw()
    {
        cardCollection.drawPile = shuffle(cardCollection.discardPile);
    }

    List<int> shuffle(List<int> listToShuffle)
    {
        List<int> listCopy = listToShuffle;
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

    void playCard(int handIndex)
    {
        int cardToPlay = cardCollection.hand[handIndex];
        // Do card effect or talk to whatever manager handles playing the card
        discardCard(handIndex);
    }

    void discardCard(int handIndex) // handIndex is what index the card was in your hand
    {
        int cardToDiscard = cardCollection.hand[handIndex];
        cardCollection.discardPile.Add(cardToDiscard);
        cardCollection.hand.RemoveAt(handIndex);

        cardDiscardedEvent.Raise();
    }

}
