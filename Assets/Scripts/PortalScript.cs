using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    [Tooltip("Each switch needed to activate this portal.")]
    public List<GameObject> switches;
    public GameObject portalQuad;
    public ParticleSystem blueParticles;
    public ParticleSystem darkBeam;

    [Tooltip("Name of the scene this portal leads to.")]
    public string nextScene;
    [Tooltip("Check if last portal to unlock and reveal cursor in menu.")]
    public bool toMenu;
    public bool isPortalActive = false;

    public void Start()
    {
        portalQuad.GetComponent<MeshRenderer>().enabled = false;
        blueParticles.Stop();
        darkBeam.Stop();
    }
    public void ActivatePortal()
    {
        bool activateportal = true;

        foreach (GameObject _switch in switches)
        {
            if (!_switch.GetComponent<SwitchScript>().isActivated)
            {
                activateportal = false;
            }
        }

        isPortalActive = activateportal;

        if (isPortalActive)
        {
            portalQuad.GetComponent<MeshRenderer>().enabled = true;
            blueParticles.Play();
            darkBeam.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        // Makes the cursor visible and moveable if openning the main menu
        if (toMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(other.CompareTag("Player") && isPortalActive)
        {
            SoundManager.Instance.PlaySFX("PortalUsed");
            GameManager.instance.FinalScore += GameManager.instance.LevelScore;
            GameManager.instance.LevelScore = 0;
            SceneManager.LoadScene(nextScene);
        }
    }
}
