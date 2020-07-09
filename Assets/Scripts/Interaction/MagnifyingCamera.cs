/*
 * 暂时注释了，考虑不要这个功能。
 */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using Lean.Touch;

//namespace FireDrill
//{

//    //[ExecuteAlways]
//    [RequireComponent(typeof(Camera))]
//    public class MagnifyingCamera : MonoBehaviour, IInteraction
//    {
//        [Header("Refs")]
//        public Camera totalCam;
//        public FacilitiesManager manager;
//        public RectTransform displayUI;
//        [Space(5)]
//        public GameObject target;
//        public LeanSelectable targetSelectable;
//        [Header("Settings")]
//        public bool magEnabled = false;
//        public float PressDetectTime = 1.0f;
//        [Header("ForDebug")]
//        public Vector2 camMoveRect;
//        public Vector2 fingerPos;
//        #region Deprecated
//        //public bool pressing = false;
//        //public float pressCount = 0f;
//        #endregion

//        Coroutine magRoutine;
//        Camera cam;

//        // Start is called before the first frame update
//        void Start()
//        {
//            #region Deprecated
//            //LeanTouch.OnFingerDown += FingerDown_Mag;
//            //LeanTouch.OnFingerUp += FingerUp_Mag;
//            #endregion
//            if (!CanMagnify())
//            {
//                if(refValid())
//                {
//                    displayUI.gameObject.SetActive(false);
//                }
//            }
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            #region Deprecated
//            //if (pressing)
//            //{
//            //    //判断是否处于按压不动状态
//            //    LeanFinger finger = LeanTouch.Fingers[0];
//            //    Vector2 differ = finger.ScreenPosition - finger.LastScreenPosition;
//            //    if(InteractStateMachine.isMoving2D(finger.ScreenDelta, Configs.SingleSwipePixelThreshold))
//            //    {
//            //        StopPressing();
//            //    }
//            //    else
//            //    {
//            //        if (!magEnabled)
//            //        {
//            //            pressCount += Time.deltaTime;

//            //            if (pressCount >= PressDetectTime)
//            //            {
//            //                magEnabled = true;
//            //                pressing = false;
//            //            }
//            //        }
//            //    }
//            //}
//            #endregion
//            if (!refValid())
//            {
//                return;
//            }
//            target = manager.GetCurDisplayingFacilityObj();
//            targetSelectable = target.GetComponent<LeanSelectable>();
//            if(InteractStateMachine.machine.curState == InteractType.SinglePress && 
//                InteractStateMachine.machine.pressTimeRecord >= Configs.PressDetectDuration)
//            {
//                if(InteractStateMachine.machine.curInteraction == null)
//                {
//                    InteractStateMachine.machine.curInteraction = this;
//                }
//                if(InteractStateMachine.machine.isCurInteraction(this))
//                {
//                    magEnabled = true;
//                }
//            }
//            else if(InteractStateMachine.machine.curState != InteractType.SingleSwipe)
//            {
//                magEnabled = false;
//            }
//            if(CanMagnify())
//            {
//                if(magRoutine == null)
//                {
//                    magRoutine = StartCoroutine(MagnifyRoutine());
//                }
//                fingerPos = LeanTouch.Fingers[0].ScreenPosition;
//            }
//            else
//            {
//                DisableMagnify();
//            }
//        }

//        bool refValid()
//        {
//            return totalCam && displayUI;// && target;
//        }

//        bool CanMagnify()
//        {
//            return refValid() && magEnabled && target && targetSelectable.IsSelected ;
//        }
//        #region Deprecated
//        //public void FingerDown_Mag(LeanFinger finger)
//        //{
//        //    if(LeanTouch.Fingers.Count > 1)
//        //    {
//        //        StopPressing();
//        //        return;
//        //    }
//        //    if (!pressing)
//        //    {
//        //        pressing = true;
//        //        pressCount = 0f;
//        //    }
//        //}
//        //public void FingerUp_Mag(LeanFinger finger)
//        //{
//        //    if (magEnabled)
//        //    {
//        //        magEnabled = false;
//        //        DisableMagnify();
//        //    }
//        //}
//        //public void StopPressing()
//        //{
//        //    pressing = false;
//        //    PressDetectTime = 0f;
//        //    magEnabled = false;
//        //}
//        #endregion
//        public void DisableMagnify()
//        {
//            magEnabled = false;
//            if (magRoutine != null)
//            {
//                Debug.Log("<color=yellow>stop magnifying</color>");
//                StopCoroutine(magRoutine);
//                magRoutine = null;
//            }
//            displayUI.gameObject.SetActive(false);
//        }

//        IEnumerator MagnifyRoutine()
//        {
//            Bounds bounds = AdjustUtils.getBounds(target);
//            displayUI.gameObject.SetActive(true);
//            cam = GetComponent<Camera>();
//            camMoveRect = Vector2.zero;
//            camMoveRect.x = Mathf.Abs(Mathf.Abs(cam.transform.position.z) - bounds.extents.z) * Mathf.Tan(AdjustUtils.VerticalFovToHorizontal(totalCam) / 2 * Mathf.Deg2Rad);
//            camMoveRect.y = Mathf.Abs(Mathf.Abs(cam.transform.position.z) - bounds.extents.z) * Mathf.Tan(totalCam.fieldOfView / 2 * Mathf.Deg2Rad);
//            displayUI.anchorMin = new Vector2(0f, 0f);
//            displayUI.anchorMax = new Vector2(0f, 0f);
//            displayUI.pivot = new Vector2(0.5f, 0.5f);
//            Debug.Log("<color=green>start magnifying</color>");
//            Debug.Log(Screen.width);
//            while (CanMagnify())
//            {
//                //UI Position
//                Vector2 uiSizeMul = Vector2.one;
//                if(fingerPos.x > Screen.width / 2)
//                {
//                    uiSizeMul.x *= -1;
//                }
//                if(fingerPos.y > Screen.height / 2)
//                {
//                    uiSizeMul.y *= -1;
//                }
//                Vector2 uiNewPos = Vector2.zero;
//                Vector2 parentResolution = (displayUI.parent as RectTransform).sizeDelta;
//                uiNewPos.x = (fingerPos.x / Screen.width) * parentResolution.x /*- Screen.width / 2 */ + displayUI.sizeDelta.x / 2 * uiSizeMul.x;
//                uiNewPos.y = (fingerPos.y / Screen.height) * parentResolution.y /*- Screen.height / 2*/ + displayUI.sizeDelta.y / 2 * uiSizeMul.y;
//                displayUI.anchoredPosition = uiNewPos;
//                //Camera Position
//                Vector3 newPos = cam.transform.position;
//                newPos.x = -camMoveRect.x * (fingerPos.x - Screen.width / 2) / (Screen.width / 2);
//                newPos.y = camMoveRect.y * (fingerPos.y - Screen.height / 2) / (Screen.height / 2);

//                cam.transform.position = newPos;
//                yield return null;
//            }
//        }
//    }
//}
