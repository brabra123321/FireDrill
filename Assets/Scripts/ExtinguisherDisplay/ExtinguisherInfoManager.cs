using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{
    public delegate void ChangeCurDisplayingEvent();
    /// <summary>
    /// Display场景中使用的灭火器统一管理用。
    /// </summary>
    public class ExtinguisherInfoManager : MonoBehaviour
    {
        public static string InfoPath;
        public static string ResourcesPath;

        public Transform initTrans;
        public bool inited = false;
        public List<GameObject> extinguishers;
        public int curDisplaying = 0;
        public ChangeCurDisplayingEvent changeEvent;

        public void Init()
        {
            if (inited)
            {
                return;
            }
            SetupPath();
            LoadExtinguishers();
            if (extinguishers.Count > 0)
            {
                curDisplaying = 0;
                extinguishers[0].SetActive(true);
                extinguishers[0].GetComponent<ExtinguisherInfo>().PlayAppearAnim();
            }
            inited = true;
        }


        private void Start()
        {
            Init();
        }


        public void SetupPath()
        {
            InfoPath = Application.dataPath + "/StreamingAssets/ExtinguisherInfos.json";
            ResourcesPath = "Prefabs/";
        }

        public void LoadExtinguishers()
        {
            Debug.Log("Start Loading Facilities");
            if (File.Exists(InfoPath))
            {
                JsonTextReader jsonReader = new JsonTextReader(File.OpenText(InfoPath));
                JObject all = (JObject)JToken.ReadFrom(jsonReader);
                JArray jArray = JArray.Parse(all["Extinguishers"].ToString());
                for (int i = 0; i < jArray.Count; i++)
                {
                    JObject jObject = JObject.Parse(jArray[i].ToString());
                    string prefabName = jObject["prefabName"].ToString();
                    GameObject extinguisherPrefab = Resources.Load<GameObject>(ResourcesPath + prefabName);
                    if (extinguisherPrefab != null)
                    {
                        GameObject extinguisherObj = Instantiate(extinguisherPrefab, initTrans);
                        extinguisherObj.GetComponent<ExtinguisherInfo>().Init(jObject);
                        extinguishers.Add(extinguisherObj);
                        extinguisherObj.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("prefab: " + prefabName + " do not exists");
                    }
                }
            }
            else
            {
                Debug.LogError("info path does not exists");
            }
            Debug.Log("Load Facilities End");
        }


        public bool HasNext()
        {
            return /*extinguishers.Count > 1 && */ curDisplaying < extinguishers.Count - 1;
        }
        public bool HasPrev()
        {
            return /*extinguishers.Count > 1 && */ curDisplaying > 0;
        }

        public ExtinguisherInfo getCurInfo()
        {
            if (!inited)
            {
                return null;
            }
            return extinguishers[curDisplaying].GetComponent<ExtinguisherInfo>();
        }

        public void ShowNext()
        {
            if (!HasNext())
            {
                return;
            }
            ExtinguisherInfo curE = getCurInfo();
            curE.StopAppearAnim();
            curE.ResetState();
            curE.gameObject.SetActive(false);
            curDisplaying++;
            curE = getCurInfo();
            curE.gameObject.SetActive(true);
            curE.PlayAppearAnim();
            changeEvent?.Invoke();
        }

        public void ShowPrev()
        {
            if (!HasPrev())
            {
                return;
            }
            ExtinguisherInfo curE = extinguishers[curDisplaying].GetComponent<ExtinguisherInfo>();
            curE.StopAppearAnim();
            curE.ResetState();
            curE.gameObject.SetActive(false);
            curDisplaying--;
            curE = extinguishers[curDisplaying].GetComponent<ExtinguisherInfo>();
            curE.gameObject.SetActive(true);
            curE.PlayAppearAnim();
            changeEvent?.Invoke();
        }

    }
}
