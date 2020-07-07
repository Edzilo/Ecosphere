using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //Button button = findFirstButton();
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            print("I select " + button);
            button.Select();
        }
    }

    private Button findFirstButton()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            print("For loop: " + transform.GetChild(i));
            if (transform.GetChild(i).GetComponent<Button>() != null)
            {
                return transform.GetChild(i).gameObject.GetComponent<Button>();
            }
        }

        return null;

    }
}
