using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{

    public FloatVariable HorizontalAxis;
    public FloatVariable VerticalAxis;
    public FloatVariable MouseXAxis;
    public BoolVariable Mouse1;
    public BoolVariable Mouse2;
    public BoolVariable Space;

    private bool canControl = true;

    public void e_StartCinematic()
    {
        canControl = false;
    }

    public void e_EndCinematic()
    {
        canControl = true;
    }

    void Update()
    {
        if (canControl)
        {
            HorizontalAxis.Value = Input.GetAxis("Horizontal");
            VerticalAxis.Value = Input.GetAxis("Vertical");
            MouseXAxis.Value = Input.GetAxis("Mouse X");


            //perhaps this should throw an event instead?
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Mouse1.SetValue(true);
            }
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                Mouse1.SetValue(false);
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Mouse2.SetValue(true);
            }
            if (!Input.GetKey(KeyCode.Mouse1))
            {
                Mouse2.SetValue(false);
            }
            if (Input.GetKey("space"))
            {
                Space.SetValue(true);
            }
            if (!Input.GetKey("space"))
            {
                Space.SetValue(false);
            }
        } else
        {
            HorizontalAxis.Value = 0;
            VerticalAxis.Value = 0;
            MouseXAxis.Value = 0;
            Mouse1.SetValue(false);
            Mouse2.SetValue(false);
            Space.SetValue(false);
        }
    }
}
