using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class fridge : MonoBehaviour {

    public GameObject circleFat;
    public GameObject circleVeggy;
    public GameObject circleMeat;
    public GameObject btnAdvance;

    private bool bChoseFat;
    private bool bChoseVeggy;
    private bool bChoseMeat;

    private string fatSprite;
    private string veggySprite;
    private string meatSprite;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosition = new Vector2(worldPos.x, worldPos.y);
            var hitCollider = Physics2D.OverlapPoint(mousePosition);
            
            if (hitCollider) {
                var renderer = hitCollider.GetComponent<SpriteRenderer>();
                if (hitCollider.tag.Contains("fat")) {
                    placeCircle(hitCollider, circleFat);
                    bChoseFat = true;
                    fatSprite = renderer.sprite.name;
                } else if (hitCollider.tag.Contains("meat")) {
                    placeCircle(hitCollider, circleMeat);
                    bChoseMeat = true;
                    meatSprite = renderer.sprite.name;
                } else if (hitCollider.tag.Contains("veggy")) {
                    placeCircle(hitCollider, circleVeggy);
                    bChoseVeggy = true;
                    veggySprite = renderer.sprite.name;
                } else {
                    //you clicked the arrow button.
                    //Debug.Log("Moving on to the next scene!");
                    saveChoices();

                    //TODO:  Load new level here
                    SceneManager.LoadScene("LevelMinigame");
                }
                updateButton();
            }
        }
    }

    void saveChoices() {
        if (fatSprite.Contains("cheese")) { PlayerPrefs.SetInt("food1", 0); }
        else if (fatSprite.Contains("avocad")) { PlayerPrefs.SetInt("food1", 1); }
        else if (fatSprite.Contains("butter")) { PlayerPrefs.SetInt("food1", 2); }

        if (veggySprite.Contains("lettu")) { PlayerPrefs.SetInt("food2", 3); }
        else if (veggySprite.Contains("onion")) { PlayerPrefs.SetInt("food2", 4); }
        else if (veggySprite.Contains("tomat")) { PlayerPrefs.SetInt("food2", 5); }

        if (meatSprite.Contains("patty")) { PlayerPrefs.SetInt("food3", 6); }
        else if (meatSprite.Contains("roast")) { PlayerPrefs.SetInt("food3", 7); }
        else if (meatSprite.Contains("sandw")) { PlayerPrefs.SetInt("food3", 8); }

        /*
        Debug.Log(string.Format("Saved: {0}, {1}, {2}",
            PlayerPrefs.GetInt("food1"),
            PlayerPrefs.GetInt("food2"),
            PlayerPrefs.GetInt("food3")));
        */
    }

    void placeCircle(Collider2D hitCollider, GameObject circle) {
        circle.transform.position = new Vector3(
            hitCollider.transform.position.x,
            hitCollider.transform.position.y,
            0);
    }

    void updateButton() {
        if (bChoseFat && bChoseMeat && bChoseVeggy) {
            Vector3 pos = btnAdvance.transform.position;
            pos.x = 6.621002f;
            pos.y = -3.56601f;
            btnAdvance.transform.position = pos;
        } else {
            Vector3 pos = btnAdvance.transform.position;
            pos.y = -1000;
            btnAdvance.transform.position = pos;
        }
    }
}
