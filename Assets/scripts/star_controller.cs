using UnityEngine;
using System.Collections;

public class star_controller : MonoBehaviour {

    public GameObject starMagic;
    Animator mAnim;

	// Use this for initialization
	void Start () {
        mAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1")) {
            triggerStarMagic();
        }
	}

    public void triggerStarMagic() {
        mAnim.SetTrigger("triggerPulse");
        Instantiate(starMagic, transform.position, Quaternion.identity);
    }

    public void triggerStarNoMagic() {
        mAnim.SetTrigger("triggerPulse");
    }
}
