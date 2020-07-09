using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

namespace FireDrill
{
    /// <summary>
    /// 两指旋转来控制物体旋转。
    /// 会用到configs中的参数来调整。
    /// </summary>
    public class DoubleTwistRotate : MonoBehaviour, IInteraction
    {

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log(Mathf.Pow(-1, 2.5f) + " " + Mathf.Pow(-1, 2));
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractStateMachine.machine.curState == InteractType.DoubleTwist)
            {
                if (InteractStateMachine.machine.curInteraction == null)
                {
                    InteractStateMachine.machine.curInteraction = this;
                }
                if (InteractStateMachine.machine.isCurInteraction(this))
                {
                    Camera cam = Camera.main;
                    Vector3 axis = transform.InverseTransformDirection(cam.transform.forward);
                    float twistDegree = Mathf.Pow(Mathf.Abs(InteractStateMachine.machine.twist), Configs.DoubleTwistSensitivity)
                        * (InteractStateMachine.machine.twist > 0 ? 1 : -1);
                    //Debug.Log(twistDegree);
                    transform.rotation *= Quaternion.AngleAxis(twistDegree, axis);
                }
            }
        }
    }
}
