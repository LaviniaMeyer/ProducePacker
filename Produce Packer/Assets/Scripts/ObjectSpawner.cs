using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject[] FoodObjects;
    public GameObject[] FlowerObjects;
    public GameObject[] GiftObjects;
    public GameObject[] RubbishObjects;

    [HideInInspector] public GameObject[][] AllObjects;

    [Header("Timers")]
    public float minWait = 2f;
    public float maxWait = 2.5f;
    public float defMinWait = 2f;
    public float defMaxWait = 2.5f;

    private float timer;
    private float thisWait;

    public bool startGame = false;
    private AudioHandler audioScript;

    private void OnEnable()
    {
        AllObjects = new GameObject[4][];
        AllObjects[0] = FoodObjects;
        AllObjects[1] = FlowerObjects;
        AllObjects[2] = GiftObjects;
        AllObjects[3] = RubbishObjects;

        NewRandom();
    }

    private void Start()
    {
        audioScript = GameObject.Find("Audio").GetComponent<AudioHandler>();
    }

    private void Update()
    {
        if (startGame)
        {
            timer += Time.deltaTime;
            if (timer >= thisWait)
            {
                CreateNewObject();
            }
        }
    }

    public void CreateNewObject()
    {
        //Create Object
        GameObject newObject = new GameObject("ObjectRoot");
        newObject.transform.position = transform.position;

        //Add Scripts
        ObjectScript newScript = newObject.AddComponent<ObjectScript>();
        newScript.spawner = this;

        //Reset Timer
        timer = 0;
        NewRandom();

        audioScript.PlaySFX(audioScript.NewItem);
    }

    private void NewRandom()
    {
        thisWait = Random.Range(minWait, maxWait);
    }

}
