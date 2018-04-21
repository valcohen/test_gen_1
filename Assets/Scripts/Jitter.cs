using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour {

    public bool  move = true;
    public float radius = 0.25f;

    public bool rotate = true;
    public float maxAngle = 15;

    public bool pulse = true;
    public float interval = 2;


    private static System.Random rnd = new System.Random();
    private Vector3 initialPosition;

    // Use this for initialization
	void Start () {
        initialPosition = this.transform.position;

        if (pulse) {
            InvokeRepeating ("reset", interval, interval);
        }
    }
	
	// Update is called once per frame
	void Update () {
        bool addX = (rnd.Next(0, 100) < 50);
        bool addY = (rnd.Next(0, 100) < 50);
        bool addZ = (rnd.Next(0, 100) < 50);

        if (move) {
            var moveX = (float)rnd.NextDouble() * radius;
            var moveY = (float)rnd.NextDouble() * radius;
            var moveZ = (float)rnd.NextDouble() * radius;

            this.transform.Translate (
                addX ? moveX : -moveX,
                addY ? moveY : -moveY,
                addZ ? moveZ : -moveZ
            );
        }

        if (rotate) {
            var rotX = (float)rnd.NextDouble() * maxAngle;
            var rotY = (float)rnd.NextDouble() * maxAngle;
            var rotZ = (float)rnd.NextDouble() * maxAngle;

            this.transform.Rotate (
                addX ? rotX : -rotX,
                addY ? rotY : -rotY,
                addZ ? rotZ : -rotZ
            );
        }
    }

    private void reset () {
        this.transform.position = initialPosition;
        this.transform.rotation = Quaternion.identity;
    }

}
