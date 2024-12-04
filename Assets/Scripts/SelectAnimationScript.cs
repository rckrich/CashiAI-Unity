using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAnimationScript : MonoBehaviour
{
    public Animator animator; 
    public void OnRetrieveAnimation(string animationRetrieve)
    {
        switch (animationRetrieve) {
            case "Talking_0":
                animator.SetTrigger("ButtonPress");
                break;
            case "Talking_1":
                break;
            case "Talking_2":
                break;
            case "Crying":
                animator.SetTrigger("hasCrying");
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
}
