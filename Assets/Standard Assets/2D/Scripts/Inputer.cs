using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FallingBoxes
{
	[RequireComponent (typeof(MainCharacter))]
	public class Inputer : MonoBehaviour
	{

		private MainCharacter m_Character;
		private bool m_Jump;

		private void Awake ()
		{
			m_Character = GetComponent<MainCharacter> ();
		}


		private void Update ()
		{
			if (!m_Jump) {
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
			}

			if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
				Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
			}
		}


		private void FixedUpdate ()
		{
			float h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
			float v = CrossPlatformInputManager.GetAxisRaw ("Vertical");
			// Pass all parameters to the character control script.
			m_Character.Move (h, v, m_Jump);
			m_Jump = false;
		}

	}
}
