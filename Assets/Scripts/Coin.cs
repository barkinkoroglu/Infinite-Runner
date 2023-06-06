using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin : MonoBehaviour {

    [SerializeField] float turnSpeed = 90f;
    //public AudioSource coinSound;

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null) {
            Destroy(gameObject);
            return;
        }

        // Check that the object we collided with is the player
        if (other.gameObject.name != "Player") {
            return;
        }

        // Add to the player's score
        GameManager.inst.IncrementScore();

        
        
        // Destroy this coin object
        Destroy(gameObject);

        GameManager.Source.Play();
        
    }

    private void Start () {

	}

	private void Update () {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
	}
}