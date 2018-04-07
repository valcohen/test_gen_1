using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Walkers : MonoBehaviour {

    public static List<Transform>  walkers;

	public byte         rows = 10;
    public byte         cols = 10;
    public float        rowSpace = 3;
    public float        colSpace = 3;
	public Transform    walkerPrefab;
    public Vector3      initialGoal;
    public Color        initialColor;

    void Awake()
	{
        UnityEngine.Debug.Log ("starting...");
        
        var watch = new Stopwatch ();
        watch.Start();

        walkers = new List<Transform>(rows * cols);

        var startX = transform.position.x;
        var startY = transform.position.y; // 1f;
        var startZ = transform.position.z;

        var rot = new Quaternion();
        var currentGoal  = initialGoal;
        var currentColor = initialColor;
        var rnd = new System.Random();

        var interval = 1.0f / cols;

		for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {

                // setInitialPosition -- line up in rows & cols
                // TODO: extract to named function, support plugging in diff. initial position arrangements
                var x = startX + (c * colSpace);
                var z = startZ + (r * rowSpace);

                var pos     = new Vector3 (x, startY, z);
                var walker  = Instantiate (walkerPrefab, pos, rot, this.transform);

                var moveTo  = walker.GetComponentInParent<MoveTo> ();
                moveTo.goal = currentGoal;

                // setColor
                float h, s, v;

                // Color.RGBToHSV (currentColor, out h, out s, out v);

                h = (currentGoal.x + 50f) / 100f;   // change hue by destination. x = +/-50, so +50 to get 0-99, then normalize to 0-1
                // h = c * interval;    // change hue by column index
                s = 1f;
                v = 1f;

                Color newColor = Color.HSVToRGB (h, s, v);
                currentColor = newColor;

                Material material = walker.GetComponentInParent<Renderer>().material;
                material.color = currentColor;

                // setNextGoal
                // new goals is anywhere on X and +/-2.5 of the initial goal's Z
                // TODO: extract to a named function, support plugging in diff. goal functions
                float randX = rnd.Next (1, 100);
                float randZ = rnd.Next (1, 5000) / 1000f; // magnify up to get more precision, then divide down to 1.000 - 5.000

                float goalX = randX - 50f;      // shift -50 to get +/-50 to match map coords
                float goalZ = randZ - 2.5f;     // +/- 5 Z offset

                Vector3 newGoal = new Vector3( goalX, initialGoal.y, initialGoal.z + goalZ );
                UnityEngine.Debug.Log ("walkers: " + newGoal);

                currentGoal = newGoal;

                // keep track of the new walker
                walkers.Add(walker);
            }
		}

        watch.Stop ();
        UnityEngine.Debug.Log (string.Format("generated {0} walkers in {1} milliseconds", walkers.Count, watch.ElapsedMilliseconds));

	}
}
