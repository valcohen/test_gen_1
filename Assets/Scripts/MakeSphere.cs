using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class MakeSphere : MonoBehaviour {
    public Transform    baseObject;
    public int          steps;
    public int          timeScale = 1;

    public Vector3 objectScale = new Vector3 (0.01f, 1.00f, 0.01f);

    // use these for cubes: instead of rotating, stretch along one axis
    public Vector3 xScale = new Vector3 (1.00f, 0.01f, 0.01f);
    public Vector3 yScale = new Vector3 (0.01f, 1.00f, 0.01f);
    public Vector3 zScale = new Vector3 (0.01f, 0.01f, 1.00f);

    public float sphereScaleIncrement = 0.01f;

    // private members
    private static Vector3 xAxis = new Vector3(360, 0, 0);
    private static Vector3 yAxis = new Vector3(0, 120, 0);
    private static Vector3 zAxis = new Vector3(0, 0, 120);

    private static Vector3 Ninety = new Vector3(90, 90, 90);

    private GameObject sphere;
    private Vector3 sphereStartScale = new Vector3 (0.01f, 0.01f, 0.01f);
    private Vector3 sphereScaleLimit = new Vector3(1f, 1f, 1f);

    // Use this for initialization
	void Start () {
        Vector3     initialPos = new Vector3 (0, 0, 0);
        float       interval = 180f / steps;    // 180 = half-circle to avoid drawing on top of ourselves

        var disc = genCubesOnAxis (initialPos, interval, xAxis, objectScale);
        // genCubesOnAxis (initialPos, interval, yAxis, objectScale);
        // genCubesOnAxis (initialPos, interval, zAxis, objectScale);
	
        for (int i = 0; i < steps; i++) {
            float angle = i * interval;
            Instantiate (disc, baseObject.position, Quaternion.AngleAxis (angle, baseObject.up), baseObject);
        }


        genSphere (initialPos);


	}

    GameObject genCubesOnAxis (Vector3 initialPos, float interval, Vector3 axis, Vector3 scale)
    {
        GameObject baseObj = new GameObject ("base");
        baseObj.transform.position = initialPos;
        baseObj.transform.SetParent (baseObject.transform, false);


        Quaternion  rotation;
        GameObject  newObject;

        for (int i = 0; i < steps; i++) {
            float angle = i * interval;
            rotation = Quaternion.AngleAxis (angle, axis) ;

            newObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            newObject.transform.position        = initialPos;
            newObject.transform.localScale      = scale;
            newObject.transform.localRotation   = rotation;

            newObject.transform.SetParent (baseObj.transform, false);  // worldPositionStays false = keep local pos
            // newObject = Instantiate (baseObject, initialPos, rotation);

            // setColor
            float h, s, v;
            h = angle / 180f;
            s = 1f;
            v = 1f;
            Color newColor = Color.HSVToRGB (h, s, v);
            Material material = newObject.GetComponent<Renderer> ().material; //.sharedMaterial; - works in Edit, but shares colors
            material.color = newColor;
            UnityEngine.Debug.Log (string.Format ("interval: {0}, rot: {1}, hue: {2}", interval, angle, h));
            UnityEngine.Debug.Log (string.Format ("cube: {0}", newObject.transform.localRotation));

        }
        return baseObj;
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
        Material material = sphereRenderer.material; //.sharedMaterial; - works in Edit, but shares colors
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

        // set sphere Color

        Material material = sphere.GetComponentInParent<Renderer>().material; //.sharedMaterial; - works in Edit, but shares colors
        float h, s, v;

        Color.RGBToHSV (material.color, out h, out s, out v);
        h += interval / 10; 
        s = 1f;
        v = 1f;
        material.color = Color.HSVToRGB (h, s, v);

        Vector3 newScale = sphere.transform.localScale + new Vector3(
            sphereScaleIncrement, sphereScaleIncrement, sphereScaleIncrement
        );

        bool grow = newScale.magnitude < objectScale.magnitude;
        if (grow) {
        
        } else {
        
        }
        

        sphere.transform.localScale = (newScale.y < objectScale.y)
                                    ? newScale
                                    : sphereStartScale;

	}
}
