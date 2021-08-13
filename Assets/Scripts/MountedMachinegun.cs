using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountedMachinegun : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Transform weaponMuzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject targetingDot;
    [SerializeField] private float maxHorizAngle;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float fireRate;
    [SerializeField] private float weaponSpread;
    private float lastFireTimestamp;
    private bool firingEnabled;

    void Start()
    {
        lastFireTimestamp = 0f;
        firingEnabled = true;
    }


    void Update() {
        //We raycast our mouse position to find where to point the gun
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask)) {
            targetingDot.transform.position = raycastHit.point;

            //The weapon muzzle is aimed towards raycast target
            Vector3 aimDirectionVector = raycastHit.point - weaponMuzzle.position;
            aimDirectionVector = Vector3.Normalize(aimDirectionVector);
            Quaternion newRot = Quaternion.LookRotation(aimDirectionVector);
            
            weaponModel.transform.rotation = Quaternion.RotateTowards(weaponModel.transform.rotation, newRot, Time.deltaTime * turnSpeed);

            //If the weapon is pointed too low or high, let's rotate it back
            Vector3 forward = weaponModel.transform.forward;
            Vector3 horizontal = new Vector3(forward.x, 0, forward.z);
            var deviationInDegrees = Vector3.Angle(horizontal, forward);
            if (deviationInDegrees > maxHorizAngle)
            {
                var rotateAxis = Vector3.Cross(horizontal, forward);
                weaponModel.transform.RotateAround(weaponModel.transform.position, rotateAxis, (maxHorizAngle - deviationInDegrees));
            }
        }

        if (Input.GetMouseButton(0) && firingEnabled)
        {
            FireWeapon(weaponModel.transform.forward);
        }
    }

    public void SetFiringEnabled(bool value)
    {
        firingEnabled = value;
    }

    private void FireWeapon(Vector3 direction)
    {
        if (Time.time > lastFireTimestamp + fireRate)
        {
            lastFireTimestamp = Time.time;

            //The bullets are rigidbodies without gravity
            GameObject newBullet = GameObject.Instantiate(bulletPrefab, weaponMuzzle.position, weaponModel.transform.rotation);
            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            
            if (bulletRigidbody == null)
            {
                Debug.LogError("There is no rigidbody attached to " + newBullet.gameObject.name);
            }
            else {
                Vector3 forceVector = direction.normalized * bulletVelocity;

                //Adding random weapon spread
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-weaponSpread, weaponSpread), Random.Range(-weaponSpread, weaponSpread));
                forceVector = randomRotation * forceVector;

                bulletRigidbody.AddForce(forceVector, ForceMode.VelocityChange);
            }
        }
    }
}
