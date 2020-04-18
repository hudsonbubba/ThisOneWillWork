using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public FloatVariable energySO;
    public float energyLossPerTurn;
    public float energyGainPerKill;
    public float energyGainPerDiscard;
    
    public void e_endOfTurn()
    {
        energySO.Value -= energyLossPerTurn;
    }

    public void e_enemyDestroyed()
    {
        energySO.Value += energyGainPerKill;
    }

    public void e_cardDiscarded()
    {
        energySO.Value += energyGainPerDiscard;
    }
}
