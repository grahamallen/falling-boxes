using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {

    Material material;

    // Use this for initialization
    void Start () {
	material = GetComponent<Renderer>().material;
	// setRandomRGB();
	setRGB(.5f, .5f, .5f);
    }

    private void setRandomRGB(){
	setRGB(UnityEngine.Random.value,
	       UnityEngine.Random.value,
	       UnityEngine.Random.value);
    }
    private void setRGB(float r, float g, float b){
	material.SetFloat("_R", r);
	material.SetFloat("_G", g);
	material.SetFloat("_B", b);
    }

    // Update is called once per frame
    void Update () {
	if(Time.frameCount % 100 == 0){
	    // setRandomRGB();
	}
    }
}
