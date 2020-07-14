using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireDrill
{

    public class CharacterContorller : MonoBehaviour
    {
        [Header("Refs")]
        public Camera playerCam;


        public float interactDis = 1.0f;
        public GameObject focusingObj;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            #region 检测Focasable物体（往往是可交互物体）。比如柜子或者灭火器
            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * interactDis, Color.blue);
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward,
                out hit, interactDis))//, ~LayerMask.NameToLayer("PlayerBlock")))
            {
                Focusable hitFocusable = hit.collider.transform.GetComponentInParent<Focusable>();
                if (hitFocusable == null)
                {
                    hitFocusable = hit.collider.transform.GetComponentInChildren<Focusable>();
                }
                if (hit.collider.transform.parent != null
                    && hitFocusable != null && hitFocusable.focusable)
                {
                    if (focusingObj != null)
                    {
                        focusingObj.GetComponent<Focusable>().focused = false;
                    }
                    focusingObj = hitFocusable.gameObject;
                    Focusable focusable = focusingObj.GetComponent<Focusable>();
                    focusable.focused = true;
                }
            }
            else
            {
                if (focusingObj != null)
                {
                    focusingObj.GetComponent<Focusable>().focused = false;
                    focusingObj = null;
                }
            }
            #endregion
        }
    }

}