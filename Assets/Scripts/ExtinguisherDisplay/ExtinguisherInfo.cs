using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{
    public class ExtinguisherInfo : MonoBehaviour
    {
        public static string InfoPath;
        public static string ResourcesPath;
        public string ExtinguisherName;
        public string Descriptions;
        public List<string> UseSteps;

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
        }
    }

}