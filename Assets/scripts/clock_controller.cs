using UnityEngine;
using System.Collections;

public class clock_controller : MonoBehaviour {
    
    private Animator m_Anim;

    // Use this for initialization
    void Start () {
        m_Anim = gameObject.GetComponent<Animator>();
    }

    public void StartRing() {
        m_Anim.SetBool("ringing", true);
        Invoke("StopRing", 1);
    }

    public void StopRing() {
        m_Anim.SetBool("ringing", false);
    }
}
