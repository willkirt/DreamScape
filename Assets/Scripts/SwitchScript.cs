using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    [Header("The portal this switch helps activate.")]
    public GameObject portal;
    [Header("Switch variables.")]
    public bool isActivated = false;
    public bool isEthereal = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            portal.GetComponent<PortalScript>().ActivatePortal();
            SoundManager.Instance.PlaySFX("PortalActivate");
        }
    }
}
