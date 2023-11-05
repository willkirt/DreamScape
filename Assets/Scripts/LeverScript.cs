using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public bool leverPulled = false;
    public GameObject gate;
    private Animator anim;

    public void Activate()
    {
        if(!leverPulled)
        {
            leverPulled = true;

            anim = GetComponentInChildren<Animator>();
            anim.SetTrigger("LeverPulled");
            SoundManager.Instance.PlaySFX("LeverPulled");
            gate.GetComponent<GateScript>().OpenGate();
        }
    }
}
