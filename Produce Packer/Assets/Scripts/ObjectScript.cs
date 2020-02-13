using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectScript : MonoBehaviour
{
    public SortObject objectData;
    [HideInInspector] public ObjectSpawner spawner;

    private int TypeID;
    private GameObject thisTypeObject;
    private float xRotation;

    private void Start()
    {
        RandomiseType();
        GetRandomTypeObject();
        RandomiseRotation();
        CreateNewObject();

        SetupTriggerEvents();
    }

    private void RandomiseType() //Randomise the Type of the new object (Food, Flower, Gift, Rubbish)
    {
        //Get new random value
        TypeID = Random.Range(0, 3);

        objectData = new SortObject(SortObject.SortType.Flower);

        //Handle Values, set object type
        switch (TypeID)
        {
            case 0:
                objectData.objectType = SortObject.SortType.Food;
                break;

            case 1:
                objectData.objectType = SortObject.SortType.Flower;
                break;

            case 2:
                objectData.objectType = SortObject.SortType.Gift;
                break;

            case 3:
                objectData.objectType = SortObject.SortType.Rubbish;
                break;

            default:
                throw new System.ArgumentOutOfRangeException("Type Randomisation Error: Invalid Random Number! Input was: " + TypeID);
        }
    }

    private void GetRandomTypeObject() //Create a random child object of the chosen type, with this object as parent
    {
        int newRandomValue = Random.Range(0, spawner.AllObjects[TypeID].Length);
        thisTypeObject = spawner.AllObjects[TypeID][newRandomValue];
    }

    private void RandomiseRotation()
    {
        xRotation = Random.Range(0, 360);
    }

    private void CreateNewObject()
    {
        GameObject Object = Instantiate(thisTypeObject, transform.position, transform.rotation, transform);
        Object.transform.Rotate(new Vector3(xRotation, 0, 0));

        Rigidbody RB = Object.AddComponent<Rigidbody>();

        Object.name = "Type_" + objectData.objectType;
    }

    private void SetupTriggerEvents()
    {
        GameObject rootObject = transform.root.gameObject;
        ControllerSelection.ObjectInteraction interacter = rootObject.AddComponent<ControllerSelection.ObjectInteraction>();

        //Setup Trigger events on object
        GameObject triggerObject = GetComponentInChildren<SphereCollider>().gameObject;
        EventTrigger trigger = triggerObject.AddComponent<EventTrigger>();

        EventTrigger.Entry OnHoverEnter_Entry = new EventTrigger.Entry();                               //Create Event
        OnHoverEnter_Entry.eventID = EventTriggerType.PointerEnter;                                     //Set Event type
        OnHoverEnter_Entry.callback.AddListener((eventData) => { interacter.OnPointerEnter(); });       //Set Callback

        EventTrigger.Entry OnHoverExit_Entry = new EventTrigger.Entry();
        OnHoverExit_Entry.eventID = EventTriggerType.PointerExit;
        OnHoverExit_Entry.callback.AddListener((eventData) => { interacter.OnPointerExit(); });

        EventTrigger.Entry OnClick_Entry = new EventTrigger.Entry();
        OnClick_Entry.eventID = EventTriggerType.PointerClick;
        OnClick_Entry.callback.AddListener((eventData) => { interacter.OnPointerClick(); });

        trigger.triggers.Add(OnHoverEnter_Entry);
        trigger.triggers.Add(OnHoverExit_Entry);
        trigger.triggers.Add(OnClick_Entry);
    }
}
