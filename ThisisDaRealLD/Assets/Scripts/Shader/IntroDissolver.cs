﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDissolver : MonoBehaviour
{

    SpriteRenderer spriteRenderer;


    float fade = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDissolving)
        {
            spriteRenderer.material = matDissolve;

            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }

            matDissolve.SetFloat("_Fade", fade);
        }
    }

    public void e_FadeOut()
    {
        isDissolving = true;
        matDissolve.SetFloat("_Fade", 1);
    }

    public void e_ReAppear()
    {
        matDissolve.SetFloat("_Fade", 1);
        fade = 1;
        spriteRenderer.material = matDefault;
    }

}
