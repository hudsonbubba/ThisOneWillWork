﻿using System.Collections;
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
    public List<GameObject> burnButtonList = new List<GameObject>();
    public IntegerVariable handIndexPlayed;
    public List<Sprite> cardSpriteList = new List<Sprite>();

    // Events
    public GameEvent cardDiscardedEvent;
    public GameEvent cardDrawnEvent;
    public GameEvent reshuffleDiscardEvent;
    public GameEvent previewCardEvent;
    public GameEvent commitTurnEvent;

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
        cardCollection.discardPile.Clear();
        reshuffleDiscardEvent.Raise();
    }

    void drawCard()
    {
        if (cardCollection.drawPile.Count <= 0)
        {
            shuffleDiscardToDraw();
        }

        int nextCard = cardCollection.drawPile[0];
        cardCollection.drawPile.RemoveAt(0);
        if (cardCollection.hand.Count >= maxHandSize)
        {
            discardCard(nextCard);
        }
        else
        {
            cardCollection.hand.Add(nextCard);
        }
        cardDrawnEvent.Raise();
    }

    public void e_initialPreview()
    {
        handIndexPlayed.SetValue(-1);
        e_previewCard();
    }
    
    public void e_previewCard()
    {
        int handIndex = handIndexPlayed.Value;
        if (handIndex == -1)
        {
            playerShip.action = "";
            previewCardEvent.Raise();
        } 
        else
        {
            int cardToPreview = cardCollection.hand[handIndex];
            string cardAction = cardIndex.array[cardToPreview];
            playerShip.action = cardAction;
            previewCardEvent.Raise();
        }
    }

    public void e_commitCard()
    {
        int handIndex = handIndexPlayed.Value;
        if (handIndex == -1) // Pass Turn
        {
            playerShip.action = "";
            commitTurnEvent.Raise();
        }
        else // Play Card
        {
            int cardToCommit = cardCollection.hand[handIndex];
            string cardAction = cardIndex.array[cardToCommit];
            playerShip.action = cardAction;
            commitTurnEvent.Raise();

            cardCollection.hand.RemoveAt(handIndex);
            discardCard(cardToCommit);
        }
        handIndexPlayed.SetValue(-1);
    }

    public void e_burnCard()
    {
        int handIndex = handIndexPlayed.Value;
        int cardToBurn = cardCollection.hand[handIndex];
        cardCollection.hand.RemoveAt(handIndex);
        discardCard(cardToBurn);

        handIndexPlayed.SetValue(-1);
        e_commitCard();
    }

    void discardCard(int cardToDiscard)
    {
        cardCollection.discardPile.Add(cardToDiscard);
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
            burnButtonList[i].SetActive(false);
            if (i < cardCollection.hand.Count)
            {
                int cardNumber = cardCollection.hand[i];
                //string cardAction = cardIndex.array[cardNumber];

                cardButton.GetComponent<Image>().sprite = cardSpriteList[cardNumber];
                //cardButton.GetComponentInChildren<Text>().text = cardAction;
                cardButton.SetActive(true);
                burnButtonList[i].SetActive(true);
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
