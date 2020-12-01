using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscBtnPressedListening : MonoBehaviour
{

    public GameObject inGameMenu;
    public bool isPanelShown = false;

    // Update is called once per frame
    
    void Start()
    {
        HideInGameMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & !isPanelShown)
        {
            ShowInGameMenu();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) & isPanelShown)
        {
            HideInGameMenu();
        }
    }

    public void ShowInGameMenu()
    {
            inGameMenu.SetActive(true);
            isPanelShown = true;
            Time.timeScale = 0;
    }

    public void HideInGameMenu()
    {
            inGameMenu.SetActive(false);
            isPanelShown = false;
            Time.timeScale = 1;
    }

}
