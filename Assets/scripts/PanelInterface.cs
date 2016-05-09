using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public abstract class PanelInterface : MonoBehaviour {
	
	public Text QuestionText;
	public Button[] Buttons; // callback functions defined in Choice
	public GameObject ModalObject; // represents the panel; enables showing/hiding
	
	
	/*
	This function defines how a panel closes.
	This is a purely UI function; code that affects state of characters should
	be put in the callback functions tied to buttons in the Choice function below.
	
	The below example one-liner is usually good enough.
	
		ModalObject.SetActive(false);
	*/
	abstract public void ClosePanel();
	
	
	/*
	This function defines how the panel will respond to being invoked.
	"question" should set the text of the upper panel.
	Each button in a panel should be mapped to an event and should also be mapped to a "close" function.
	In addition, the ModalObject property must be made active in this function.
	
	EXAMPLE:
		ModalObject.SetActive(true);
		SomeButton.GameObject.SetActive(true);
		SomeButton.OnClick.RemoveAllListeners();
		SomeButton.OnClick.AddListener(events[0]);
		SomeButton.OnClick.AddListener(ClosePanel);
		this.QuestionText.text = question;
		
	*/
	abstract public void Choice(string question, UnityAction[] events);
}
