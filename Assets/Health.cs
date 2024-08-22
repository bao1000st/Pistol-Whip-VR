using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioSource source;
    public AudioClip die;

    public float maxHealth;
    public float currentHealth;

    public Ragdoll ragdoll; 
    SkinnedMeshRenderer skinnedMeshRenderer;
    GameObject outlineBody;
    DissolveObject dissolveObject;

    float blinkIntensity = 0.4f;
    float blinkDuration = 0.5f;
    float blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll>();
        dissolveObject = gameObject.GetComponent<DissolveObject>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        outlineBody = gameObject.transform.GetChild(0).gameObject;
        outlineBody.layer = 8;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
                HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
                hitBox.health = this; 
        };
    }

    // Update is called once per frame
    void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;
        if ( currentHealth > 0 ) dissolveObject.SetValue(intensity);
    }

    public void TakeDamage(float damage)
    {
        currentHealth-=damage;
        if ( currentHealth <= 0 )
        {
            Die();
        };
        blinkTimer = blinkDuration;
    }


    public void Die() {
        source.PlayOneShot(die);
        ragdoll.ActivateRagdoll();
        // turn off outline effects
        outlineBody.layer = 0;
        // turn on dissolve effects
        dissolveObject.value = 0.3f;
    }
}
