using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGrid : ScriptableWizard {

    public Transform prefab;
    public int rows     = 10;
    public int columns  = 10;
    public int layers   = 1;
    public float gap    = 10f;

    [MenuItem("MyTools/Create Grid...")]
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

        for (var r = 0; r < rows; r++) {
            var instance = Instantiate (prefab, pos, rot, parent.transform);
            pos.x += gap;
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
}
