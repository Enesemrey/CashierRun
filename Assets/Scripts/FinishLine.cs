using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FinishLine : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementController register))
        {
            register.isActive = false;

            CameraManager.instance.SetCam(CameraManager.instance.GetEndingCam());
            MovementController.instance.DoEndingRotation();
            MovementController.instance.DoEndingPosition();
            WaitForIndicator();
        }
    }

    public void WaitForIndicator()
    {
        IEnumerator Wait()
        {
            CashRegisterController.instance.DisableAllCounts();
            CashRegisterController.instance.DisableHands();
            CashRegisterController.instance.ChangeStateOfAllLocations();
            yield return new WaitForSeconds(1.5f);
            CashRegisterController.instance.ShowIndicator();

            yield return new WaitForSeconds(1);

            CashRegisterController.instance.StartCounting();
            CashRegisterController.instance.WaitForDestroy();




        }

        StartCoroutine(Wait());
    }
    
}
