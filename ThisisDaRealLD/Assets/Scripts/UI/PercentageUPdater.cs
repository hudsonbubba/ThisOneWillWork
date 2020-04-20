using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PercentageUPdater : MonoBehaviour
{

    public FloatVariable signalPercantage;
    TextMeshProUGUI textMP;
    private void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMP.text = signalPercantage.Value.ToString() + "%";
    }
}
