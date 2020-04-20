using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnCard : MonoBehaviour
{
    public IntegerVariable playedCard;
    public GameEvent resetSelectionEvent;
    public GameEvent burnedCardEvent;

    public void e_burnCard(int handIndex)
    {
        resetSelectionEvent.Raise();
        playedCard.SetValue(handIndex);
        burnedCardEvent.Raise();
    }
}
