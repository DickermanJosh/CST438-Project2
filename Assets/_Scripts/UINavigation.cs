using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace _Scripts
{
    public class UINavigation : MonoBehaviour
    {
        public Button settingsBackUpButton; 
        
        public void ChangeSelectedButton(Button newButton)
        {
            // If a button is intractable no matter what, choose this one!
                EventSystem.current.SetSelectedGameObject(newButton.gameObject);
            
        }
        
        //this is for buttons that may not be intractable when switching between menus.
        //also unity is really dumb because it only displays functions with >2 game object arguments
        //for the event system to use. Very Annoying.
        public void ChangeSelectedButtonSettings(Button newButton)
        {   Debug.Log("Called changeSelectedButtonSettings");
            // Check if the first button is dead.
            if (!newButton.interactable)
            {
                    Debug.Log("OG fucked, get back up.");
                    EventSystem.current.SetSelectedGameObject(settingsBackUpButton.gameObject);
            }
            else
            {
                Debug.Log("OG made it through");
                EventSystem.current.SetSelectedGameObject(newButton.gameObject);
            }
        }
        
    }
}
