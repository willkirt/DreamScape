using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallResetScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.SceneReset();
    }
}
