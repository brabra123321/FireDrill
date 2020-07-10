using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireDrill
{
    public class AdjustUtils : MonoBehaviour
    {

        public static void AdjustCenterToZero(GameObject target)
        {
            Bounds bounds = getBounds(target);

            target.transform.position -= bounds.center;
        }

        public static void AdjustBottomToZero(GameObject target)
        {
            Bounds bounds = getBounds(target);

            Vector3 bottom = bounds.center;
            bottom.y -= bounds.extents.y;
            target.transform.position -= bottom;
        }

        /// <summary>
        /// 把物体的缩放根据它的包围盒来调整到正好放在指定相机的框里面
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vertical"></param>
        /// <param name="cam"></param>
        /// <param name="edgeRate"></param>
        public static void AdjustSize(GameObject target, bool vertical = true, Camera cam = null, float edgeRate = -1f)
        {
            if (cam == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("<color=red>Can't find MainCamera in the scene");
                    return;
                }
                cam = Camera.main;
            }
            if(edgeRate < 0)
            {
                edgeRate = Configs.AdjustSize_EdgeReserve;
            }
            var bounds = getBounds(target);
            //target.transform.position -= bounds.center;//先移到中心
            float dis = Vector3.Distance(cam.transform.position, Vector3.zero) - cam.nearClipPlane; //bounds.center)
                                                                                                    //- bounds.extents.z;//这里减z是因为要按照相机在z轴的标准来算，让相机能看全物体。
                                                                                                    //Debug.Log(dis);
            float rate = 1.0f;
            if (vertical)
            {
                //float height = Mathf.Tan(cam.fieldOfView / 2f * Mathf.Deg2Rad)
                //    * dis;// * (1f - edgeRate);
                //Debug.Log(height + " " + bounds.extents);
                //经过计算后得出应该使用下面的公式，！Vertical同理
                rate = dis / (bounds.extents.y / ((1f - edgeRate) * Mathf.Tan(cam.fieldOfView / 2f * Mathf.Deg2Rad)) + bounds.extents.z);

            }
            else
            {
                //float width = Mathf.Tan(Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect)
                //    / 2f * Mathf.Deg2Rad) * dis * (1f - edgeRate);
                //Debug.Log(width + " " + bounds.extents);
                rate = dis / (bounds.extents.x / ((1f - edgeRate) * Mathf.Tan(VerticalFovToHorizontal(cam) / 2f * Mathf.Deg2Rad)) + bounds.extents.z);
            }
            target.transform.localScale *= rate;
            target.transform.localPosition *= rate;
        }

        public static void AddBoxCollider(GameObject target)
        {
            Bounds bounds = getBounds(target);
            var collider = target.AddComponent<BoxCollider>();
            Vector3 scale = Vector3.one;
            //Debug.Log(scale + " " + target.transform.localScale);
            scale.x /= target.transform.localScale.x;
            scale.y /= target.transform.localScale.y;
            scale.z /= target.transform.localScale.z;
            //Debug.Log(scale.x);
            Vector3 temp = bounds.center - target.transform.position;
            temp.Scale(scale);
            collider.center = temp;
            temp = bounds.size;
            temp.Scale(scale);
            collider.size = temp;
        }

        public static float MaxScale(GameObject target, float disLeftRate)
        {
            Bounds bounds = getBounds(target);
            float diagnalLength = Mathf.Sqrt(Mathf.Pow(bounds.extents.x, 2) +
                Mathf.Pow(bounds.extents.y, 2) +
                Mathf.Pow(bounds.extents.z, 2)) / 2;
            //Debug.Log(diagnalLength + " " + bounds.extents);
            float camDis = 0f;
            if (Camera.main == null)
            {
                Debug.Log("<color=yellow>No camera is taged MainCamera</color>");
                return 1.0f;
            }
            else
            {
                camDis = Mathf.Abs(Camera.main.transform.position.z) - Camera.main.nearClipPlane;
            }
            return Mathf.Sqrt(camDis * (1f - disLeftRate) / diagnalLength);//记得开根号
        }
        /// <summary>
        /// 获取物体的整个包围盒的函数，通用并且好用
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Bounds getBounds(GameObject target)
        {
            Renderer[] rs = target.GetComponentsInChildren<Renderer>();
            if (rs.Length == 0)
            {
                return new Bounds(Vector3.zero, Vector3.zero);
            }
            Vector3 center = Vector3.zero;
            foreach (var r in rs)
            {
                center += r.bounds.center;
            }
            center /= rs.Length;
            Bounds bounds = new Bounds(center, Vector3.zero);
            foreach (var r in rs)
            {
                bounds.Encapsulate(r.bounds);
            }
            return bounds;
        }
        /// <summary>
        /// 因为18版没有自带的计算函数，因此
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static float VerticalFovToHorizontal(Camera cam)
        {
            return 2 * Mathf.Atan(Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2) * cam.aspect) * Mathf.Rad2Deg;
        }
    }
}

