using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardCollection : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public List<int> deck = new List<int>(); // Complete deck of cards, order is irrelevant, unshuffled
    public List<int> drawPile = new List<int>();
    public List<int> discardPile = new List<int>();
    public List<int> hand = new List<int>();

}
