using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FilesSentUpdate : MonoBehaviour
{

    public IntegerVariable filesSentSo;

    public void e_updateText()
    {
        GetComponent<TextMeshProUGUI>().text = filesSentSo.Value.ToString();
    }

}
