using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Utils : MonoBehaviour {

    public static GameObject createSphere(Transform parent) {
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

    public static void setSharedColor (Renderer renderer, Color newColor)
    {
        var newMaterial = new Material (renderer.sharedMaterial);
        newMaterial.color = newColor;
        renderer.sharedMaterial = newMaterial;
    }


}
