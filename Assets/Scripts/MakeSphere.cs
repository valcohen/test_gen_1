using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSphere : MonoBehaviour {
    public Transform    baseObject;
    public int          steps;
    public int          timeScale = 1;

    public Vector3 xScale = new Vector3 (1.00f, 0.01f, 0.01f);
    public Vector3 yScale = new Vector3 (0.01f, 1.00f, 0.01f);
    public Vector3 zScale = new Vector3 (0.01f, 0.01f, 1.00f);

    public float sphereScaleIncrement = 0.01f;

    // private members
    private static Vector3 xAxis = new Vector3(1, 0, 0);
    private static Vector3 yAxis = new Vector3(0, 1, 0);
    private static Vector3 zAxis = new Vector3(0, 0, 1);

    private GameObject sphere;
    private Vector3 sphereStartScale = new Vector3 (0.01f, 0.01f, 0.01f);
    private Vector3 sphereScaleLimit = new Vector3(1f, 1f, 1f);

    // Use this for initialization
	void Start () {
        Vector3     initialPos = new Vector3 (0, 0, 0);
        float       interval = 180f / steps;    // 180 = half-circle to avoid drawing on top of ourselves

        genCubesOnAxis (initialPos, interval, xAxis, zScale);
        genCubesOnAxis (initialPos, interval, yAxis, xScale);
        genCubesOnAxis (initialPos, interval, zAxis, yScale);
	
        genSphere (initialPos);


	}

    void genCubesOnAxis (Vector3 initialPos, float interval, Vector3 axis, Vector3 scale)
    {
        Quaternion  rotation;
        GameObject  newObject;

        for (int i = 0; i < steps; i++) {
            float angle = i * interval;
            rotation = Quaternion.AngleAxis (angle, axis);

            newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.position        = initialPos;
            newObject.transform.localScale      = scale;
            newObject.transform.localRotation   = rotation;

            newObject.transform.SetParent (baseObject, false);  // worldPosStayes false = keep local pos
            // newObject = Instantiate (baseObject, initialPos, rotation);

            // setColor
            float h, s, v;
            h = angle / 180f;
            s = 1f;
            v = 1f;
            Color newColor = Color.HSVToRGB (h, s, v);
            Material material = newObject.GetComponent<Renderer> ().material;
            material.color = newColor;
            UnityEngine.Debug.Log (string.Format ("interval: {0}, rot: {1}, hue: {2}", interval, angle, h));
            UnityEngine.Debug.Log (string.Format ("cube: {0}", newObject.transform.localRotation));
        }
    }

    void genSphere (Vector3 initialPos)
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position       = initialPos;
        sphere.transform.localScale     = sphereStartScale;
        sphere.transform.localRotation  = new Quaternion();

        sphere.transform.SetParent (baseObject, false);  // worldPosStayes false = keep local pos

        Renderer sphereRenderer = sphere.GetComponent<Renderer> ();

        Color newColor = Color.HSVToRGB (0, 1f, 1f);
        Material material = sphereRenderer.material;
        material.color = newColor;
        sphereRenderer.receiveShadows = true;
    }
	
	// Update is called once per frame
	void Update () {
        float interval = timeScale * Time.deltaTime;

        // Rotate the object around the local axes at <timeScale> degrees per second
        baseObject.transform.Rotate(Vector3.right   * interval);    // X axis
        baseObject.transform.Rotate(Vector3.up      * interval);    // Y 
        baseObject.transform.Rotate(Vector3.forward * interval);    // Z

        // setColor
        Material material = sphere.GetComponentInParent<Renderer>().material;
        float h, s, v;

        Color.RGBToHSV (material.color, out h, out s, out v);
        h += interval / 100; 
        s = 1f;
        v = 1f;
        material.color = Color.HSVToRGB (h, s, v);

        Vector3 newScale = sphere.transform.localScale + new Vector3(
            sphereScaleIncrement, sphereScaleIncrement, sphereScaleIncrement
        );

        bool grow = newScale.magnitude < sphereScaleLimit.magnitude;
        if (grow) {
        
        } else {
        
        }

        sphere.transform.localScale = (newScale.magnitude < sphereScaleLimit.magnitude)
                                    ? newScale
                                    : sphereStartScale;


        UnityEngine.Debug.Log (string.Format ("interval: {0}, hue: {0}", interval, h));



	}
}
