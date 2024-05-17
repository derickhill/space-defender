using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPoint : MonoBehaviour
{
    public GameObject asteroid;
    private float T;
    public Material mat;


    System.Random rnd;
    // Start is called before the first frame update
    void Start()
    {
        //spheres = new GameObject[10];
        rnd = new System.Random();

        T = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerControllerNew.playing)
            return;
        T += Time.deltaTime;

        if (T > 15 + rnd.NextDouble() * 20)
        {
            T = 0;

            GameObject newAsteroid = Instantiate(asteroid, gameObject.transform.localPosition, new Quaternion(0, 0, 0, 0));
            GameObject minimapAsteroid = Instantiate(asteroid, gameObject.transform.localPosition, new Quaternion(0, 0, 0, 0));
            minimapAsteroid.GetComponent<MeshRenderer>().material = mat;
            int minimap = LayerMask.NameToLayer("Minimap");
            minimapAsteroid.layer = minimap;
            minimapAsteroid.GetComponent<CapsuleCollider>().enabled = false;
            minimapAsteroid.transform.SetParent(newAsteroid.transform);
            Debug.Log("Asteroid incoming");
        }


    }
}
