using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{
    /// <summary>
    /// 用在Display场景里面的灭火器控制
    /// </summary>
    public class ExtinguisherInfo : MonoBehaviour
    {
        public static string InfoPath;
        public static string ResourcesPath;
        public string ExtinguisherName;
        public string Descriptions;
        public List<string> UseSteps;

        Coroutine appearRotine;

        public void SetupPath()
        {
            InfoPath = Application.dataPath + "StreamingAssets/ExtinguisherInfos.json";
            ResourcesPath = "Prefabs/";
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void Init(JObject jObject)
        {
            ExtinguisherName = jObject["name"].ToString();
            Descriptions = jObject["description"].ToString();
            JArray jArray = JArray.Parse(jObject["steps"].ToString());
            foreach(var jo in jArray)
            {
                UseSteps.Add(jo.ToString());
            }
            ResetState();
        }
        
        /// <summary>
        /// 出现动画的调用接口
        /// </summary>
        public void PlayAppearAnim()
        {
            if(appearRotine != null)
            {
                return;
            }
            appearRotine = StartCoroutine(AppearAnim());
        }
        /// <summary>
        /// 停止出现动画
        /// </summary>
        public void StopAppearAnim()
        {
            if(appearRotine == null)
            {
                return;
            }
            StopCoroutine(appearRotine);
            appearRotine = null;
        }

        /// <summary>
        /// 用携程实现的shader动画
        /// </summary>
        /// <returns></returns>
        IEnumerator AppearAnim()
        {
            Debug.Log(gameObject.name + "Play AppearAnim");
            Bounds bounds = AdjustUtils.getBounds(gameObject);
            float curY = bounds.center.y + bounds.extents.y * 1.01f;
            float desY = bounds.center.y - bounds.extents.y * 1.01f;
            float differPerSec = (desY - curY) / Configs.AppearAnimationLength;
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                foreach (var m in r.materials)
                {
                    m.SetFloat("_UseClip", 1);
                }
            }
            while (curY > desY)
            {
                foreach(var r in renderers)
                {
                    foreach(var m in r.materials)
                    {
                        m.SetFloat("_clipY", curY);
                    }
                }
                yield return null;
                curY += differPerSec * Time.deltaTime;
            }
            foreach (var r in renderers)
            {
                foreach (var m in r.materials)
                {
                    m.SetFloat("_UseClip", 0);
                }
            }
            Debug.Log(gameObject.name + "AppearAnim End");
            appearRotine = null;
        }

        /// <summary>
        /// 重置一下状态，包括transform和clipY等。
        /// </summary>
        public void ResetState()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
            Bounds bounds = AdjustUtils.getBounds(gameObject);
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            float curY = bounds.center.y + bounds.extents.y;
            foreach (var r in renderers)
            {
                foreach (var m in r.materials)
                {
                    m.SetFloat("_clipY", curY);
                    m.SetFloat("_UseClip", 1);
                }
            }
        }
    }

}