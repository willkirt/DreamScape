using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsScript : MonoBehaviour
{
    public void FootStep()
    {
        SoundManager.Instance.PlaySFX("FootStep");
    }

    public void JumpGroan()
    {
        SoundManager.Instance.PlaySFX("JumpGroan");
    }

    public void BatFlap()
    {
        gameObject.GetComponent<EnemyAIScript1>().PlaySFX("Batwing");
    }

    public void SceneReset()
    {
        GameManager.instance.SceneReset();
    }

    public void Interact()
    {
        gameObject.GetComponentInParent<CharacterScript>().Activate();
    }

    public void Landed()
    {
        gameObject.GetComponent<Animator>().SetBool("isJumping", false);
    }

    public void CanJump()
    {
        gameObject.GetComponentInParent<CharacterScript>().ReadyToJump = true;
    }
}
