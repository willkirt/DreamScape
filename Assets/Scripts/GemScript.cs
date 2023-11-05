using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public AudioClip clip;
    public void Update()
    {
        transform.Rotate(new Vector3(0, 70, 0) * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlaySFX("GemCollected");
            GameManager.instance.LevelScore += 10;
            other.gameObject.GetComponent<CharacterScript>().ProjCurrDuration -= 1f;
            GameManager.instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
