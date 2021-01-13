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
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}

