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
        
        Buttons[1].onClick.RemoveAllListeners();
        Buttons[1].onClick.AddListener (events[1]);
        Buttons[1].onClick.AddListener (ClosePanel);
        
        Buttons[2].onClick.RemoveAllListeners();
        Buttons[2].onClick.AddListener (events[2]);
        Buttons[2].onClick.AddListener (ClosePanel);

        this.QuestionText.text = question;

        Buttons[0].gameObject.SetActive (true);
        Buttons[1].gameObject.SetActive (true);
        Buttons[2].gameObject.SetActive (true);
    }

    override public void ClosePanel () {
        ModalObject.SetActive (false);
    }
}