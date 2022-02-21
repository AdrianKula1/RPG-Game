using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int getLayerNumber(string layerName)
    {
        int layerNumber = 0;
        int layer = LayerMask.GetMask(layerName);
        while (layer > 0)
        {
            layer >>= 1;
            layerNumber++;
        }
        return layerNumber -1;
    }
}
