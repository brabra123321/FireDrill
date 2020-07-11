using UnityEngine;
using UnityEditor;


namespace FireDrill
{
    /// <summary>
    /// 定义可交互物体的基类
    /// </summary>
    public interface IInteractable
    {
        void Interacted();
        bool IsInteractable();
    }
}