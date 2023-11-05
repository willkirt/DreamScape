using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    public void PlayBTN()
    {
        SoundManager.Instance.PlaySFX("ButtonClicked");
        SceneManager.LoadScene("Tutorial");
        GameManager.instance.ChangeState("Play");
    }

    public void MenuBTN()
    {
        SoundManager.Instance.PlaySFX("ButtonClicked");
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.ChangeState("Menu");
    }

    public void HowToPlayBTN()
    {
        SoundManager.Instance.PlaySFX("ButtonClicked");
        SceneManager.LoadScene("HowtoPlayScene");
    }

    public void Quit()
    {
        SoundManager.Instance.PlaySFX("ButtonClicked");
        Application.Quit();
    }

    public void Resume()
    {
        SoundManager.Instance.PlaySFX("ButtonClicked");
        GameManager.instance.ChangeState("Play");
    }
}
