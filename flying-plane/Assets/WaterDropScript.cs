using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropScript : MonoBehaviour {

    private GameController gameController;
    private Rigidbody rb;
    public float DeathTime = 3f;
    public float dropAcc = .001f;
    public float maxDropSpeed = 1.5f;
    public int scoreValue = 100;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, DeathTime);

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();

            gameController.reduceWaterGauge();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }
	
	void Update () {
    }

    void FixedUpdate()
    {
        rb.velocity += Vector3.down * dropAcc;
        
        Vector3 vel = rb.velocity;
        if (vel.sqrMagnitude > maxDropSpeed * maxDropSpeed)
        {
            rb.velocity = vel.normalized * maxDropSpeed;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Target Root")
        {
            Debug.Log("Hit target with water");
            gameController.AddScore(scoreValue);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
