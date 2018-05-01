using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{
    private bool canControl = true;
    private bool canDropWater = true;

    private Rigidbody rb;
    private ControllerInput controllerInput;

    public float RotateAroundYSpeed = 2.0f;
    public float RotateAroundXSpeed = 2.0f;
    public float RotateAroundZSpeed = 2.0f;
    public float accelerationSpeed = .01f;
    public float topSpeed = 1.0f;
    public float minSpeed = 0.25f;
    public float speed = 0;
    public float accel = .5f;
    public Vector3 forceup;
    public Vector3 forcedown;
    public Vector3 forceforward;
    public Vector3 forceback;
    public Vector3 forceleft;
    public Vector3 forceright;

    public GameObject WaterDrop;


    private GameController gameController;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerInput = new ControllerInput(0, 0.19f);

        //(PT) set these here so they aren't affected by rotation later
        forceup = Vector3.up;
        forcedown = Vector3.down;
        forceforward = Vector3.forward;
        forceback = Vector3.back;
        forceright = Vector3.right;
        forceleft = Vector3.left;
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            //move up
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(forceup * 2, ForceMode.Acceleration);
            }

            //move forward, backward, left, and right
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(forceforward, ForceMode.Acceleration);
                if (transform.rotation.x < 0.25f)
                {
                    transform.Rotate(Vector3.right * 45 * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(forceback, ForceMode.Acceleration);
                if (transform.rotation.x > -0.25f)
                {
                    transform.Rotate(Vector3.left * 45 * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(forceleft, ForceMode.Acceleration);
                if (transform.rotation.z < 0.25f)
                {
                    transform.Rotate(Vector3.forward * 45 * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(forceright, ForceMode.Acceleration);
                if (transform.rotation.z > -0.25f)
                {
                    transform.Rotate(Vector3.back * 45 * Time.deltaTime);
                }
            }

            //slowly normalize rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);

            //gravity (usegravity was working weird so I just added it myself)
            rb.AddForce(forcedown, ForceMode.Acceleration);
        }
    }
    
    void Update()
    {
        if (canControl)
        {
            controllerInput.Update();
            if (Input.GetKey(KeyCode.W))
            {
                //rb.transform.Translate(Vector3.forward * accelerationSpeed);
                //rb.AddForce(forceup * 3, ForceMode.Acceleration);
                //if (transform.rotation.eulerAngles.z > 180)
                //{
                //    rb.transform.Rotate(Vector3.right * (Mathf.Sin((-270 + transform.rotation.eulerAngles.z) * Mathf.PI / 180)));
                //    rb.transform.Rotate(Vector3.up * (Mathf.Sin((360 - transform.rotation.eulerAngles.z) * Mathf.PI / 180)));
                //}


            }

            if (Input.GetKey(KeyCode.S))
            {
                //rb.transform.Translate(Vector3.back * accelerationSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                //transform.Rotate(Vector3.down * 45 * Time.deltaTime);
                // if (transform.rotation.z < 0.2f)
                //{
                //    Debug.Log("z value is: " + transform.rotation.eulerAngles.z);
                //    transform.Rotate(Vector3.forward * 45 * Time.deltaTime);
                //}
                // rb.AddForce(Vector3.left * accelerationSpeed, ForceMode.Impulse);

            }

            if (Input.GetKey(KeyCode.D))
            {
                //transform.Rotate(Vector3.up * 45 * Time.deltaTime);
                //if (transform.rotation.z > -0.2f)
                //{
                //    Debug.Log("z value is: " + transform.rotation.eulerAngles.z);
                //    transform.Rotate(Vector3.back * 45 * Time.deltaTime);
                //}
            }
            
            translateRotateScale();
            translateForward();
            dropWater();
        }
    }

    private void translateRotateScale()
    {

        float rotateAroundY = RotateAroundYSpeed * controllerInput.GetAxisLeftThumbstickX();
        float rotateAroundX = RotateAroundXSpeed * controllerInput.GetAxisLeftThumbstickY();
        float rotateAroundZ = RotateAroundZSpeed * controllerInput.GetAxisRightThumbstickX();
        this.transform.Rotate(rotateAroundX, rotateAroundY, -rotateAroundZ);
    }

    private void translateForward()
    {
        rb.transform.Translate(Vector3.forward * (controllerInput.GetAxisRightTrigger() - controllerInput.GetAxisLeftTrigger()) * accelerationSpeed);

        if (rb.velocity.magnitude > topSpeed)
        {
            rb.velocity = rb.velocity.normalized * topSpeed;
        }

        if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
    }

    private void dropWater()
    {
        if (controllerInput.GetButton(ControllerButton.A) || Input.GetKey(KeyCode.Space) && canDropWater)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-0.02f, 0.02f), transform.position.y, transform.position.z + Random.Range(-0.02f, 0.02f));
            Instantiate(WaterDrop, position, Quaternion.identity);
        }
    }

    public void stopControl()
    {
        canControl = false;
    }

    public void stopDroppingWater()
    {
        canDropWater = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SpatialMapping")
        {
            gameController.gameOver("You crashed!");
        }

        Debug.Log("target hit by " + other.gameObject.name);

    }

    private void OnCollisionEnter(Collision collision)
    {
        gameController.gameOver("You crashed!");
    }
}
