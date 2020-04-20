using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverReticule : MonoBehaviour
{

    public string myShipString;
    public StringVariable mouseOverSO;

    public GameEvent mouseOver;
    public GameEvent mouseExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseOver()
    {
        mouseOverSO.Value = myShipString;
        mouseOver.Raise();
    }

    void OnMouseExit()
    {
        mouseOverSO.Value = "";
        mouseExit.Raise(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
