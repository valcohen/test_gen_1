using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenObjectsOnSphere : ScriptableWizard {

    public Transform prefab;
    public int numInstances = 100;
    public float radius = 10;


    public bool     color   = true;
    public float    alpha   = 0.25f;    // 1 == opaque, 0 == clear

    [MenuItem("Custom/Gen Objects on Sphere...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard ("Generate Objects on a Sphere", typeof(GenObjectsOnSphere), "Create");
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

        for (var i = 0; i < numInstances; i++) {
            Vector3 instancePos = Random.onUnitSphere * radius;
            Transform instance = Instantiate (prefab, instancePos, rot, parent.transform);
            instance.name = string.Format ("inst_{0:D3}", i);

            Debug.Log (instance.name);
        }

    }

}
