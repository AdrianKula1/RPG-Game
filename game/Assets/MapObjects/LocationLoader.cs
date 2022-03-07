using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LocationLoader : MonoBehaviour, ILocation
{
    public string LevelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if(collisionGameObject.name == "Player"){
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
