using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireDrill
{
    public class Extinguisher : Focusable, IInteractable
    {
        [Header("Debug")]
        public bool interactable = true;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Interacted()
        {

        }

        public bool IsInteractable()
        {
            return focused && interactable;
        }
    }
}
