using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class SimpleEnemyShoot : MonoBehaviour
{
    public AudioSource source;
    public AudioClip shoot;

    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject line;

    private Animator gunAnimator;
    private Transform barrelLocation;
    private Transform casingExitLocation;

    private float destroyTimer = 2f;
    private float fireSpeed = 6;
    private float ejectPower = 150f;

    void Awake() 
    {
        casingExitLocation = transform.GetChild(0).GetComponent<Transform>();
        barrelLocation = transform.parent.GetChild(1).GetComponent<Transform>();  
        Debug.Log("Hello");  
    }
    //This function creates the bullet behavior
    public void Shoot()
    {
        source.PlayOneShot(shoot);
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }


        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hitInfo, 100);
        if (line)
        {
            GameObject liner = Instantiate(line);
            liner.GetComponent<LineRenderer>().SetPositions(new Vector3[] {barrelLocation.position, hasHit ? hitInfo.point : barrelLocation.position + barrelLocation.forward * 100});
            Destroy(liner, 0.5f);
        }


        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.position = barrelLocation.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = barrelLocation.forward * fireSpeed;
        Destroy(spawnedBullet,3);

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
