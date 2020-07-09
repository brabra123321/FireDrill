/*
 * 工具类，专门用来调整物体的中心
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lean.Touch;


namespace FireDrill
{
    /// <summary>
    /// 把AdjustUtils中的方法包装到编辑器拓展里
    /// </summary>
    public class ModelAdjust : Editor
    {

        [MenuItem(@"AdjustUtils/QuicklySetup", priority = 101)]
        public static void SetupExtinguisherDisplayPrefab()
        {
            GameObject[] targets = Selection.gameObjects;
            foreach (var target in targets)
            {
                GameObject parent = new GameObject(target.name);
                parent.transform.position = Vector3.zero;
                parent.transform.rotation = Quaternion.Euler(Vector3.zero);
                parent.transform.localScale = Vector3.one;

                LeanSelectable selectable = parent.AddComponent<LeanSelectable>();
                //selectable.DeselectOnUp = true;
                parent.AddComponent<SingleSwipeRotate>();
                parent.AddComponent<DoublePinchScale>();
                parent.AddComponent<DoubleTwistRotate>();
                parent.AddComponent<ExtinguisherInfo>();

                target.transform.SetParent(parent.transform);
                AdjustUtils.AdjustCenterToZero(target);
                AdjustUtils.AddBoxCollider(target);
                Bounds bounds = AdjustUtils.getBounds(target);
                AdjustUtils.AdjustSize(target, bounds.size.y > bounds.size.x);
            }
        }

        [MenuItem(@"AdjustUtils/AddBoxCollider", priority = 101)]
        public static void AddBoxCollider()
        {
            GameObject[] targets = Selection.gameObjects;
            foreach (var target in targets)
            {
                AdjustUtils.AddBoxCollider(target);
            }
        }

        [MenuItem(@"GameObject/AdjustCenterToZero", priority = 0)]
        public static void AdjustCenterToZero()
        {
            GameObject[] targets = Selection.gameObjects;
            foreach (var target in targets)
            {
                AdjustUtils.AdjustCenterToZero(target);
            }
        }
    
        [MenuItem(@"GameObject/AdjustBottomToZero", priority = 0)]
        public static void AdjustBottomToZero()
        {
            GameObject[] targets = Selection.gameObjects;
            foreach (var target in targets)
            {
                AdjustUtils.AdjustBottomToZero(target);
            }
        }


        [MenuItem(@"GameObject/AdjustSizeVerticallyFullfill",priority = 0)]
        public static void AdjustSizeVerticallyFullfill()
        {
            AdjustSize(true);
        }
        [MenuItem(@"GameObject/AdjustSizeHorizontallyFullfill", priority = 0)]
        public static void AdjustSizeHorizontallyFullfill()
        {
            AdjustSize(false);
        }
        
        
        public static void AdjustSize(bool vertical)
        {
            GameObject[] targets = Selection.gameObjects;
            if(Camera.main  == null)
            {
                Debug.LogError("<color=red>Can't find MainCamera in the scene");
                return;
            }
            Camera cam = Camera.main;
            foreach (var target in targets)
            {
                AdjustUtils.AdjustSize(target, vertical, cam, Configs.AdjustSize_EdgeReserve);
            }
        }
    }

    
}


