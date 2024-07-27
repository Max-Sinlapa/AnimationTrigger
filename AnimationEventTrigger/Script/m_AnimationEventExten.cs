using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MaxDev.Animation.Event
{
    [System.Serializable]
    public class m_AnimationEventExten
    {
        [HideInInspector]public string animationName;
        [HideInInspector]public float animationTime;
        [SerializeField] private UnityEvent animationStartEvent;
        [SerializeField] private UnityEvent animationEndEvent;
        [SerializeField] private List<OptionalAnimationEvent> optionalAnimationEvent;

        public UnityEvent AnimationStartEvent => animationStartEvent;
        public UnityEvent AnimationEndEvent => animationEndEvent;
        public List<OptionalAnimationEvent> OptionalAnimationEvent => optionalAnimationEvent;
    }

    [System.Serializable]
    public class OptionalAnimationEvent : IComparable<OptionalAnimationEvent>
    {
        [Range(0f, 1f)]
        public float PersenAnimationToTrigger;

        public UnityEvent AnimationTrigerEvent;
        

        public int CompareTo(OptionalAnimationEvent other)
        {
            if (other.PersenAnimationToTrigger > this.PersenAnimationToTrigger) return 0;
            if (other.PersenAnimationToTrigger < this.PersenAnimationToTrigger) return 1;
            return PersenAnimationToTrigger.CompareTo(other.PersenAnimationToTrigger);
        }
    }
}