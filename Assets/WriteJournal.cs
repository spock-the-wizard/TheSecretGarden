using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteJournal : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionrEnter(Collider other)
    {
        Debug.Log("trigger with write object");

        if (other.gameObject.tag == "Flower")
        {
            Flower flw = other.gameObject.GetComponent<Flower>();
            flw.text = "a new string goes here";
        }
    }

}
