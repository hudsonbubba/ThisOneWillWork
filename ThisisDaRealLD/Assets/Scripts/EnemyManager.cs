using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Ship enemyShipSO;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    int getRowPosition()
    {
        return enemyShipSO.rowPosition;
    }

    int getColumnPosition()
    {
        return enemyShipSO.columnPosition;
    }

    void declareAction(string actionName)
    {
        enemyShipSO.action = actionName;
    }
}
