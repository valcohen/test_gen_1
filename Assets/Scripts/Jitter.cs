using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour {

    public float radius = 0.25f;

    private static System.Random rnd = new System.Random();

    // Use this for initialization
	void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
        var p = this.transform.position;

        var moveX = (float)rnd.NextDouble() * radius;
        var moveY = (float)rnd.NextDouble() * radius;
        var moveZ = (float)rnd.NextDouble() * radius;

        bool addX = (rnd.Next(0, 100) < 50);
        bool addY = (rnd.Next(0, 100) < 50);
        bool addZ = (rnd.Next(0, 100) < 50);

        var jitter = new Vector3 (
                         addX ? moveX : -moveX,
                         addY ? moveY : -moveY,
                         addZ ? moveZ : -moveZ
        );
        
        this.transform.Translate (jitter);
    }
}
