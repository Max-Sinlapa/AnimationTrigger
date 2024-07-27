using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MaxDev.Animation.Event
{
    public class AnimationEventController : MonoBehaviour
    {
        public Animator _animator;
        public m_AnimationEventExten[] animationEvent;

        private string CurrentAnimClip;
        private int CurrentOptionalEventTime;
        
        private float animTimeStart;

        #region UnityEvent

        private void OnValidate()
        {
            if (_animator == null) return;
            if (_animator.runtimeAnimatorController.animationClips.Length <= 0) return;

            if (animationEvent != null &&
                animationEvent.Length == _animator.runtimeAnimatorController.animationClips.Length)
            {
                for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
                {
                    if (animationEvent[i].OptionalAnimationEvent.Count > 1)
                    {
                        animationEvent[i].OptionalAnimationEvent.Sort();
                    }
                }
                return;
            }

            if (animationEvent == null)
                animationEvent = new m_AnimationEventExten[_animator.runtimeAnimatorController.animationClips.Length];
            else
                Array.Resize(ref animationEvent, _animator.runtimeAnimatorController.animationClips.Length);
            
            for (int i = 0; i <  _animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip animationClipInController = _animator.runtimeAnimatorController.animationClips[i];

                if (animationEvent[i] != null)
                {
                    animationEvent[i].animationName = animationClipInController.name;
                    animationEvent[i].animationTime = animationClipInController.length;
                    continue;
                }
   
                animationEvent[i] = new m_AnimationEventExten() { animationName = animationClipInController.name, animationTime = animationClipInController.length};
                    
            }
        }

        void Awake()
        {
            foreach (AnimationClip animClip in _animator.runtimeAnimatorController.animationClips)
            {
                AddStartEventToAnimationClip(animClip);
                AddEndEventToAnimationClip(animClip);
                AddOptionalEventToAnimationClip(animClip);
            }
        }

        #endregion

        #region PublicMethods

        

        #endregion

        #region AddAnimEventFuntion
        

        void AddStartEventToAnimationClip(AnimationClip animationClip)
        {
            AnimationEvent animEvent;
            animEvent = new AnimationEvent();

            animEvent.time = 0f;
            animEvent.functionName = "InvokeAnimStartEvent";
            animEvent.stringParameter = animationClip.name;
            
            animationClip.AddEvent(animEvent);
        }

        void AddEndEventToAnimationClip(AnimationClip animationClip)
        {
            AnimationEvent animEvent;
            animEvent = new AnimationEvent();

            animEvent.time = animationClip.length;
            animEvent.functionName = "InvokeAnimEndEvent";
            animEvent.stringParameter = animationClip.name;
            
            animationClip.AddEvent(animEvent);
        }

        void AddOptionalEventToAnimationClip(AnimationClip animationClip)
        {
            foreach (var animEventExten in animationEvent)
            {
                if (animEventExten.animationName == animationClip.name)
                {
                    if (animEventExten.OptionalAnimationEvent.Count > 0)
                    {
                        foreach (var optionalAnimationEvent in animEventExten.OptionalAnimationEvent)
                        {
                            AnimationEvent animEvent;
                            animEvent = new AnimationEvent();

                            animEvent.time = animationClip.length * optionalAnimationEvent.PersenAnimationToTrigger;
                            animEvent.functionName = "InvokeAnimOptionalEvent";
                            animEvent.stringParameter = animationClip.name;

                            animationClip.AddEvent(animEvent);
                        }
                    }
                    break;
                }
            }
            
        }

        #endregion

        #region InvokeUnityEvent

        void InvokeAnimStartEvent(string animationClipName)
        {
            foreach (var _animationEventExten in animationEvent)
            {
                if (animationClipName == _animationEventExten.animationName)
                {
                    _animationEventExten.AnimationStartEvent.Invoke();
                    break;
                }
            }
        }
        
        void InvokeAnimEndEvent(string animationClipName)
        {
            foreach (var _animationEventExten in animationEvent)
            {
                if (animationClipName == _animationEventExten.animationName)
                {
                    _animationEventExten.AnimationEndEvent.Invoke();
                    
                    ///reset value to ues next AnimEvent
                    CurrentOptionalEventTime = 0;
                    CurrentAnimClip = null;
                    break;
                }
            }
        }

        void InvokeAnimOptionalEvent(string animationClipName)
        {
            if (CurrentAnimClip != animationClipName || CurrentAnimClip == null)
            {
                CurrentOptionalEventTime = 0;
                CurrentAnimClip = animationClipName;
            }

            foreach (var animEventExten in animationEvent)
            {
                if (animEventExten.animationName == animationClipName)
                {
                    for (int i = 0; i < animEventExten.OptionalAnimationEvent.Count; i++)
                    {
                        if (animEventExten.OptionalAnimationEvent[CurrentOptionalEventTime].PersenAnimationToTrigger == animEventExten.OptionalAnimationEvent[i].PersenAnimationToTrigger)
                        {
                            /// Invoke Event by check CurentTimeTrigger that equle
                            animEventExten.OptionalAnimationEvent[i].AnimationTrigerEvent.Invoke();
                        }
                        else if (animEventExten.OptionalAnimationEvent[i].PersenAnimationToTrigger > animEventExten.OptionalAnimationEvent[CurrentOptionalEventTime].PersenAnimationToTrigger)
                        {
                            /// if Next timeTrigger is greater that CurentTimeTrigger ,Add list position to use next event
                            CurrentOptionalEventTime = i;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region PrivateFuntion

        AnimationClip getAnimationClipData(string animClipName)
        {
            AnimationClip animClipToReturn = null;
            
            foreach (var VARIABLE in _animator.runtimeAnimatorController.animationClips)
            {
                if (VARIABLE.name == animClipName)
                {
                    animClipToReturn = VARIABLE;
                    return animClipToReturn;
                    break;
                }
            }
            
            Debug.LogError(animClipName + " is not contain in = " + _animator.name);
            return null;
        }

        #endregion
    }
}