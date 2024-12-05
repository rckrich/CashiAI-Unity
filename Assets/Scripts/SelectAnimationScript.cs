using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyMinnow.SALSA;

public class SelectAnimationScript : MonoBehaviour
{
    public Emoter emoter;
    public Animator animator;

    public void OnRetrieveAnimation(string animationRetrieve)
    {
        switch (animationRetrieve)
        {
            case "Talking_0":
                animator.SetTrigger("Talking_0");
                break;
            case "Talking_1":
                animator.SetTrigger("Talking_1");
                break;
            case "Talking_2":
                animator.SetTrigger("Talking_2");
                break;
            case "Crying":
                animator.SetTrigger("hasCrying");
                emoter.ManualEmote("sad", ExpressionComponent.ExpressionHandler.RoundTrip);
                break;
            case "Laughing":
                animator.SetTrigger("hasLaughing");
                break;
            case "Rumba":
                animator.SetTrigger("hasDancing");
                break;
            case "idle":
                break;
            case "Terrified":
                animator.SetTrigger("hasTerrified");
                break;
            case "Angry":
                animator.SetTrigger("hasAngry");
                break;
        }
    }

    public void ReatrieveEmote(string emote)
    {
        emoter.ManualEmote(emote, ExpressionComponent.ExpressionHandler.RoundTrip);
    }
}
