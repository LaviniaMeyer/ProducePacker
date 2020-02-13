using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuteTele : MonoBehaviour
{
    public Transform spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        ControllerSelection.ObjectInteraction script = other.transform.root.GetComponentInChildren<ControllerSelection.ObjectInteraction>();
        script.death = false;
        other.transform.position = spawnPos.position;
        other.transform.root.GetComponentInChildren<Rigidbody>().isKinematic = false;
    }
}
