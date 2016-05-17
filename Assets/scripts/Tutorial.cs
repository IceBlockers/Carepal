using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Tutorial : MonoBehaviour {
    private ModalPanel modalPanel;

    // Use this for initialization
    void Start () {
        modalPanel = ModalPanel.Instance();
        if (PlayerPrefs.HasKey("Tutorial")) {
            EventManager.StartListening("Tutorial", Click);
            EventManager.TriggerEvent("Tutorial");
        } else {
            PlayerPrefs.SetInt("Tutorial", 1);

        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    void Click () {
        UnityAction[] arr = { new UnityAction(Callback)};
        modalPanel.Choice("hi", arr);
    }

    void Callback() {
        Debug.Log("Testing");
    }
}
