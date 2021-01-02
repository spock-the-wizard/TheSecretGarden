using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteSystem : MonoBehaviour
{
    private List<Flower> flowersToDelete = new List<Flower>();
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("trigger with the trashbin");

        if (collision.gameObject.GetComponent<Flower>() != null)
        {
            flowersToDelete.Add(collision.gameObject.GetComponent<Flower>());

            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
    
    public void undoDelete()
    {
        if (flowersToDelete.Count == 0)
        {
            return;
        }
        Flower recentFlower = flowersToDelete[flowersToDelete.Count - 1];
        recentFlower.gameObject.transform.position = new Vector3(-9.0f, 3.0f, -19.0f);
        recentFlower.gameObject.SetActive(true);
        flowersToDelete.Remove(recentFlower);

    }
    public void deleteFlowerFilesInBin()
    {
        for(int i = 0; i < flowersToDelete.Count; i++)
        {
            string path = Application.persistentDataPath + "/" + flowersToDelete[i].flowerName + ".flw";
            if (File.Exists(path))
                File.Delete(path);
            else
                Debug.LogError("file for "+flowersToDelete[i]+" already gone!");

      
        }
        Debug.Log("Deleted " + flowersToDelete.Count + " flowers");
        flowersToDelete.Clear();
    }
}
