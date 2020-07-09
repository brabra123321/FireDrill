using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{
    public class ExtinguisherInfo : MonoBehaviour
    {
        public string ExtinguisherName;
        public string Descriptions;
        public List<string> useSteps;

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
            
        }
    }

}