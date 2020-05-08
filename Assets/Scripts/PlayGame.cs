using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void Instructions()
    {
        SceneManager.LoadScene("instructionsscene", LoadSceneMode.Single);
    }

    public void Back()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
