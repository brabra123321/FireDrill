using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace FacilityDisplay
{
    /// <summary>
    /// 用来扒下Prefab的预览图存在textures里面以供可能需要的使用
    /// </summary>
    public class PreviewImage : Editor
    {
        public static string SavePath;
        /// <summary>
        /// 构建图片保存路径，确保目录存在
        /// </summary>
        private static void SetupPath()
        {
            SavePath = Application.dataPath + "/Textures/PreviewImgs/";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }
        /// <summary>
        /// 通过AssetPreview来拔去prefab的预览图并且保存
        /// </summary>
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

    }
}
