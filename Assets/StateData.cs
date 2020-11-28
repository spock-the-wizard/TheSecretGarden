using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class StateData
{
    [SerializeField]
    public int flowerCnt;

    public StateData(int cnt)
    {
        flowerCnt = cnt;
    }

    public int GetFlowerNum()
    {
        return flowerCnt;
    }

}
