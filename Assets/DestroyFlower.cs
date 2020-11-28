using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlower : MonoBehaviour
{
    private void Start()
    {
       //Debug.Log("Destroy Flower Script enabled!");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.collider.gameObject.GetComponent<FlowerPart>() != null)
        {

            //Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigggggg");
    }
}
