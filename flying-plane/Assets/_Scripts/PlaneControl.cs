using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensXboxController;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{
    private bool canControl;
    private bool canDropWater;

    private Rigidbody rb;
    private ControllerInput controllerInput;
    
    private Vector3 forceforward;
    private Vector3 forceback;
    private Vector3 forceleft;
    private Vector3 forceright;
    private float currentYaw;

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

        canControl = true;
        canDropWater = true;
        
        currentYaw = 0;
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            forceforward = new Vector3(transform.forward.x, 0, transform.forward.z);
            forceback = -1 * forceforward;
            forceright = new Vector3(transform.right.x, 0, transform.right.z);
            forceleft = -1 * forceright;

            // xbox controls
            controllerInput.Update();
            moveLeftRight();
            addForceUp();
            addForceDown();
            moveForwardBack();
            adjustPitch();
            dropWater();
        }

        //slowly normalize rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, currentYaw, 0), Time.deltaTime);
    }

    private void moveForwardBack()
    {
        // xbox control
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

        // keyboard control
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
    }

    private void moveLeftRight()
    {
        // xbox control
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

        // keyboard control
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(forceright, ForceMode.Acceleration);
            if (transform.rotation.z > -0.25f)
            {
                transform.Rotate(Vector3.back * 45 * Time.deltaTime);
            }
            transform.Rotate(Vector3.up * 45 * Time.deltaTime);
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
    }

    private void addForceUp()
    {
        // xbox control
        rb.AddForce(Vector3.up * 2 * controllerInput.GetAxisRightTrigger(), ForceMode.Acceleration);
        
        // keyboard control
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * 2, ForceMode.Acceleration);
        }
    }

    private void addForceDown()
    {
        // xbox control
        rb.AddForce(Vector3.down * 2 * controllerInput.GetAxisLeftTrigger(), ForceMode.Acceleration);

        // keyboard control
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * 2, ForceMode.Acceleration);
        }
    }

    private void adjustPitch()
    {
        // xbox control
        if (controllerInput.GetAxisRightThumbstickX() > 0)
        {
            transform.Rotate(Vector3.up * controllerInput.GetAxisRightThumbstickX() * 45 * Time.deltaTime);
            currentYaw += controllerInput.GetAxisRightThumbstickX() * 45 * Time.deltaTime;
        }
        else if (controllerInput.GetAxisRightThumbstickX() < 0)
        {
            transform.Rotate(Vector3.down * controllerInput.GetAxisRightThumbstickX() * 45 * Time.deltaTime);
            currentYaw -= controllerInput.GetAxisRightThumbstickX() * 45 * Time.deltaTime;
        }

        // keyboard control
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 45 * Time.deltaTime);
            currentYaw += 45 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * 45 * Time.deltaTime);
            currentYaw -= 45 * Time.deltaTime;
        }
    }

    private void dropWater()
    {
        if ((controllerInput.GetButton(ControllerButton.A) || Input.GetKey(KeyCode.Space)) && canDropWater)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-0.05f, -0.01f), transform.position.y , transform.position.z + Random.Range(-0.02f, 0.02f));
            GameObject waterDrop = Instantiate(WaterDrop, position, Quaternion.identity);
            Physics.IgnoreCollision(waterDrop.GetComponent<Collider>(), GetComponent<Collider>());
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
        Debug.Log("plane hit something " + collision.gameObject.name);
        gameController.gameOver("You crashed!");
    }
}
