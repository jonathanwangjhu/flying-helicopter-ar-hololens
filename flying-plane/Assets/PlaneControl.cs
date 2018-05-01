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
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

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
        //if (canControl)
        //{
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
            transform.Rotate(Vector3.down * 45 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(forceright, ForceMode.Acceleration);
            if (transform.rotation.z > -0.25f)
            {
                transform.Rotate(Vector3.back * 45 * Time.deltaTime);
            }
            transform.Rotate(Vector3.up * 45 * Time.deltaTime);
        }

        if (canControl)
        {
            controllerInput.Update();
            moveLeftRight();
            addForceUp();
            addForceDown();
            moveForwardBack();
            dropWater();
        }
        //slowly normalize rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);

        //gravity (usegravity was working weird so I just added it myself)
       // rb.AddForce(forcedown, ForceMode.Acceleration);
        //}
    }

    void Update()
    {

    }

    private void moveForwardBack()
    {
        if (controllerInput.GetAxisLeftThumbstickY() > 0)
        {
            rb.AddForce(forceforward, ForceMode.Acceleration);
            if (transform.rotation.x < 0.25f)
            {
                transform.Rotate(Vector3.right * controllerInput.GetAxisLeftThumbstickY() * 45 * Time.deltaTime);
            }
        }
        else if (controllerInput.GetAxisLeftThumbstickY() < 0)
        {
            rb.AddForce(forceback, ForceMode.Acceleration);
            if (transform.rotation.x > -0.25f)
            {
                transform.Rotate(Vector3.left * -controllerInput.GetAxisLeftThumbstickY() * 45 * Time.deltaTime);
            }
        }
    }

    private void moveLeftRight()
    {
        if (controllerInput.GetAxisLeftThumbstickX() > 0)
        {
            rb.AddForce(forceright, ForceMode.Acceleration);
            if (transform.rotation.z > -0.25f)
            {
                transform.Rotate(Vector3.back * controllerInput.GetAxisLeftThumbstickX() * 45 * Time.deltaTime);
            }
            transform.Rotate(Vector3.up * 45 * Time.deltaTime);
        }
        else if (controllerInput.GetAxisLeftThumbstickX() < 0)
        {
            rb.AddForce(forceleft, ForceMode.Acceleration);
            if (transform.rotation.z < 0.25f)
            {
                transform.Rotate(Vector3.forward * -controllerInput.GetAxisLeftThumbstickX() * 45 * Time.deltaTime);
            }
            transform.Rotate(Vector3.down * 45 * Time.deltaTime);
        }
    }

    private void addForceUp()
    {
        rb.AddForce(forceup * 2 * controllerInput.GetAxisRightTrigger(), ForceMode.Acceleration);
    }

    private void addForceDown()
    {
        rb.AddForce(forcedown * 2 * controllerInput.GetAxisLeftTrigger(), ForceMode.Acceleration);
    }

    private void dropWater()
    {
        if ((controllerInput.GetButton(ControllerButton.A) || Input.GetKey(KeyCode.Space)) && canDropWater)
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit something " + collision.gameObject.name);
        if (collision.gameObject.name != "WaterDrop")
        {
            gameController.gameOver("You crashed!");
        }
    }
}
