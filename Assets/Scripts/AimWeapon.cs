using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private GameObject weaponMuzzle;
    [SerializeField] private GameObject targetingDot;
    [SerializeField] private float maxHorizAngle;
    [SerializeField] private float trackingSpeed;


    private void Update() {
        //We raycast our mouse position to find where to point the gun
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask)) {
            targetingDot.transform.position = raycastHit.point;
            Vector3 aimDirectionVector = raycastHit.point - weaponMuzzle.transform.position;
            aimDirectionVector = Vector3.Normalize(aimDirectionVector);
            Quaternion newRot = Quaternion.LookRotation(aimDirectionVector);
            //weaponModel.transform.rotation = newRot;

            
            weaponModel.transform.rotation = Quaternion.Lerp(weaponModel.transform.rotation, newRot, Time.deltaTime * trackingSpeed);

            //If the weapon is pointed too low or high, let's clamp its rotation
            Vector3 forward = weaponMuzzle.transform.forward;
            Vector3 horizontal = new Vector3(forward.x, 0, forward.z);
            var deviationInDegrees = Vector3.Angle(horizontal, forward);
            if (deviationInDegrees > maxHorizAngle)
            {
                // Get the correctional rotation axis
                var rotateAxis = Vector3.Cross(horizontal, forward);
                weaponModel.transform.RotateAround(weaponModel.transform.position, rotateAxis, (maxHorizAngle - deviationInDegrees));
            }
        }
        
    }
}
