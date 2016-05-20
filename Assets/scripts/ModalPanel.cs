using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

/*
Example ModalPanel.
*/
public class ModalPanel : BaseModalPanel {
    
	public Button[] Buttons;	
	
	// implement the singleton pattern
    private static ModalPanel modalPanel;

    public static ModalPanel Instance () {
        if (!modalPanel) {
            modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
            if (!modalPanel)
                Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }
        
        return modalPanel;
    }

    // call a prompt with the given question
    override public void Choice (string question, UnityAction[] events) {
        ModalObject.SetActive (true);
        
        Buttons[0].onClick.RemoveAllListeners();
        Buttons[0].onClick.AddListener (events[0]);
        Buttons[0].onClick.AddListener (ClosePanel);

        Buttons[0].gameObject.SetActive(true);

        

        this.QuestionText.text = question;
    }

    override public void ClosePanel () {
        ModalObject.SetActive (false);
    }
}