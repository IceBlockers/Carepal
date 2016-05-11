using UnityEngine;
using System.Collections;

public class kitchen_controller : MonoBehaviour {

    public GameObject kitchen_base;
    public GameObject kitchen_cutout;
    
    public void InHallway() {
        Renderer r = kitchen_cutout.GetComponent<Renderer>();
        r.sortingOrder = 101;
    }

    public void InKitchen() {
        Renderer r = kitchen_cutout.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }
}
