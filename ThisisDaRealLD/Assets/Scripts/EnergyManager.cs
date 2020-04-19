using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public FloatVariable energySO;
    public float energyLossPerTurn;
    public float energyGainPerKill;
    public float energyGainPerBurn;
    public float energyMax;

    private void Start()
    {
        energySO.SetValue(energyMax);
    }

    public void e_endOfTurn()
    {
        energySO.Value -= energyLossPerTurn;
        if (energySO.Value <= 0)
        {
            Debug.Log("Energy fully depleted. Game Over!");
        }
    }

    public void e_enemyDestroyed()
    {
        energySO.Value += energyGainPerKill;
        if (energySO.Value >= energyMax)
        {
            energySO.SetValue(energyMax);
        }
    }

    public void e_cardBurned()
    {
        energySO.Value += energyGainPerBurn;
        if (energySO.Value >= energyMax)
        {
            energySO.SetValue(energyMax);
        }
    }
}
