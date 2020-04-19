using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{

    Material mat;

    bool isDissolving = false;
    float fade = 1f;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }

            mat.SetFloat("_Fade", fade);
        }
    }

    public void e_FadeOut()
    {
        isDissolving = true;
    }

}
