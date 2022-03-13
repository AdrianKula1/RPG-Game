using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour, IDestructableObject
{
    // Start is called before the first frame update
    private float hp = 100;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(50);
        }
    }

    public void takeDamage(float damage)
    {
        hp -= 50;
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}