using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireDrill
{
    [ExecuteAlways]
    public class Configs : MonoBehaviour
    {
        //JsonConfigs
        private static string path;
        //interact
        public static Vector2 SingleSwipePixelThreshold = new Vector2(3.0f, 3.0f); //in pixel
        public static Vector2 DoubleSwipePixelThreshold = new Vector2(6.0f, 6.0f);
        public static float DoublePinchScaleThreshold = 0.001f;
        public static float DoublePinchScaleSensitivity = 1.0f;
        public static float DoubleTwistDegreeThreshold = 1.5f;
        public static float DoubleTwistSensitivity = 1.0f;
        public static float PressDetectDuration = 1.0f;

        public static float AdjustSize_EdgeReserve = 0.1f;
        public static float AppearAnimationLength = 1.0f;
        public static float DisappearAnimationLength = 0.5f;
        public static float FacilityListAnimationLength = 1.0f;

        //OtherConfigs
        public static Material DisplayClipMat;


        private void Awake()
        {
            LoadConfigs();

            DisplayClipMat = Resources.Load<Material>("WorldYClip");
        }

        public void OnEnable()
        {
            LoadConfigs();
        }

        private void Update()
        {
            //检测重新加载设置的输入
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && Input.GetKeyDown(KeyCode.R))
            {
                LoadConfigs();
            }
        }

        private static void SetupPath()
        {
            path = Application.dataPath + "/StreamingAssets/configs.json";
        }

        private static bool LoadConfigs()
        {
            SetupPath();
            if (File.Exists(path))
            {
                StreamReader file = File.OpenText(path);
                JsonTextReader reader = new JsonTextReader(file);
                JObject jObject = JToken.ReadFrom(reader) as JObject;
                JObject SingleSwipePixelThreshold_Obj = jObject["SingleSwipePixelThreshold"] as JObject;
                SingleSwipePixelThreshold = new Vector2((float)SingleSwipePixelThreshold_Obj["x"], 
                    (float)SingleSwipePixelThreshold_Obj["y"]);
                JObject DoubleSwipePixelThreshold_Obj = jObject["DoubleSwipePixelThreshold"] as JObject;
                DoubleSwipePixelThreshold = new Vector2((float)DoubleSwipePixelThreshold_Obj["x"], 
                    (float)DoubleSwipePixelThreshold_Obj["y"]);
                DoublePinchScaleThreshold = (float)jObject["DoublePinchScaleThreshold"];
                DoublePinchScaleSensitivity = (float)jObject["DoublePinchScaleSensitivity"];
                DoubleTwistDegreeThreshold = (float)jObject["DoubleTwistDegreeThreshold"];
                DoubleTwistSensitivity = (float)jObject["DoubleTwistSensitivity"];
                PressDetectDuration = (float)jObject["PressDetectDuration"];

                AdjustSize_EdgeReserve = (float)jObject["AdjustSize_EdgeReserve"];
                AppearAnimationLength = (float)jObject["AppearAnimationLength"];
                DisappearAnimationLength = (float)jObject["DisappearAnimationLength"];
                FacilityListAnimationLength = (float)jObject["FacilityListAnimationLength"];
            }
            else
            {
                Debug.LogWarning("<color=yellow>" + path + "</color> does not exists. Load config Failed");
                return false;
            }
            Debug.Log("Load Config Success!");
            return true;
        }
    }
}
