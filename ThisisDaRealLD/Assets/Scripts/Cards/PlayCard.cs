using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard : MonoBehaviour
{
    public GameEvent playCardEvent;
    public IntegerVariable playedCard;
    
    public void playCard(int handIndex)
    {
        playedCard.SetValue(handIndex);
        playCardEvent.Raise();
    }
}
