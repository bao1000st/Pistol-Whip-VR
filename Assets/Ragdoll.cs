using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] rigidBodies;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
            rigidBody.gameObject.layer = 7;
        };
        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.gameObject.layer = 0;
        };
        anim.enabled = false;
    }
}
