using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementController player))
        {
            player.inSwerveMode = false;
            // HOLD TO SWERVE YAZILI PANELÝ AÇ
            CanvasManager.instance.ShowText(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovementController player))
        {
            player.inSwerveMode = true;
            // HOLD TO SWERVE YAZILI PANELÝ KAPAT
            CanvasManager.instance.ShowText(false);
        }
    }
}
