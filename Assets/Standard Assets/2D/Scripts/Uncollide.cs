using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingBoxes
{
    public class Uncollide : MonoBehaviour {
	public MainCharacter mainCharacter;
	public Collision2D coll;

	public void OnDestroy ()
	{
	    if (mainCharacter != null && coll != null) {
		mainCharacter.unCollide (coll);
	    }
	}

    }
}
