using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTriggerScript : MonoBehaviour
{
    [Header("Display text")]
    [Tooltip("Add the text you want displayed on screen when the trigger is hit.")]
    public string popupText;

    [Header("Trigger target")]
    [Tooltip("The tag of the object you want to be able to trigger this popup.")]
    public string targetTag;

    [Tooltip("Toggle on if you want the trigger to only play once per scene load.")]
    public bool triggerOnce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            StartCoroutine(GameManager.instance.PopupText(popupText));
        }

        if (triggerOnce)
        {
            targetTag = "NoTriggerTarget!!!";
        }
    }
}
