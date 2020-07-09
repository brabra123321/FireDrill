using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace FacilityDisplay
{
    public class PreviewImage : Editor
    {
        public static string SavePath;

        [MenuItem("Assets/SavePreviewImg")]
        public static void SavePreviewImg()
        {
            SetupPath();

            string[] guids = Selection.assetGUIDs;
            Object[] assets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            foreach (var a in assets)
            {
                Texture2D tex = AssetPreview.GetAssetPreview(a);
                byte[] data = tex.EncodeToPNG();
                FileStream fs = File.Open(SavePath + a.name + ".png", FileMode.OpenOrCreate);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
            Debug.Log("Save Preview Image End");
        }

        private static void SetupPath()
        {
            SavePath = Application.dataPath + "/Textures/PreviewImgs/";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }
    }
}
