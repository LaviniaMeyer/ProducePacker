using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    public GameObject ConveyorPrefab;
    public Transform ConveyorStart;

    public float PartDiameter;
    public float ConveyorLength;

    private void OnEnable()
    {
        SpawnConveyor();
    }

    private void SpawnConveyor()
    {
        float total = ConveyorLength/PartDiameter;
        int whole = Mathf.FloorToInt(total);

        for (int i = 1; i < whole; i++)
        {
            // Instantiate the next part to the right of this one
            Vector3 newPos = ConveyorStart.TransformPoint(Vector3.right * (PartDiameter * i));
            Instantiate(ConveyorPrefab, newPos, ConveyorStart.rotation, ConveyorStart);
        }


    }


}
