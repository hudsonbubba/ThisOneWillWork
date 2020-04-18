using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CardArt cardArt;

    public AnimationCurve scaleCurve;
    public float animationDuration = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardArt = GetComponent<CardArt>();
    }

    public void FlipCard(Sprite startImage, Sprite endImage, int cardIndex, string dir)
    {
        //Stop Coroutine
        //StartCoroutine
    }

    IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex, string dir)
    {

        spriteRenderer.sprite = startImage;

        float time = 0f;
        while (time <= 1f) //should probably be a modular duration
        {
            float scale = scaleCurve.Evaluate(time);
            time =+ Time.deltaTime / animationDuration;

            Vector3 localScale = transform.localScale;
            switch (dir)
            {
                case "up":
                    //localScale.x
                    break;
                case "right":
                    break;
                case "down":
                    break;
                case "left":
                    break;
            }

            //localScale.x = scale;
            //transform.localScale = localScale;

            if (time >= 0.5f)
            {
                spriteRenderer.sprite = endImage;
            }

        }
        yield break;
    }
}
