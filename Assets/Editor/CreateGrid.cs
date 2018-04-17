using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGrid : ScriptableWizard {

    public Transform prefab;
    public int rows     = 10;
    public int columns  = 10;
    public int layers   = 1;
    public float gap    = 2f;
    public bool color   = true;

    [MenuItem("Custom/Create Grid...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard ("Create Grid of Objects", typeof(CreateGrid), "Create");
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
        if (prefab == null) { prefab = createSphere (parent).transform; }

        Color prefabColor = new Color (0, 0, 0);
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

                        setSharedColor (instance.GetComponentInParent<Renderer>(), prefabColor);
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
                    ? prefabColor.r + yStep
                    : 0;
                prefabColor.g = currentG;
            }
        }
    }

    private GameObject createSphere(Transform parent) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent (parent);
        sphere.AddComponent<MeshCollider>();

        string tempFolder = "Assets/Temp";
        if ( ! AssetDatabase.IsValidFolder(tempFolder) ) {
            string guid = AssetDatabase.CreateFolder("Assets", "Temp");
            tempFolder  = AssetDatabase.GUIDToAssetPath(guid);
        }

        GameObject spherePrefab = PrefabUtility.CreatePrefab(tempFolder + "/generatedSphere.prefab", sphere, 
            ReplacePrefabOptions.ReplaceNameBased);

        return spherePrefab;
    }

    static void setSharedColor (Renderer renderer, Color newColor)
    {
        var newMaterial = new Material (renderer.sharedMaterial);
        newMaterial.color = newColor;
        renderer.sharedMaterial = newMaterial;
    }

}
