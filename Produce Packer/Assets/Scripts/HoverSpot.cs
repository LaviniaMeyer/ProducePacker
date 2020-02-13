using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerSelection
{
    public class HoverSpot : MonoBehaviour
    {
        [Tooltip("Tracking space of the OVRCameraRig.\nIf tracking space is not set, the scene will be searched.\nThis search is expensive.")]
        public Transform trackingSpace = null;

        [HideInInspector]
        public OVRInput.Controller activeController = OVRInput.Controller.None;

        private Vector3 pos;

        public GameObject rootHeld;
        public Bin BinTarget;
        public ObjectInteraction heldScript;

        private TextMesh DebugText;
        private Ray selectionRay;

        private void Start()
        {
            DebugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        }

        void Update()
        {
            activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
            selectionRay = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);

            Vector3 controllerPos = OVRInput.GetLocalControllerPosition(activeController);

            pos = new Vector3(selectionRay.direction.x + controllerPos.x, selectionRay.direction.y + 1.8f + controllerPos.y, selectionRay.direction.z -1.5f + controllerPos.z);

            transform.position = pos;

            //DebugText.text = "TARGET IS: " + rootHeld.GetComponentInChildren<ObjectInteraction>().target;
        }
    }
}
