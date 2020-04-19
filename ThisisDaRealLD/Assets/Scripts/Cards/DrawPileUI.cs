using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPileUI : MonoBehaviour
{
    public CardCollection cardCollection;
    public void e_updateDrawPileSize()
    {
        int size = cardCollection.drawPile.Count;
        this.gameObject.GetComponent<Text>().text = size.ToString();
    }
}
