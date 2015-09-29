using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UnityStandardAssets.ImageEffects.Blur))]
public class LensController : MonoBehaviour {

	public int rate = 1;
	public UnityStandardAssets.ImageEffects.Blur blurComponent;

	private Material material;
	private Lens[] lenses;

	// Use this for initialization
	void Start () {
		blurComponent = GetComponent<UnityStandardAssets.ImageEffects.Blur> ();
		lenses = new Lens[2];
		lenses[0] = new Lens(1);
		lenses[1] = new Lens(2);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetButton("ChangeLens")){
			SetLense(lenses[0]);
		}
		else if(Input.GetButton("ClearLens")){
			SetLense(null);
		}
	}

	void SetLense(Lens lens){
		if (lens != null) {
			blurComponent.enabled = true;
			blurComponent.iterations = lens.GetRate();
		} else {
			blurComponent.enabled = false;	
		}
	}
}
