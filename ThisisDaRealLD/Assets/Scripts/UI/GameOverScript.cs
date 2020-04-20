using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public StringVariable deathCause;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Text>().text = "";
    }

    public void e_displayGameOver()
    {
        string text = "GAME OVER\nCause of death: ";
        switch(deathCause.Value)
        {
            case "boundary":
                text += "Crashed into the boundary";
                break;
            case "object":
                text += "Crashed into an asteroid";
                break;
            case "missile":
                text += "Shot by a missile";
                break;
            case "energy":
                text += "Energy depleted";
                break;
            case "out of bounds":
                text += "Flew too fast";
                break;
            default:
                text += "???";
                break;
        }
        this.gameObject.GetComponent<Text>().text = text;
    }
}
