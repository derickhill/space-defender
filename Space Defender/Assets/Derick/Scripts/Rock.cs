using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rock : MonoBehaviour
{
    public static GameObject[] spheres;
    int rnd;
    // Start is called before the first frame update
    void Start()
    {
        spheres = PlayerControllerNew.spheresStatic;

        System.Random random = new System.Random();
        rnd = random.Next(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        

        var step = 15 * Time.deltaTime; // calculate distance to move
        transform.Rotate(0.01f, 0.01f, 0.01f);
        transform.position = Vector3.MoveTowards(transform.position, spheres[rnd].transform.position, step); ;

        //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0) - transform.position * step);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        FractureObject();

    }

    public void FractureObject()
    {
        Destroy(gameObject); //Destroy the object to stop it getting in the way
    }
}
