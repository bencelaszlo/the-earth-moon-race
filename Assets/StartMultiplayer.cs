using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMultiplayer : MonoBehaviour
{

    public void LoadMultiplayerScene()
    {
        SceneManager.LoadScene("main");
    }
}
