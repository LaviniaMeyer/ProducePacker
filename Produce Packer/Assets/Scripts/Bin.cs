using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bin : MonoBehaviour
{
    public enum BinType { Food, Flower, Gift, Rubbish }
    public BinType thisBin;

    public ControllerSelection.ObjectInteraction heldScript;
    public ControllerSelection.ObjectInteraction lastHeld;
    public progressManager score;

    private TextMesh DebugText;
    private bool updateLock = false;
    public SphereCollider thisSphere;

    public GameObject ChuteEntry;
    public GameObject ChuteExit;

    [HideInInspector] public bool isTut = true;
    private bool retry = false;

    private Tutorial tutScript;
    private AudioHandler audioScript;

    private void Start()
    {
        DebugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        SetupBinTriggers();
        thisSphere = transform.parent.gameObject.GetComponentInChildren<SphereCollider>();
        thisSphere.enabled = false;
        tutScript = GameObject.Find("Tutorial").GetComponent<Tutorial>();
        audioScript = GameObject.Find("Audio").GetComponent<AudioHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interactable" && heldScript == null)
        {
            //Check Item Type
            if (collision.transform.root.GetComponentInChildren<ObjectScript>().objectData.objectType.ToString() == thisBin.ToString())
            {
                CorrectSort();
                Destroy(collision.transform.root.gameObject);
            }
            else
                IncorrectSort();
        }
        else
            OtherCollision(collision.gameObject);
    }

    public void CorrectSort()
    {
        audioScript.PlaySFX(audioScript.CorrectSort);
        if (!isTut)
        {
            if (score.score == 0)
                tutScript.StartGameScreen();
            score.ScoreUp();

        }
        else
        {
            if (!retry)
                tutScript.CorrectSort();
            else
                tutScript.CorrectSortRetry();
        }
    }

    public void IncorrectSort()
    {
        audioScript.PlaySFX(audioScript.IncorrectSort);
        if (!isTut)
        {
            score.WrongItem();
            lastHeld.TriggerChuteMovement();
            DebugText.text = "Wrong sort";
        }
        else
        {
            tutScript.IncorrectSort();
            retry = true;
        }
    }

    public void OtherCollision(GameObject collided)
    {
        DebugText.text = "Something went wrong - sort neither correct nor incorrect";
    }


    public void SetupBinTriggers()
    {
        //Setup Trigger events on object
        GameObject[] Bins = GameObject.FindGameObjectsWithTag("Bin");
        for (int i = 0; i < Bins.Length; i++)
        {
            GameObject triggerObject = Bins[i].GetComponentInChildren<SphereCollider>().gameObject;
            EventTrigger trigger = triggerObject.AddComponent<EventTrigger>();

            EventTrigger.Entry OnHoverEnter_Entry = new EventTrigger.Entry();                                       //Create Event
            OnHoverEnter_Entry.eventID = EventTriggerType.PointerEnter;                                             //Set Event type
            OnHoverEnter_Entry.callback.AddListener((eventData) => { BinPointerEnter(triggerObject); });    //Set Callback

            EventTrigger.Entry OnHoverExit_Entry = new EventTrigger.Entry();
            OnHoverExit_Entry.eventID = EventTriggerType.PointerExit;
            OnHoverExit_Entry.callback.AddListener((eventData) => { BinPointerExit(); });

            EventTrigger.Entry OnClick_Entry = new EventTrigger.Entry();
            OnClick_Entry.eventID = EventTriggerType.PointerClick;
            OnClick_Entry.callback.AddListener((eventData) => { BinPointerClick(); });

            trigger.triggers.Add(OnHoverEnter_Entry);
            trigger.triggers.Add(OnHoverExit_Entry);
            trigger.triggers.Add(OnClick_Entry);
        }

    }

    public void BinPointerEnter(GameObject binTarget)
    {
        //DebugText.text = "Bin Pointer Enter - held object: " + heldScript;
        if (heldScript != null) //heldscript NOT null
        {
            heldScript.target = binTarget;
            //DebugText.text = "Bin Hover - Obj Target Set";
        }

    }

    public void BinPointerExit() //WHY IS EXIT BEING TRIGGERED WITHOUT POINTER LEAVING
    {
        //DebugText.text = "Bin Pointer Exit";
        if (heldScript != null)
        {
            heldScript.target = heldScript.itemHeldPos;
            //DebugText.text = "Bin Exit - Target reset to pointer";
        }
    }

    public void BinPointerClick()
    {
        if (heldScript != null)
        {
            heldScript.DropObject();
        }
    }
}
