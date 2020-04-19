using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardPileUI : MonoBehaviour
{
    public CardCollection cardCollection;

    private void Start()
    {
        this.gameObject.GetComponent<Text>().text = "0";
    }
    public void e_updateDiscardPileSize()
    {
        int size = cardCollection.discardPile.Count;
        this.gameObject.GetComponent<Text>().text = size.ToString();
    }
}
