using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public List<GameObject> levers;
    public bool isClosed = true;

    public void Update()
    {
        if (gameObject.transform.position.y !< 5.9 && !isClosed)
            StartCoroutine(Open());          
    }

    public void OpenGate()
    {
        bool allLeversPulled = true;

        foreach (GameObject lever in levers)
        {
            if (!lever.GetComponentInChildren<LeverScript>().leverPulled)
            {
                allLeversPulled = false;
            }
        }

        if (allLeversPulled)
        {
            isClosed = false;
            GameManager.instance.ActivateEnemy();
            GameManager.instance.DisplayText("That was it. The gate is now open.");
        }
        else
        {
            GameManager.instance.DisplayText("Hmmm, nothing happened. There must be another lever here somewhere.");
        }
    }

    IEnumerator Open()
    {
        while (gameObject.transform.position.y < 4.9)
        {
            gameObject.transform.position += new Vector3(0, .1f, 0) * Time.deltaTime;
            yield return new WaitForSeconds(.1f);
        }
    }
}
