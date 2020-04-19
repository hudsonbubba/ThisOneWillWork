using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalManager : MonoBehaviour
{
    public FloatVariable signalSO;
    public float SIGNAL_MAX;
    public float signalGainPerTurn;
    public float signalGainPerRelay;

    private void Start()
    {
        signalSO.SetValue(0);
    }

    public void e_endOfTurn()
    {
        signalSO.Value += signalGainPerTurn;
        if (signalSO.Value >= SIGNAL_MAX)
        {
            signalSO.Value = SIGNAL_MAX;
            signalComplete();
        }
    }

    public void e_signalRelayPassed()
    {
        signalSO.Value += signalGainPerRelay;
        if (signalSO.Value >= SIGNAL_MAX)
        {
            signalSO.Value = SIGNAL_MAX;
            signalComplete();
        }
    }

    void signalComplete()
    {
        Debug.Log("Signal has reached the maximum. Reach max speed to get outta there!");
    }
}
