using UnityEngine;
using System.Collections;
using System;

public class PlayerControllerNew : MonoBehaviour
{

    private float moveSpeed = 30; // move speed
    private float distGround; // distance from character position to ground
    private bool jumping = false; // flag "I'm jumping to wall"

    public GameObject[] spheres;
    public static GameObject[] spheresStatic;
    public static int howManyDead;

    public static float distance;
    public static bool playing;

    public GameObject crosshair;
    public AudioSource audioSource1, audioSource2, audioSource3;

    public Camera LookAtCamera;
    public static GameObject LookAtTarget;

    public GameObject map;

    Ray ray;
    RaycastHit hit;

    public Animator anim;

    public Material mat1, mat2, mat3;

    public static float T;

    private void Start()
    {
        spheresStatic = (GameObject[])spheres.Clone();

        LookAtTarget = null;
        GetComponent<Rigidbody>().freezeRotation = true; // disable physics rotation
        distance = 0;
        T = 0;
        howManyDead = 5;
        map.SetActive(false);

        playing = false;
    }

    private void Update()
    {
        if (!playing || howManyDead == 10)
            return;

        T += Time.deltaTime;

        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall      

        // Move character on sphere, left joystick
        float horizontal, vertical;
        horizontal = -Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        transform.Rotate(0, 0, horizontal * moveSpeed * Time.deltaTime);
        transform.Rotate(vertical * moveSpeed * Time.deltaTime, 0, 0);

        anim.SetFloat("Forward", vertical);
        anim.SetFloat("Right", horizontal);

        if(vertical < -.1 || vertical > .1 || horizontal > .1 || horizontal < -.1)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.Play();
            }
        }
       // else { audioSource1.Pause(); }

        // Camera movement, right joystick
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal2"));
        LookAtCamera.transform.Rotate(Vector3.left, Input.GetAxis("Vertical2"));


        if (Physics.Raycast(LookAtCamera.transform.position, LookAtCamera.transform.forward, out hit))
        {
            // Get object looking at
            LookAtTarget = hit.collider.gameObject;

            distance = hit.distance;
                                    
            //Set the new cross hair position based on the distance
            crosshair.transform.position = LookAtCamera.transform.position + (LookAtCamera.transform.forward * distance);
            crosshair.transform.LookAt(LookAtCamera.transform.position);
            crosshair.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

            //Scale the cross hair so it's the same size even when it's in the distance.
            crosshair.transform.localScale = new Vector3(.06f, .06f, .06f) * distance;

            if (distance < 30 && distance > 0)
                crosshair.GetComponent<MeshRenderer>().material = mat1;
            else
                crosshair.GetComponent<MeshRenderer>().material = mat2;

            if (LookAtTarget.CompareTag("Bullet"))
            {
                crosshair.GetComponent<MeshRenderer>().material = mat3;
            }

        }
        else
        {
            distance = 1000;
            crosshair.GetComponent<MeshRenderer>().material = mat2;
            crosshair.transform.position = LookAtCamera.transform.position + (LookAtCamera.transform.forward * distance);
            crosshair.transform.LookAt(LookAtCamera.transform.position);
            crosshair.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

            //Scale the cross hair so it's the same size even when it's in the distance.
            crosshair.transform.localScale = new Vector3(.05f, .05f, .05f) * distance;
        }

        if (Input.GetButtonDown("Map"))
        {
            map.SetActive(true);
        }

        if(Input.GetButtonUp("Map"))
        {
            map.SetActive(false);
        }

        if (Input.GetButtonDown("Shoot"))
        {
            if (Physics.Raycast(LookAtCamera.transform.position, LookAtCamera.transform.forward, out hit))
            {
                // Get object looking at
                LookAtTarget = hit.collider.gameObject;

                if (LookAtTarget.CompareTag("Bullet"))
                {
                    crosshair.GetComponent<MeshRenderer>().material = mat3;
                    
                    audioSource2.Play();

                    crosshair.GetComponent<SphereCollider>().enabled = true;

                    Destroy(LookAtTarget);

                }
            }
            
        }

        if (Input.GetButtonUp("Shoot"))
        {

            crosshair.GetComponent<SphereCollider>().enabled = false;
        }

        if (Input.GetButtonDown("Jump"))
        { // jump pressed:
            if (Physics.Raycast(LookAtCamera.transform.position, LookAtCamera.transform.forward, out hit))
            {
                // Get object looking at
                LookAtTarget = hit.collider.gameObject;

                if (distance < 30.0f)
                {
                    audioSource3.Play();

                    JumpToWall(LookAtTarget.transform.position, hit.normal);

                    transform.SetParent(LookAtTarget.transform, false);
                   
                    float radius = LookAtTarget.transform.localScale.y * 0.5f;
                    GetComponentsInChildren<Rigidbody>()[0].transform.localPosition += new Vector3(0.0f, radius, 0.0f);
                }
            }
        }
    }

    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumping = true; // signal it's jumping to wall
        GetComponent<Rigidbody>().isKinematic = true; // disable physics while jumping
        Vector3 orgPos = transform.position;
        Quaternion orgRot = transform.rotation;
        Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(transform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(orgPos, dstPos, t);
            transform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
        }
        GetComponent<Rigidbody>().isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished
        transform.localPosition = new Vector3(0, 0, 0);
        
    }
}