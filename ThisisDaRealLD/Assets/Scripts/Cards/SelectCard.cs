using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    public IntegerVariable playedCard;
    public GameEvent resetSelectionEvent;
    public GameEvent cardSelectedEvent;

    private bool isSelected = false;

    public Animator animator;

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
            animator.SetBool("glowing", true);
            //this.gameObject.GetComponent<Image>().color = Color.blue;
            isSelected = true;
        }
        else
        {
            e_resetSelection();
            resetSelectionEvent.Raise();
            playedCard.SetValue(-1);
        }

        cardSelectedEvent.Raise();
    }

    public void e_resetSelection()
    {
        animator.SetBool("glowing", false);
        //this.gameObject.GetComponent<Image>().color = Color.white;
        isSelected = false;
    }
}
