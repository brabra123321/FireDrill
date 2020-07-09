using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


namespace FireDrill
{

    [RequireComponent(typeof(LeanSelectable))]
    public class SingleSwipeRotate : MonoBehaviour, IInteraction
    {
        //[Range(1.0f, 5.0f)]
        //public float DetectThreshold = 3.0f;
        public LeanSelectable selectable;
        public float Sensitivity = 0.5f;
        public InteractStateMachine interactState;
        // Start is called before the first frame update
        void Start()
        {
            selectable = GetComponent<LeanSelectable>();
            if(interactState == null)
            {
                interactState = InteractStateMachine.machine;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(InteractStateMachine.machine.curState == InteractType.SingleSwipe)
            {
                if(InteractStateMachine.machine.curInteraction == null)
                {
                    InteractStateMachine.machine.curInteraction = this;
                }
                if(InteractStateMachine.machine.isCurInteraction(this))
                {
                    LeanFinger finger = LeanTouch.Fingers[0];
                    Vector2 differ = finger.ScreenPosition - finger.LastScreenPosition;
                    if (Mathf.Abs(finger.ScreenDelta.x) > Configs.SingleSwipePixelThreshold.x)
                    {
                        transform.Rotate(Vector3.up, -differ.x * Sensitivity, Space.World);
                    }
                    if (Mathf.Abs(finger.ScreenDelta.y) > Configs.SingleSwipePixelThreshold.x)
                    {
                        transform.Rotate(Vector3.right, -differ.y * Sensitivity, Space.World);
                    }
                }
            }
        }
    }
}

