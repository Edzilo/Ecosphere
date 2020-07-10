using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    private void Start()
    {
        SelectFirstButton();
    }

    private void OnEnable()
    {
        SelectFirstButton();
    }


    private void SelectFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //Button button = findFirstButton();
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            print("I select " + button);
            //button.Select();
            //EventSystem.current.firstSelectedGameObject = button.gameObject;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}

