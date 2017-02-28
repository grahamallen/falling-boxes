using UnityEngine;
using UnityEngine.UI;

namespace FallingBoxes
{

	public class GameManager : MonoBehaviour
	{

		public GameObject box;
		public GameObject platformPiece;
		public GameObject character;
		public GameObject laser;
		public GameObject motivationArrow;
		private Transform gameManager;
		private int boxSpawnPeriodicity = 400;
		private int laserSpawnPeriodicity = 50;
		private static float maxHeightReached = 0f;
		private int boxSpawnOffset = 40;
		private int boxSpawnVariance = 0;
		// public float minGravity = 0.75f;
		// public float maxGravity = 1.25f;
		private int difficulty = 1;
		private int highestDifficulty = 1;
		private int numLasers = 0;
		private int boxDeltaX = 10;
		private int laserDeltaX = 14;
		public int maxTotalLasers = 5;
		private int minArea = 2;

		void Awake ()
		{
			InitGame ();
		}

		// Update is called once per frame
		void Update ()
		{
			difficulty = ((int)Mathf.Abs (character.transform.position.y) / 100) + 1;
			if (difficulty > highestDifficulty) {
				highestDifficulty = difficulty;
				numLasers = (int)Mathf.Ceil ((difficulty / 10f / (1f + difficulty / 10f)) * maxTotalLasers); //  x / (1 + x)  --> 1/11, 1/6, 3/7, 1/2, 5/9, 3/5, 2/3, 5/7, 3/4, 7/9
				becomeMotivated();
			}

			if (Time.frameCount % boxSpawnPeriodicity == 0) {
				int boxNum = Time.frameCount / boxSpawnPeriodicity;
				box.name = "box" + boxNum.ToString ();
				int randomWidth = Random.Range (0, 5);
				int randomHeight = Random.Range (0, 5); 
				float boxX = Random.Range (-boxDeltaX, boxDeltaX);
				float boxY = Random.Range (boxSpawnOffset - boxSpawnVariance, boxSpawnOffset + boxSpawnVariance) + character.transform.position.y;
				GameObject newBox = Instantiate (box, new Vector3 (boxX, boxY, 0f), Quaternion.identity, gameManager);
				// newBox.GetComponent<Rigidbody2D> ().gravityScale = Random.Range (minGravity, maxGravity);
				newBox.transform.localScale += new Vector3 (randomWidth, randomHeight, 0f);
				newBox.GetComponent<Uncollide> ().mainCharacter = character.GetComponent<MainCharacter> ();
			}

			if (maxHeightReached < character.transform.position.y) {
				maxHeightReached = character.transform.position.y;
			}

			Text highScore = GameObject.Find ("HighScore").GetComponent<Text> ();
			highScore.text = "High Score: " + Mathf.Round(maxHeightReached).ToString ();

			Text currentScore = GameObject.Find ("CurrentScore").GetComponent<Text> ();
			currentScore.text = "Current Score: " + Mathf.Round(character.transform.position.y).ToString ();

			if ((Time.frameCount % laserSpawnPeriodicity == 0) && numLasers > 0) {
				//GameObject newLaser = Instantiate(laser, new Vector3(laserDeltaX * (Mathf.Sign(Random.Range(-1,1))), character.transform.position.y + boxSpawnOffset, 0f), Quaternion.identity, gameManager);
				Instantiate (laser, new Vector3 (laserDeltaX * (Mathf.Sign (Random.Range (-1, 1))), character.transform.position.y + boxSpawnOffset, 0f), Quaternion.identity, gameManager);
				numLasers--;
			}
			SaveScore ();
		}

		void InitGame ()
		{

			if (PlayerPrefs.HasKey ("highscore")) {
				maxHeightReached = PlayerPrefs.GetFloat ("highscore");
			}
			gameManager = new GameObject ("Game").transform;

			GameObject startingPlatform = Instantiate (platformPiece, new Vector3 (-15, 3.866f, 0f), Quaternion.identity, gameManager) as GameObject;
			GameObject startingCeiling = Instantiate (platformPiece, new Vector3 (-14.376f, -0.634f, 0f), Quaternion.Euler (new Vector3 (0, 0, 90)), gameManager) as GameObject;
			GameObject startingNookWall = Instantiate (platformPiece, new Vector3 (-15, -0.634f, 0f), Quaternion.identity, gameManager) as GameObject;

			startingCeiling.GetComponent<Uncollide> ().mainCharacter = character.GetComponent<MainCharacter> ();
			startingNookWall.GetComponent<Uncollide> ().mainCharacter = character.GetComponent<MainCharacter> ();
			startingPlatform.GetComponent<Uncollide> ().mainCharacter = character.GetComponent<MainCharacter> ();

			Destroy (startingPlatform, 30f);
			Destroy (startingCeiling, 30f);
			Destroy (startingNookWall, 30f);

		}

		void SaveScore ()
		{
			PlayerPrefs.SetFloat ("highscore", Mathf.Round(maxHeightReached));
		}

		void becomeMotivated() {
			GameObject newMotivation = Instantiate (motivationArrow, new Vector3 (0f, character.transform.position.y + boxSpawnOffset, 0f), Quaternion.identity, gameManager) as GameObject;

			newMotivation.layer = 10;
		}
	}
}
