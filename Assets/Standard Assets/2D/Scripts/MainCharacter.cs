using System;
using UnityEngine;

namespace FallingBoxes
{
	public class MainCharacter : MonoBehaviour
	{
		public float xDrag = 4f;
		private float xMaxSpeed = 20f;
		private float yMaxSpeed = 40f;
		private float m_JumpForce = 1800f;

		public int jumps_total = 1;
		private int jumps_left = 0;

		private Rigidbody2D m_Rigidbody2D;
		public AudioSource jumpSound;
		public AudioSource doubleJumpSound;
		public AudioSource clutchSound;
		public AudioSource landSound;

		private bool grounded = false;
		private bool walled = false;

		Vector2 wallNormal;

		private void Awake ()
		{
			wallNormal = new Vector2 (0f, 0f);
			m_Rigidbody2D = GetComponent<Rigidbody2D> ();
			jumpSound.clip.LoadAudioData ();
			doubleJumpSound.clip.LoadAudioData ();
			clutchSound.clip.LoadAudioData ();
			landSound.clip.LoadAudioData ();
		}

		private void FixedUpdate ()
		{
		}

		public void Move (float h, float v, bool jump)
		{
			float move_factor = Mathf.Sign (h) == Mathf.Sign (m_Rigidbody2D.velocity.x) ? 10 : 30;
			if (v >= 0) {
				m_Rigidbody2D.AddForce (new Vector2 (h * xMaxSpeed * move_factor, 0f));
			} else {
				m_Rigidbody2D.AddForce (new Vector2 (h * xMaxSpeed * move_factor, v * yMaxSpeed * 50));
			}

			float extraDrag = 3f;
			m_Rigidbody2D.AddForce (new Vector2 (m_Rigidbody2D.velocity.x / xMaxSpeed * -1 * xDrag * extraDrag, 0f));

			if (jump && grounded) {
				doJump ();
			} else if (jump && walled) {
				doJump (1.5f);
				m_Rigidbody2D.velocity = new Vector2 (Mathf.Sign (wallNormal.x) * 20f, 0f);
			} else if (jump && jumps_left > 0) {
				doubleJump ();
				jumps_left -= 1;
			}

			if (!jump && walled && Mathf.Sign (wallNormal.x) != Math.Sign (h)) {
				m_Rigidbody2D.AddForce (new Vector2 (-100 * wallNormal.x, 0f));
			}

			clampVelocity ();
		}

		private void doJump (float bonusJumpFactor = 1f) {
			jumpSound.Play ();
			m_Rigidbody2D.AddForce (new Vector2 (0f, m_JumpForce * bonusJumpFactor));
		}

		private void doubleJump () {
			doubleJumpSound.Play ();
			m_Rigidbody2D.AddForce (new Vector2 (0f, m_JumpForce));
			m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, 0f);
		}

		private void clampVelocity ()
		{
			float x = m_Rigidbody2D.velocity.x;
			float y = m_Rigidbody2D.velocity.y;
			if (x > 0) {
				x = x < xMaxSpeed ? x : xMaxSpeed;
			} else {
				x = x > -1 * xMaxSpeed ? x : -1 * xMaxSpeed;
			}
			if (y > 0) {
				y = y < xMaxSpeed ? y : xMaxSpeed;
			} else {
				y = y > -1 * xMaxSpeed ? y : -1 * xMaxSpeed;
			}

			Vector2 velocity = new Vector2 (x, y);
			m_Rigidbody2D.velocity = velocity;
		}

		private void OnCollisionExit2D (Collision2D coll)
		{
			this.unCollide (coll);
		}

		public void unCollide (Collision2D coll)
		{
			foreach (ContactPoint2D wallHit in coll.contacts) {
				if (wallHit.normal.normalized.y > .8 || wallHit.normal.normalized.y < -.8) {
					grounded = false;
				}
				if (wallHit.normal.normalized.x > .8 || wallHit.normal.normalized.x < -.8) {
					walled = false;
					wallNormal = new Vector2 (0, 0);
				}
			}
		}

		private void OnCollisionStay2D (Collision2D coll)
		{

			coll.gameObject.GetComponent<Uncollide> ().coll = coll;
			foreach (ContactPoint2D wallHit in coll.contacts) {
				if (wallHit.normal.normalized.y > .8) {
					if (grounded == false) {
						landSound.Play ();
					}

					grounded = true;
					jumps_left = jumps_total;
				}
				if (wallHit.normal.normalized.x > .8 || wallHit.normal.normalized.x < -.8) {
					if (walled == false) {
						clutchSound.Play ();
					}
				
					walled = true;
					wallNormal = wallHit.normal;
					jumps_left = jumps_total;

				}
			}
		}
	}
}
