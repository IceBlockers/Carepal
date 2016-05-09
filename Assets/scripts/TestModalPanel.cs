using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class TestModalPanel : MonoBehaviour {
	
	private ModalPanel modalPanel;
	private UnityAction yese;
	private UnityAction noe;
	private UnityAction cancele;
	
	
	void Awake() {
		modalPanel = ModalPanel.Instance();
		
		yese = new UnityAction(yes);
		noe = new UnityAction(no);
		cancele = new UnityAction(cancel);
	}

	public void testPanel() {
		UnityAction[] arr = {yese, noe, cancele};
		modalPanel.Choice("What food should pal eat?", arr);
	}
	
	void yes() {
		Debug.Log("Yes");
		RewardManager.Instance().AddReward("yreward");
	}
	
	void no() {
		Debug.Log("No");
	}
	
	void cancel() {
		Debug.Log("Cancel");
	}
}
