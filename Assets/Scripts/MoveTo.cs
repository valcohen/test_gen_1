using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public Vector3 goal;
    public float goalDistance = 40f;

    void Start () {
        // var target = transform.position + Vector3. forward * goalDistance;

        // UnityEngine.Debug.Log ("walker " + this.GetInstanceID() + ": " + goal);

        var agent = GetComponent<NavMeshAgent>();
        agent.SetDestination (goal);
    }

    // TODO: change goals at various intervals
}

