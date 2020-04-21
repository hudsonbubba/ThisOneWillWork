using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDissolver : MonoBehaviour
{

    public Material matDissolve;
    public Material matDefault;

    SpriteRenderer spriteRenderer;

    bool isDissolving = false;
    float fade = 1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //matDefault = spriteRenderer.material;
        matDissolve.SetFloat("_Fade", 0f);
    }

    private void Update()
    {
        if (isDissolving)
        {

            fade += Time.deltaTime;

            if (fade >= 1f)
            {
                spriteRenderer.material = matDefault;
                isDissolving = false;
            }

            matDissolve.SetFloat("_Fade", fade);
        }
    }

    public void e_FadeOut()
    {
        isDissolving = true;
    }

    public void e_ReAppear()
    {
        matDissolve.SetFloat("_Fade", 1);
        fade = 1;
        spriteRenderer.material = matDefault;
    }

}
