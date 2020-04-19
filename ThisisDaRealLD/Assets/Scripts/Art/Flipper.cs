using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CardArt cardArt;
    string onDeckArt;

    Animator animController;

    public GameEvent EndAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardArt = GetComponent<CardArt>();
        animController = GetComponent<Animator>();
    }

    public void FlipCard(string shipString, string dir)
    {
        //Stop Coroutine
        //StartCoroutine
        //spriteRenderer.sprite = startImage; Removed arg from above
        onDeckArt = shipString;
        animController.SetTrigger(dir);
    }

    public void ae_ArtSwap() //called by an animation event on the last frame of the flipStart animation (when it is invisible)
    {
        Debug.Log(this.ToString() + " - Should change to: " + onDeckArt.ToString());
        cardArt.setState(onDeckArt);
        //spriteRenderer.sprite = onDeckArt;
    }

    public void ae_SwapCardBack()
    {
        cardArt.setState("b");
    }

    public void ae_EndAnimation() // called on the last frame of the flipEnd animation to notify it has finished
    {
        EndAnimation.Raise();
    }

    /*
    IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex, string dir)
    {

        spriteRenderer.sprite = startImage;

        while (time <= 1f) //should probably be a modular duration
        {

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
    */
}
