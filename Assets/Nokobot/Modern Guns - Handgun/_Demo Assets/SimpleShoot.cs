using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class SimpleShoot : MonoBehaviour
{
    public int maxAmmo = 10;
    public int currentAmmo;
    public float damage = 10;
    public Player player;

    public AudioSource source;
    public AudioClip noAmmo;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip heal;

    private bool gunIsShoot;

    public TMPro.TextMeshPro text;
    public InputActionProperty pinchAnimation;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject line;

    private Animator gunAnimator;
    private Transform barrelLocation;
    private Transform casingExitLocation;

    private float destroyTimer = 2f;
    private float ejectPower = 150f;

    public ParticleSystem hitEffect;

    void Start()
    {
        casingExitLocation = transform.GetChild(0).GetComponent<Transform>();
        barrelLocation = transform.parent.GetChild(1).GetComponent<Transform>();
        if (barrelLocation == null)
            barrelLocation = transform;
        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
        currentAmmo = maxAmmo;
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        source.PlayOneShot(reload);
    }

    void LateUpdate()
    {
        float triggerValue = pinchAnimation.action.ReadValue<float>();
        //If you want a different input, change it here
        if (triggerValue == 1)
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            if (currentAmmo>0 && gunIsShoot == false) 
            {
                gunIsShoot = true;
                gunAnimator.SetTrigger("Fire");
                source.PlayOneShot(shoot);
            }
            else
            {
               source.PlayOneShot(noAmmo); 
            };
        };
        // text ammo
        text.text = currentAmmo.ToString();
        //reload gun
        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentAmmo < maxAmmo) Reload();
        
    }


    //This function creates the bullet behavior
    public void Shoot()
    {
        currentAmmo--;
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }


        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hitInfo, 150);
        
        if (line)
        {
            GameObject liner = Instantiate(line);
            liner.GetComponent<LineRenderer>().SetPositions(new Vector3[] {barrelLocation.position, hasHit ? hitInfo.point : barrelLocation.position + barrelLocation.forward * 100});
            Destroy(liner, 0.3f);
        }
        
        if (hasHit)
        {
            if (hitEffect) {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
            }

            var hitBox = hitInfo.collider.GetComponent<HitBox>();
            if (hitBox)
            {
                if (hitBox.health.currentHealth > 0 )
                {
                    player.currentScore += 100;
                    hitBox.OnHit(damage);
                };
            };
            
            var healthKit = hitInfo.collider.GetComponent<HealthKit>();
            if (healthKit)
            {
                source.PlayOneShot(heal);
                player.currentHealth = Mathf.Min(player.maxHealth, player.currentHealth + healthKit.health );
                healthKit.gameObject.SetActive(false);
            };
        }
        gunIsShoot = false;
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }
        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
