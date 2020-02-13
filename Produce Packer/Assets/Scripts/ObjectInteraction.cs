using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerSelection
{
    public class ObjectInteraction : MonoBehaviour
    {
        private GameObject[] pointerActivated;
        private List<GameObject> thisObject;
        public GameObject itemHeldPos;
        public ControllerSelection.HoverSpot pointerControl;
        public SphereCollider[] binTriggers;

        public float moveSpeed = 2f;
        private Vector3 pointerPos;
        private Vector3 dir;
        public GameObject target;

        private bool isHeld = false;
        private bool useTranslate = true;
        private TextMesh DebugText;

        private GameObject[] bins;
        private int numPickup = 0;
        private bool targetIsPointer;

        public bool death = false;
        private GameObject chuteExit;

        private AudioHandler audioScript;

        public OVRInput.Controller activeController = OVRInput.Controller.None;
        private Ray selectionRay;

        public Transform trackingSpace = null;

        private float yDif = 1.5f;

        //public Transform startMarker;

        //// Time when the movement started.
        //private float startTime;

        //// Total distance between the markers.
        //private float journeyLength;

        private void OnEnable()
        {
            DebugText = GameObject.Find("Debug").GetComponent<TextMesh>();
            itemHeldPos = GameObject.Find("HoverSpotGrab");
            thisObject = new List<GameObject>();
            trackingSpace = GameObject.Find("TrackingSpace").transform;
        }

        private void Start()
        {
            pointerActivated = GameObject.FindGameObjectsWithTag("PointerActivated");
            pointerControl = itemHeldPos.GetComponentInChildren<HoverSpot>();
            target = itemHeldPos;
            bins = GameObject.FindGameObjectsWithTag("Bin");
            audioScript = GameObject.Find("Audio").GetComponent<AudioHandler>();

            //Setup bin pointer trigger variables
            int binNum = GameObject.FindGameObjectsWithTag("Bin").Length;
            binTriggers = new SphereCollider[binNum];
            for (int i = 0; i < binNum; i++)
            {
                binTriggers[i] = bins[i].GetComponentInChildren<Bin>().thisSphere;
            }

            //Setup pointer hover objects
            for (int i = 0; i < pointerActivated.Length; i++)
            {
                if (pointerActivated[i].transform.root == transform)
                {
                    thisObject.Add(pointerActivated[i]);
                    pointerActivated[i].SetActive(false);
                    //add rotator to pointer
                }
            }
        }

        private void Update()
        {
            //if (!targetIsPointer)
                //Get the direction from adjusted target to current position
                dir = transform.TransformDirection(target.transform.position - transform.GetChild(0).position); //Instead of this use difference of target from 0,0? Position is relative to SOMETHING
            //Rigidbody was on child, position not accounting for physics movement from that child
            //Fix: Adjusted calculation to be from the RB object
            //else
            //{
            //    activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
            //    selectionRay = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);

            //    Vector3 controllerPos = OVRInput.GetLocalControllerPosition(activeController);

            //    dir = (new Vector3(
            //        selectionRay.direction.x + controllerPos.x,
            //        selectionRay.direction.y + 1.8f + controllerPos.y,
            //        selectionRay.direction.z - 1.5f + controllerPos.z)) - transform.position + new Vector3(0, 1.5f, 0);
            //}



            //Control whether to move by translate or transform
            if (isHeld && useTranslate || death)
            {
                transform.Translate(dir * moveSpeed * Time.deltaTime);
                //Move in DIRECTION at speed
                //Direction calculation position does not match current position?
                //transform.position = target.transform.position;

                //// Distance moved equals elapsed time times speed..
                //float distCovered = (Time.time - startTime) * moveSpeed;

                //// Fraction of journey completed equals current distance divided by total distance.
                //float fractionOfJourney = distCovered / journeyLength;

                //// Set our position as a fraction of the distance between the markers.
                //transform.position = Vector3.Lerp(startMarker.position, target.transform.position + new Vector3(0, 1.5f, 0), fractionOfJourney);
            }
            //else if (isHeld)
            //    transform.position = target.transform.position + new Vector3(0, 1.5f, 0);

            //Control selector rotation
            for (int i = 0; i < pointerActivated.Length; i++)
            {
                if (pointerActivated[i] != null && pointerActivated[i].transform.root == transform)
                {
                    pointerActivated[i].transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x * 90f + 180f, 0.0f, gameObject.transform.rotation.z * 180f) * Quaternion.Euler(-90f, 0.0f, 0.0f);
                }
            }

            //Wrong sort movement:
            if (death && targetIsPointer)
            {
                if (transform.position.z >= target.transform.position.z - 0.5f)
                {
                    target = chuteExit;
                    targetIsPointer = false;
                    audioScript.PlaySFX(audioScript.VentTumble);
                }
            }
        }

        public void OnPointerEnter()
        {
            if (pointerControl.rootHeld == null || pointerControl.rootHeld == transform.root && !death)
                TogglePointerIndicators(true);
        }

        public void OnPointerExit()
        {
            TogglePointerIndicators(false);
        }

        public void OnPointerClick()
        {
            //Check if the player is already holding an object
            if (pointerControl.rootHeld == null && !death)
            {
                isHeld = true;
                itemHeldPos = GameObject.Find("HoverSpotGrab");
                pointerControl = itemHeldPos.GetComponentInChildren<HoverSpot>();
                target = itemHeldPos;
                targetIsPointer = true;
                pointerControl.rootHeld = transform.root.gameObject;
                numPickup++;
                //if (numPickup > 1)
                //    yDif = itemHeldPos.transform.position.y - transform.position.y;

                //startMarker = transform;
                //journeyLength = Vector3.Distance(target.transform.position, startMarker.position);
                //startTime = Time.time;

                audioScript.PlaySFX(audioScript.ItemPickup);

                for (int i = 0; i < bins.Length; i++)
                {
                    bins[i].GetComponentInChildren<Bin>().heldScript = this;
                    bins[i].GetComponentInChildren<Bin>().lastHeld = this;
                    //DebugText.text = "Bin heldscript set";
                }

                GetComponentInChildren<Rigidbody>().isKinematic = true;
                ToggleBinTriggers();
                //DebugText.text = "Toggle bin triggers PICKUP";
            }
            else if (pointerControl.rootHeld == transform.root.gameObject) //Drop this object if it was held already
            {
                DropObject();
            }

            Tutorial tut;
            tut = GameObject.Find("Tutorial").GetComponent<Tutorial>();
            if (!tut.firstObjectSorted)
            {
                tut.firstPickup();
                tut.firstObjectSorted = true;
            }
        }

        private void TogglePointerIndicators(bool state)
        {
            for (int i = 0; i < pointerActivated.Length; i++)
            {
                pointerActivated[i].SetActive(state);
            }
        }

        private void ToggleBinTriggers()
        {
            //int binNum = GameObject.FindGameObjectsWithTag("Bin").Length;
            //binTriggers = new SphereCollider[binNum];
            for (int i = 0; i < binTriggers.Length; i++)
            {
                //binTriggers[i] = bins[i].transform.parent.GetComponentInChildren<SphereCollider>();
                bool state = binTriggers[i].enabled;
                binTriggers[i].enabled = !state;
            }
        }

        public void DropObject()
        {
            isHeld = false;
            pointerControl.rootHeld = null;
            for (int i = 0; i < bins.Length; i++)
            {
                bins[i].GetComponentInChildren<Bin>().heldScript = null;
            }

            audioScript.PlaySFX(audioScript.ItemDrop);

            GetComponentInChildren<Rigidbody>().isKinematic = false;
            ToggleBinTriggers();
            //DebugText.text = "Toggle Bin Triggers DROP";
        }

        public void TriggerChuteMovement()
        {
            DebugText.text = "Chute movement triggered";
            gameObject.GetComponentInChildren<Rigidbody>().isKinematic = true;
            death = true;
            target = GameObject.Find("ChuteEntry");
            chuteExit = GameObject.Find("ChuteExit");
        }
    }
}
