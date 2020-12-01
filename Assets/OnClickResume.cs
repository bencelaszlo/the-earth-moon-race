using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickResume : MonoBehaviour
{
    public GameObject inGameMenu;
    // Start is called before the first frame update
    public void ResumeGame()
    {
            inGameMenu.SetActive(false);
            Time.timeScale = 1;
    }
}
