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

    public GameObject WaterDrop;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerInput = new ControllerInput(0, 0.19f);
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            //rb.transform.Translate(Vector3.forward * speed);

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
                rb.velocity += Vector3.forward * accelerationSpeed;
                //if (transform.rotation.eulerAngles.z > 180)
                //{
                //    rb.transform.Rotate(Vector3.right * (Mathf.Sin((-270 + transform.rotation.eulerAngles.z) * Mathf.PI / 180)));
                //    rb.transform.Rotate(Vector3.up * (Mathf.Sin((360 - transform.rotation.eulerAngles.z) * Mathf.PI / 180)));
                //}


            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.transform.Translate(Vector3.back * accelerationSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                //transform.Rotate(Vector3.down * 45 * Time.deltaTime);
                // if (transform.rotation.z < 0.2f)
                //{
                //    Debug.Log("z value is: " + transform.rotation.eulerAngles.z);
                //    transform.Rotate(Vector3.forward * 45 * Time.deltaTime);
                //}
                rb.AddForce(Vector3.left * accelerationSpeed, ForceMode.Impulse);

            }

            if (Input.GetKey(KeyCode.D))
            {
                //transform.Rotate(Vector3.up * 45 * Time.deltaTime);
                if (transform.rotation.z > -0.2f)
                {
                    Debug.Log("z value is: " + transform.rotation.eulerAngles.z);
                    transform.Rotate(Vector3.back * 45 * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Rotate(Vector3.left * 45 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(Vector3.right * 45 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.forward * 45 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.back * 45 * Time.deltaTime);
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
}
