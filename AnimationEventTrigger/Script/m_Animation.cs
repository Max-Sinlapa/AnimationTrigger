using System.Collections;
using UnityEngine;
using System.Collections;

public class m_Animation : MonoBehaviour
{
    public void Start()
    {
        // existing components on the GameObject
        AnimationClip clip;
        Animator anim;

        // new event created
        AnimationEvent evt;
        evt = new AnimationEvent();

        // put some parameters on the AnimationEvent
        //  - call the function called PrintEvent()
        //  - the animation on this object lasts 2 seconds
        //    and the new animation created here is
        //    set up to happen 1.3s into the animation
        evt.functionName = "PrintEvent";

        // get the animation clip and add the AnimationEvent
        anim = GetComponent<Animator>();

        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length ; i++)
        {
            clip = anim.runtimeAnimatorController.animationClips[i];
            evt.time = clip.length/2;
            evt.stringParameter = anim.runtimeAnimatorController.animationClips[i].name;
            clip.AddEvent(evt);
        }
    }

    // the function to be called as an event
    void PrintEvent(string i)
    {
        print("PrintEvent: " + i + " called at: " + Time.time);
    }
}


