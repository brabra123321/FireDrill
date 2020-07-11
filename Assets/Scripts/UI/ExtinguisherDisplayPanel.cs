using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FireDrill
{
    public class ExtinguisherDisplayPanel : MonoBehaviour
    {
        public ExtinguisherInfoManager manager;

        public Text descText;
        public Text stepText;

        public Button prevButton;
        public Button nextButton;
        // Start is called before the first frame update
        void Start()
        {
            //绑定按钮事件
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(manager.ShowPrev);
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(manager.ShowNext);


            //初始化显示
            manager.Init();
            manager.changeEvent += SetDisplay;
            SetDisplay();

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void SetDisplay()
        {
            ExtinguisherInfo curE = manager.getCurInfo();
            if (curE)
            {
                descText.text = curE.Descriptions;
                string temp = curE.UseSteps[0];
                for(int i = 1; i < curE.UseSteps.Count; i++)
                {
                    temp += "\n" + curE.UseSteps[i];//这里也许可以搞一下颜色字号之类的显示，或者在json里面改
                }
                stepText.text = temp;
            }
            SetButtonInteractable();
        }
        private void SetButtonInteractable()
        {
            if (manager.inited)
            {
                prevButton.interactable = manager.HasPrev();
                nextButton.interactable = manager.HasNext();
            }
        }
    }
}
