using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

namespace FireDrill
{
    public enum InteractType
    {   
        None,        //无交互
        SingleSwipe, //单指滑动
        DoubleSwipe, //两指滑动
        DoublePinch, //两指放大或缩小
        SinglePress, //单指按压
        DoublePress, //两指按压（暂时没想到要用的操作
        DoubleTwist, //两指旋转
    }

    /// <summary>
    /// 结合leanTouch的交互状态机，放一个在场景中，
    /// 其他IInteraction类就可以通过这个类的状态来向这个类进行注册操作。
    /// 需要注意的是同一时间只允许一种交互动作，比如缩放的时候不允许旋转
    /// </summary>
    public class InteractStateMachine : MonoBehaviour
    {
        private static InteractStateMachine machineInScene;
        
        public InteractType prevState = InteractType.None;
        public InteractType curState = InteractType.None;
        public float pressTimeRecord = 0f;
        public IInteraction curInteraction;//同时只允许一种交互正在进行
        public bool ignoreStartedOverGui = true;
        LeanFingerFilter fingerFilter = new LeanFingerFilter(true);
        [Header("Debug")]
        public float pinch = 0;
        public float twist = 0;

        /// <summary>
        /// 对外静态接口，这样其他类就不用再查找场景中的引用
        /// </summary>
        public static InteractStateMachine machine
        {
            get
            {
                if (machineInScene == null)
                {
                    machineInScene = FindObjectOfType<InteractStateMachine>();
                    if (machineInScene == null)
                    {
                        Debug.LogError("Need InteractiStateMachine in Scene");
                    }
                }
                return machineInScene;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            prevState = InteractType.None;
            curState = InteractType.None;
            pressTimeRecord = 0f;
            fingerFilter = new LeanFingerFilter(ignoreStartedOverGui);
        }

        private void Update()
        {
            List<LeanFinger> fingers = fingerFilter.GetFingers();
            pinch = LeanGesture.GetPinchScale(fingers);
            twist = LeanGesture.GetTwistDegrees(fingers);
            pressTimeRecord += Time.deltaTime;
            if(fingers.Count == 0)//没有手指操作的时候归位
            {
                prevState = curState;
                curState = InteractType.None;
                pressTimeRecord = 0f;
                curInteraction = null;
            }
            else if(fingers.Count == 1)//单指的状态判断
            {
                LeanFinger finger = fingers[0];
                //没达到移动界限
                if(!isMoving2D(finger.ScreenDelta, Configs.SingleSwipePixelThreshold))
                {
                    //只有之前没动作才会进入单指按压，否则保持原动作
                    //比如本来在单指滑动，暂时压着不动，那就先保持这个状态，之后可以继续滑动
                    if (curState == InteractType.None)
                    {
                        prevState = curState;
                        curState = InteractType.SinglePress;
                    }
                }
                else
                {
                    if(curState != InteractType.SingleSwipe)
                    {
                        prevState = curState;
                        curState = InteractType.SingleSwipe;
                    }
                }
            }
            else if(fingers.Count == 2)//两指的交互状态判断
            {
                if (Mathf.Abs(pinch - 1.0f) > Configs.DoublePinchScaleThreshold)
                {
                    prevState = curState;
                    curState = InteractType.DoublePinch;
                }
                else if(Mathf.Abs(twist) > Configs.DoubleTwistDegreeThreshold)
                {
                    prevState = curState;
                    curState = InteractType.DoubleTwist;
                }
                else if (!isMoving2D(LeanGesture.GetScreenDelta(fingers), Configs.DoubleSwipePixelThreshold))
                {
                    if(curState == InteractType.None)
                    {
                        prevState = curState;
                        curState = InteractType.DoublePress;
                    }
                }
                else
                {
                    if(curState != InteractType.DoubleSwipe)
                    {
                        prevState = curState;
                        curState = InteractType.DoubleSwipe;
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否超过了移动判定界限的工具函数
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool isMoving2D(Vector2 delta, Vector2 threshold)
        {
            if(Mathf.Abs(delta.x) < threshold.x && Mathf.Abs(delta.y) < threshold.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// IInteraction来判断当前执行的交互操作是不是自己的函数，
        /// 放这里面可以避免许多类型转换等问题
        /// </summary>
        /// <param name="interaction"></param>
        /// <returns></returns>
        public bool isCurInteraction(IInteraction interaction)
        {
            return interaction == curInteraction;
        }
    }
}
