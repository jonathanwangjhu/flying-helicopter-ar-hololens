using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour {

    // Use this for initialization

    public float speed;
    private Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        /*float moveHorizontal = 0.0f;// Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);*/

        if (Input.GetKey(KeyCode.W)) {
            rb.AddRelativeForce(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.S)) {
            rb.AddRelativeForce(Vector3.back * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeForce(Vector3.left * 0.25f * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(Vector3.right * 0.25f * speed);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * 15 * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 15 * Time.deltaTime);
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
    }
}
