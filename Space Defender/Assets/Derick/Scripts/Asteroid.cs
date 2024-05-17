using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public static float randomRotationStrength;
    private float floatingPosition;
    public static float FloatStrength;
    public Material[] mats;
    public int health;
    public GameObject minimap;
    public Material mat;
    public static int? dead;
    bool destroyed;
    public AudioSource exp;

    public Light light;

    // Start is called before the first frame update
    void Start()
    {
        if (!dead.HasValue)
            dead = 0;

        floatingPosition = transform.position.y - 5;
        FloatStrength = 0.1f;
        randomRotationStrength = 0.1f;

        light = GetComponent<Light>();

        health = 4;

        destroyed = false;
        if(minimap != null)
            exp.volume = 0.168f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        System.Random random = new System.Random();

        /* randomRotationStrength1 = (float) random.NextDouble();
         randomRotationStrength2 = (float) random.NextDouble(); 
         randomRotationStrength3 = (float) random.NextDouble(); */
        if (health >= 0)
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
        else
        {
            if(!destroyed)
            {
                destroyed = false;
                dead++;
                GetComponent<MeshRenderer>().material = mat;
                light.enabled = false;
                GetComponent<SphereCollider>().enabled = false;
            }

            //transform.SetParent(null);
            //transform.GetComponent<Rigidbody>().useGravity = true;
            //transform.GetComponent<Rigidbody>().AddForce(-1 * 20 * Vector3.up);
        }

        if (minimap != null && health >= 0)
            minimap.GetComponent<MeshRenderer>().material = mats[health];
    }

    void OnCollisionEnter(Collision collision)
    {
        health -= 1;
        exp.Play();
        if (health < 0 && minimap != null)
        {
            PlayerControllerNew.howManyDead++;
            
            minimap.SetActive(false);
        }
    }
}
