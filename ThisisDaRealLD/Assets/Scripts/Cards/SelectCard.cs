using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    public IntegerVariable playedCard;
    public GameEvent resetSelectionEvent;

    private bool isSelected = false;

    private void Start()
    {
        e_resetSelection();
    }

    public void e_selectCard(int handIndex)
    {
        if (!isSelected)
        {
            resetSelectionEvent.Raise();
            playedCard.SetValue(handIndex);
            this.gameObject.GetComponent<Image>().color = Color.blue;
            isSelected = true;
        }
        else
        {
            e_resetSelection();
            resetSelectionEvent.Raise();
            playedCard.SetValue(-1);
        }
    }

    public void e_resetSelection()
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
        isSelected = false;
    }
}
