using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{

    private Rigidbody rb;

    private ControllerInput controllerInput;

    public float RotateAroundYSpeed = 2.0f;
    public float RotateAroundXSpeed = 2.0f;
    public float RotateAroundZSpeed = 2.0f;
    public float accelerationSpeed = .01f;
    public float topSpeed = 1.0f;
    public float minSpeed = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerInput = new ControllerInput(0, 0.19f);
    }

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.transform.Translate(Vector3.forward * accelerationSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.transform.Translate(Vector3.forward * accelerationSpeed);
        }


    }
    
    void Update()
    {
        controllerInput.Update();

         if (Input.GetKey(KeyCode.A))
         {
             transform.Rotate(Vector3.down * 45 * Time.deltaTime);

         }

         if (Input.GetKey(KeyCode.D))
         {
             transform.Rotate(Vector3.up * 45 * Time.deltaTime);
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

    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(0.0f, -.5f, 2.0f);
    }
}
