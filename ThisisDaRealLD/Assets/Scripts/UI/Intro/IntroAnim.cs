using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{

    Animator animator;
    AudioSource audio;

    bool isAnim = true;
    //AudioClip beepBoop;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isAnim)
        {
            animator.SetTrigger("Advance");
            audio.Play();
        }

    }

    public void ae_soundCodec()
    {
        audio.Play();
    }

    public void ae_nextSound()
    {
        //audio.clip = beepBoop;
        audio.Play();
    }

    public void ae_endScene()
    {
        //endScene
    }

    public void ae_doneAnim()
    {
        isAnim = false;
    }
}
