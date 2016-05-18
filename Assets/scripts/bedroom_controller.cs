using UnityEngine;
using System.Collections;

public class bedroom_controller : MonoBehaviour {
    public GameObject room;
    public GameObject wardrobeOpen;
    public GameObject door;
    public GameObject bed;
    public GameObject pal;
    public GameObject bedSleeping;
	public AudioClip wardrobeClip;
	public AudioClip sleepingClip;

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

    public void ToggleWardrobe() {
        Renderer r = wardrobeOpen.GetComponent<Renderer>();
        if (r.sortingOrder == -101) {
            r.sortingOrder = -99;
        } else {
            r.sortingOrder = -101;
        }
		AudioSource src = GetComponent<AudioSource>();
		if (wardrobeClip && !src.isPlaying) {
			src.clip = wardrobeClip;
			src.Play();
		}
    }

    public void BedInFront() {
        Renderer r = bed.GetComponent<Renderer>();
        r.sortingOrder = 101;
    }

    public void BedBehind() {
        Renderer r = bed.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }

    public void StartSleeping() {
        Renderer r = bedSleeping.GetComponent<Renderer>();
        r.sortingOrder = 101;
		
    }

    public void StopSleeping() {
        Renderer r = bedSleeping.GetComponent<Renderer>();
        r.sortingOrder = -101;
    }

    public void ToggleSleeping() {
        Renderer r = bedSleeping.GetComponent<Renderer>();
        if (r.sortingOrder == -101) {
            r.sortingOrder = 105;
            pal.SetActive(false);
			// play sound
			AudioSource src = GetComponent<AudioSource>();
			if (sleepingClip && !src.isPlaying) {
				src.clip = sleepingClip;
				src.Play();
			}
        } else {
            r.sortingOrder = -101;
            pal.SetActive(true);
        }

    }
}
