using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteSystem : MonoBehaviour
{
    private List<string> flowersToDelete = new List<string>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger with the trashbin");

        if (other.gameObject.GetComponent<Flower>() != null)
        {
            flowersToDelete.Add(other.GetComponent<Flower>().flowerName);

            Destroy(other.gameObject);
        }
    }

    public void deleteFlowerFilesInBin()
    {
        for(int i = 0; i < flowersToDelete.Count; i++)
        {
            string path = Application.persistentDataPath + "/" + flowersToDelete[i] + ".flw";
            if (File.Exists(path))
                File.Delete(path);
            else
                Debug.LogError("file for "+flowersToDelete[i]+" already gone!");

      
        }
        Debug.Log("Deleted " + flowersToDelete.Count + " flowers");
        flowersToDelete.Clear();
    }
}
