using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private List<GameObject> colliding = new List<GameObject>();
    public float speed = 0.03f;
    public GameObject poofParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interactable")
            colliding.Add(collision.gameObject);
    }

    void Update()
    {
        for (int i = 0; i < colliding.Count; i++)
        {
            if (colliding[i] != null)
            {
                if (colliding[i].transform.position.x > -3)
                    colliding[i].transform.root.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                else
                    colliding[i].transform.root.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
            }
            else
            {
                colliding.RemoveAt(i);
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding.Remove(collision.gameObject);
    }

    public void DestroyAll()
    {
        for (int i = 0; i < colliding.Count; i++)
        {
            Vector3 pos = colliding[i].transform.position;
            Destroy(colliding[i].transform.root.gameObject);
            Instantiate(poofParticle, pos, poofParticle.transform.rotation);
        }
        colliding.Clear();
    }
}
