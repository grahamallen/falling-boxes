using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserRayCast : MonoBehaviour {

	// Use this for initialization
	private Collider2D c;
	public CustomImageEffect cam;
	private Vector2 source;
	private Vector2 leftOrRight;
	private int prevNumCollisions = 0;
	//public AudioSource laserSound;
	public AudioSource laserBoxSound;
	public AudioSource laserKillSound;
	public GameObject deathParticles;
	public GameObject miniLasers;

	void Start () {
		c = GetComponent<Collider2D> ();
		cam = GameObject.Find ("Main Camera").GetComponent<CustomImageEffect> ();
		source = new Vector2(transform.position.x, transform.position.y);
		bool pointLeft = gameObject.transform.position.x > 0;
		leftOrRight = pointLeft ? Vector2.left : Vector2.right; 
		//laserSound = GameObject.Find ("LaserHolder").GetComponent<AudioSource> ();
		laserBoxSound = GameObject.Find ("LaserBoxHolder").GetComponent<AudioSource> ();
		laserKillSound = GameObject.Find ("LaserKillHolder").GetComponent<AudioSource> ();

		//laserSound.clip.LoadAudioData ();
		laserBoxSound.clip.LoadAudioData ();
		laserKillSound.clip.LoadAudioData ();
		laserBoxSound.loop = true;
//		laserSound.loop = true;
//		laserSound.Play ();
	}
	 
	// Update is called once per frame
	void Update () {
		RaycastHit2D[] collisions = new RaycastHit2D[1];
		int numCollisions = c.Raycast (leftOrRight, collisions, Mathf.Infinity);

		if (numCollisions == 1) {
			//beam hits a box (or player)
			Vector2 target = collisions [0].point;
			cam.lasers.Add (new Vector3 (source.x, target.x, source.y));

			if (collisions [0].collider.gameObject.tag == "Player") {
				GameObject particle2 = Instantiate (deathParticles, new Vector3 (target.x, target.y, 0f), Quaternion.identity);
				Destroy (particle2, .2f);
				collisions [0].collider.gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;

				if (prevNumCollisions < numCollisions) {
					laserKillSound.Play ();
				}

				Invoke ("restart", 0.5f);
			} else if (collisions[0].collider.gameObject.tag == "Box") {
				Vector3 cameraView = cam.gameObject.GetComponent<Camera> ().WorldToViewportPoint (new Vector3 (collisions [0].point.x, collisions [0].point.y, 0f));
				if (cameraView.x > 0 && cameraView.x < 1 && cameraView.y > 0 && cameraView.y < 1) {
					if (prevNumCollisions < numCollisions) {
						laserBoxSound.Play ();
					}
				} else {
					laserBoxSound.Pause ();
				}
			}

			GameObject particle = Instantiate (miniLasers, new Vector3 (target.x, target.y, 0f), Quaternion.identity);
			Destroy (particle, .2f);

		} else {
			//laserBoxSound.Pause ();

			//beam extends out forever
			cam.lasers.Add (new Vector3 (source.x, leftOrRight.x * 10000, source.y));
		}

		prevNumCollisions = numCollisions;
	}

	void restart(){
		SceneManager.LoadScene ("StartingScreen");
	}
}
