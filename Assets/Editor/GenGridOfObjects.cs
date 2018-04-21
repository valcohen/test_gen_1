using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenGridOfObjects : ScriptableWizard {

    public Transform prefab;
    public int      rows    = 10;
    public int      columns = 10;
    public int      layers  = 1;
    public float    gap     = 2f;
    public bool     color   = true;
    public float    alpha   = 0.25f;    // 1 == opaque, 0 == clear

    [MenuItem("Custom/Gen Grid of Objects...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard ("Generate Grid of Objects", typeof(GenGridOfObjects), "Create");
    }

    void OnWizardUpdate()
    {

    }

    void OnWizardCreate()
    {
        Vector3     pos     = Vector3.zero;
        Quaternion  rot     = Quaternion.identity;
        Transform   parent  = new GameObject ("gridParent").transform;
        parent.transform.position = pos;
        if (prefab == null) { prefab = Utils.createSphere (parent).transform; }

        Color prefabColor = new Color (0, 0, 0, alpha);  // TODO: limit Alpha
        // normalize color range (0..1): 1 / dimension 
        float xStep = 1.0f / columns; // r
        float yStep = 1.0f / layers;  // g
        float zStep = 1.0f / rows;    // b
        float currentR, currentG, currentB;
        Debug.Log(string.Format("color steps: {0} {1} {2}", xStep, yStep, zStep));

        for (var l = 0; l < layers; l++) {
            for (var r = 0; r < rows; r++) {
                for (var c = 0; c < columns; c++) {
                    Transform instance = Instantiate (prefab, pos, rot, parent.transform);
                    instance.name = string.Format ("L{0:D2}-R{1:D2}-C{2:D2}", l, r, c);
                    pos.x += gap;

                    if (color) {
                        Debug.Log (instance.name + " color " + prefabColor.ToString ());

                        Utils.setSharedColor (instance.GetComponentInParent<Renderer>(), prefabColor);
                        currentR = (prefabColor.r + xStep < 1.0) 
                            ? prefabColor.r + xStep
                            : 0;
                        prefabColor.r = currentR;
                    }
                }
                pos.x = 0;
                pos.z += gap;

                if (color) {
                    currentB = (prefabColor.b + zStep < 1.0) 
                        ? prefabColor.b + zStep
                        : 0;
                    prefabColor.b = currentB;
                }
            }
            pos.z = 0;
            pos.y += gap;

            if (color) {
                currentG = (prefabColor.g + yStep < 1.0) 
                    ? prefabColor.g + yStep
                    : 0;
                prefabColor.g = currentG;
            }
        }
    }


}
