using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour {

	public List<GameObject> menuOptions;
	public List<GameObject> controls;
	public List<GameObject> credits;
	public GameObject highScore;
	public AudioSource blipSound;
	public bool hasHighScore;
	private int selectedIndex = 0;
	private bool pressed = false;

	// Use this for initialization
	void Start () {
		Screen.SetResolution (600, 800, false);

		blipSound.clip.LoadAudioData();

		if (PlayerPrefs.HasKey ("highscore")) {
			float score = PlayerPrefs.GetFloat("highscore");
			hasHighScore = true;
			Text highScoreText = highScore.GetComponent<Text> ();
			highScoreText.text = ("High Score: " + score.ToString());
			highScore.SetActive (true);
		} else {
			highScore.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i = 0; i < menuOptions.Count; i++) {
			Text textThing = menuOptions [i].GetComponent<Text> ();
		
			if (i == selectedIndex) {
				textThing.color = new Color (255f, 0, 255f);
			} else {
				textThing.color = new Color (255f, 255f, 255f);
			}
		}

		if (selectedIndex == 1) {
			//show credits
			credits.ForEach ((credit) => {
				credit.SetActive (true);
			});

			//hide controls
			controls.ForEach ((control) => {
				control.SetActive (false);
			});
		} else if (selectedIndex == 2) {
			//hide credits
			credits.ForEach ((credit) => {
				credit.SetActive (false);
			});

			//show controls
			controls.ForEach ((control) => {
				control.SetActive (true);
			});
		} else {
			//hide credits
			credits.ForEach ((credit) => {
				credit.SetActive (false);
			});

			//hide controls
			controls.ForEach ((control) => {
				control.SetActive (false);
			});
		}

		if (CrossPlatformInputManager.GetButtonDown ("Submit") && !pressed) {
			pressed = true;
			OnPressEnter ();
		} else if (CrossPlatformInputManager.GetAxis ("Vertical") > 0 && !pressed) {
			if (pressed == false) {
				blipSound.Play ();
			}
			pressed = true;
			selectedIndex = (menuOptions.Count + (selectedIndex - 1)) % menuOptions.Count;
		} else if (CrossPlatformInputManager.GetAxis ("Vertical") < 0 && !pressed) {
			if (pressed == false) {
				blipSound.Play ();
			}
			pressed = true;
			selectedIndex = (menuOptions.Count + (selectedIndex + 1)) % menuOptions.Count;
		} else if (CrossPlatformInputManager.GetAxis("Vertical") == 0) { 
			pressed = false;
		}

	}

	void OnPressEnter() {
		if (selectedIndex == 0) {
			SceneManager.LoadScene ("MainGame");
		} else if (selectedIndex == 3) {
			UnityEngine.Application.Quit();
		}
	}
}
