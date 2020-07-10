using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{

    public class ExtinguisherInfoManager : MonoBehaviour
    {
        public static string InfoPath;
        public static string ResourcesPath;

        public Transform initTrans;
        public bool inited = false;
        public List<GameObject> extinguishers;

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
                extinguishers[0].SetActive(true);
            }
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

    }
}
