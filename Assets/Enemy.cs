using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class Enemy : MonoBehaviour
{
    public SimpleEnemyShoot shooter;
    public Transform targetTransform;
    public Transform aimTransform;

    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;
    
    public HumanBone[] humanBones;
    Transform[] boneTransforms;
    Vector3 originalPosition;
    Quaternion originalRotation;



    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i=0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }

        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Health health = GetComponent<Health>();
        if (!health) return;
        if (health.currentHealth > 0)
        {
            Vector3 targetPosition = targetTransform.position;
            for (int i = 0; i < iterations ; i++)
            {
                for (int b = 0; b < boneTransforms.Length ; b++)
                {
                    Transform bone = boneTransforms[b];
                    float boneWeight = humanBones[b].weight * weight;
                    AimAtTarget(bone,targetPosition,boneWeight);
                };
                
            };
            if (weight < 0.8f)
            {
                weight += 0.01f*1;
            };
        };

    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection,targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    void Shoot()
    {
        shooter.Shoot();
    }

    public void resetPosition()
    {
        gameObject.transform.position = originalPosition;
        gameObject.transform.rotation = originalRotation;
    }

    
}
