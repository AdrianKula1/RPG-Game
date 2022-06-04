using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyType
{
    public abstract float[] GetTypeBaseStats();
    public abstract string[] GetTypeAnimationNames();
}
