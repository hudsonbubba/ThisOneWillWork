using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastRevealer : MonoBehaviour
{

    public FloatVariable broadcastPercentage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3 (1f, broadcastPercentage.Value/100f, 1f);
    }
}
