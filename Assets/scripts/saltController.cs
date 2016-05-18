using UnityEngine;
using System.Collections;

public class saltController : MonoBehaviour {
    public int saltShakes;      // # of shakes
    public GameObject shaker;
    public GameObject saltParticles;

    private Animator saltAnim;

	// Use this for initialization
	void Start () {
        saltAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void shake()
    {
        if (saltShakes > 1)
        {
            saltShakes--;
            saltAnim.SetTrigger("SaltyTime");
            Instantiate(saltParticles);
        }
        else if (saltShakes == 1)
        {
            saltShakes--;
            // Instantiate(saltParticles, shaker.transform.position, Quaternion.identity);
            Instantiate(saltParticles);
            saltAnim.SetTrigger("DoneSalting");
        }
    }
}
