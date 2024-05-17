using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSpheres : MonoBehaviour
{

    public static float randomRotationStrength;
    private float floatingPosition;
    public static float FloatStrength;


    // Start is called before the first frame update
    void Start()
    {


        floatingPosition = transform.position.y - 5;
        FloatStrength = 0.1f;
        randomRotationStrength = 0.1f;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if (transform.position.y < floatingPosition)
            {
                //Debug.Log("force being applied upwards to move object up");
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * FloatStrength);
                transform.Rotate(randomRotationStrength, randomRotationStrength, randomRotationStrength);
            }
            if (transform.position.y >= floatingPosition)
            {
                //Debug.Log("force applied is less than the gravitational force so that the object comes down. Here mass of object is 2.  ");
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * -1 * FloatStrength);
                transform.Rotate(randomRotationStrength, randomRotationStrength, randomRotationStrength);
            }
    }
}
