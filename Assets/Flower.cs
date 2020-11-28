using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public string flowerName;
    public Time time;
    public List<GameObject> parts = new List<GameObject>();
    public Vector3 position;
    public string text;
    // Start is called before the first frame update
  

    //create from scratch in draw mode
    public static GameObject createFlowerObject(int idx)
    {
        GameObject obj = new GameObject("New Flower");

        Flower flower = obj.AddComponent<Flower>();
        flower.flowerName = "test" + idx.ToString();
        flower.position = new Vector3(-7, 3,-19);
        flower.text = "no Text yet";

        return obj;
    }
}
