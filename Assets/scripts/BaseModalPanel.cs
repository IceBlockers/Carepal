using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

/*
Creating and using panels.

1) Create a Manager object. This will hold our references later.
2) Create an object that will invoke the panel. For example, a button.
3) Create your panel using the ModalPrefab object.
3.1) Add buttons using the ButtonPrefab to the following object: ModalPrefab->ChoicePanel->ResponsePanel. Set text appropriately.
4) Create a script for your panel. It should extend BaseModalPanel.
5) Add the necessary methods; include a way to get a reference for the object (using the singleton pattern).
	Note that the singleton pattern can be replaced with some other getter that returns a reference to the derived Panel class;
	if you just use a GameObject reference and pass it in through the inspector, you won't be able to invoke the Panel methods.
6) Create a script that will invoke the panel. It should contain the callback methods that will be tied to the buttons.
7) Add the panel script from step 4 to the manager and fill in the references in the inspector.
8) Add the callback script from step 6 to the object from step 2.
9) Ensure that the panel is set to not enabled.

Note that each unique panel requires a new GameObject which can be built from the ModalPrefab.
You can re-use a panel if desired, however this should be accomplished by invoking the singleton of the child class
rather than duplicating the panel.
*/

public abstract class BaseModalPanel : MonoBehaviour {
	
	public Text QuestionText;
	//public Button[] Buttons; // callback functions defined in Choice
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
