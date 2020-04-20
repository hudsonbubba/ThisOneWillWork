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

    public GameEvent gameOverEvent;
    public StringVariable deathReason;

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
            deathReason.SetValue("energy");
            gameOverEvent.Raise();
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
