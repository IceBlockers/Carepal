using UnityEngine;
using System.Collections;

public class room_controller : MonoBehaviour {
    public GameObject wardrobeOpen;
    public GameObject door;
    public GameObject bed;

    public void OpenDoor() {
        Renderer r = door.GetComponent<Renderer>();
        r.sortingOrder = -99;
    }

    public void CloseDoor() {
        Renderer r = door.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }

    public void OpenWardrobe() {
        Renderer r = wardrobeOpen.GetComponent<Renderer>();
        r.sortingOrder = -99;
    }

    public void CloseWardrobe() {
        Renderer r = wardrobeOpen.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }

    public void BedInFront() {
        Renderer r = bed.GetComponent<Renderer>();
        r.sortingOrder = 101;
    }

    public void BedBehind() {
        Renderer r = bed.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }
}
