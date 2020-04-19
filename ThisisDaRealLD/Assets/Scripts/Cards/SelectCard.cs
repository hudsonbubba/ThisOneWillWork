using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    public IntegerVariable playedCard;
    public GameEvent resetSelectionEvent;

    private void Start()
    {
        e_resetSelection();
    }

    public void e_selectCard(int handIndex)
    {
        resetSelectionEvent.Raise();
        playedCard.SetValue(handIndex);
        this.gameObject.GetComponent<Image>().color = Color.blue;
    }

    public void e_resetSelection()
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
}
