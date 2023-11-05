using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    [Header("Stamina variables")]
    [SerializeField] float playerStamina = 100.0f;
    [SerializeField] float maxStamina = 100.0f;
    [SerializeField] bool hasRegenerated = true;
    [Tooltip("The rate the players stamina depletes while sprinting.")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 10f;
    [Tooltip("The rate the players stamina recovers while not sprinting.")]
    [Range(0, 50)] [SerializeField] private float staminaRegen = 7.5f;

    [Header("Stamina UI Elements")]
    [Tooltip("The script will find and assign this element at the start of each level.")]
    [SerializeField] private Image staminaBarUI = null;

    private void Start()
    {
        staminaBarUI = GameObject.Find("StaminaBar").GetComponent<Image>();
    }

    private void Update()
    {
        if (gameObject.GetComponent<CharacterScript>().IsSprinting)
        {
            playerStamina -= staminaDrain * Time.deltaTime;

            if (playerStamina < maxStamina)
            {
                hasRegenerated = false;
            }
            if (playerStamina < 0)
            {
                gameObject.GetComponent<CharacterScript>().IsExhausted = true;
                playerStamina = 0.0f;
            }
        }
        else if (!hasRegenerated)
        {
            playerStamina += staminaRegen * Time.deltaTime;

            if (playerStamina >= maxStamina)
            {
                playerStamina = 100.0f;
                hasRegenerated = true;
            }

            if (gameObject.GetComponent<CharacterScript>().IsExhausted && playerStamina > maxStamina/2)
            {
                gameObject.GetComponent<CharacterScript>().IsExhausted = false;
            }
        }

        staminaBarUI.fillAmount = playerStamina / maxStamina;
    }
}
