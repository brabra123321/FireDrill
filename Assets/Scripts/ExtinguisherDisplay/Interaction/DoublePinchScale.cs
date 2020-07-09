using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


namespace FireDrill
{
    /// <summary>
    /// 两指并拢张开来进行缩放的交互功能类。
    /// 会利用到Config中配置的参数来进行控制。
    /// </summary>
    [RequireComponent(typeof(LeanSelectable))]
    public class DoublePinchScale : MonoBehaviour, IInteraction
    {
        LeanSelectable selectable;
        float disLeftRate = 0.2f;
        [Header("Settings")]
        //public float Sensitivity = 1.0f;
        public float maxScale = 2.0f;
        public float minScale = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            selectable = GetComponent<LeanSelectable>();
            maxScale = AdjustUtils.MaxScale(gameObject, disLeftRate);
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            float wheelScale = Input.GetAxis("Mouse ScrollWheel");
            if(wheelScale != 0)
            {
                if (transform.localScale.x + wheelScale > maxScale)
                {
                    transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                }
                else if (transform.localScale.x + wheelScale < minScale)
                {
                    transform.localScale = new Vector3(minScale, minScale, minScale);
                }
                else
                {
                    transform.localScale += new Vector3(wheelScale, wheelScale, wheelScale);
                }
            }
#endif
            if(InteractStateMachine.machine.curState == InteractType.DoublePinch)
            {
                if(InteractStateMachine.machine.curInteraction == null)
                {
                    InteractStateMachine.machine.curInteraction = this;
                }
                if(InteractStateMachine.machine.isCurInteraction(this))
                {
                    float pinchScale = InteractStateMachine.machine.pinch;

                    pinchScale = Mathf.Pow(pinchScale, Configs.DoublePinchScaleSensitivity);
                    if (transform.localScale.x * pinchScale > maxScale)
                    {
                        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                    }
                    else if (transform.localScale.x * pinchScale < minScale)
                    {
                        transform.localScale = new Vector3(minScale, minScale, minScale);
                    }
                    else
                    {
                        transform.localScale *= pinchScale;
                    }
                }
            }
        }
    }
}

