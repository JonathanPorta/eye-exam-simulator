using UnityEngine;
using System.Collections;

public class LensController : MonoBehaviour {

	public Camera myCamera;
	public float opacity = 0.0f;
	public float rate = .01f;

	public GameObject lensPlanePrefab;
	public GameObject lensCameraPrefab;

	public GameObject currentLensPlane;
	public GameObject currentLensCamera;

	public Shader shader;

	private GameObject newLensPlane, newLensCamera;
	private bool movingIntoPosition = false;
	private float startTime;
	private Vector3 newLensStartPoint, currentLensStartPoint, currentLensEndpoint;
	private float duration = 0.5f;
	private float lensYOffset = 1.0f;
	private Vector3 lensPlanePosition;
	

	private Material material;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer>().material;
		lensPlanePosition = currentLensPlane.transform.position;
		//lensYOffset = currentLensPlane.GetComponent<Renderer>().bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Jump") && newLensPlane == null && newLensCamera == null)
			AddLense();

		if (newLensPlane != null && newLensCamera != null) {
			if(!movingIntoPosition){
				newLensStartPoint = newLensPlane.transform.position;
				currentLensStartPoint = currentLensPlane.transform.position;
				currentLensEndpoint = new Vector3(currentLensStartPoint.x, currentLensStartPoint.y - lensYOffset, currentLensStartPoint.z);
				startTime = Time.time;
				movingIntoPosition = true;
			}
			else if(Time.time - startTime > duration){
				newLensPlane.transform.position = currentLensStartPoint;

				Destroy(currentLensPlane);
				Destroy(currentLensCamera);
				
				currentLensPlane = newLensPlane;
				currentLensCamera = newLensCamera;

				newLensPlane = null;
				newLensCamera = null;
				movingIntoPosition = false;
			}
			else {
				newLensPlane.transform.position = Vector3.Slerp(newLensStartPoint, currentLensStartPoint, (Time.time - startTime) / duration);
				currentLensPlane.transform.position = Vector3.Slerp(currentLensStartPoint, currentLensEndpoint, (Time.time - startTime) / duration);
			}
		}
	}

	void AddLense(){
		newLensPlane = Instantiate(lensPlanePrefab) as GameObject;//, currentLensPlane.transform.position, currentLensPlane.transform.rotation) as GameObject;
		newLensCamera = Instantiate(lensCameraPrefab, currentLensCamera.transform.position, currentLensCamera.transform.rotation) as GameObject;
		newLensPlane.transform.SetParent(transform);
		newLensCamera.transform.SetParent(transform);

		float startTime = Time.time;
		newLensPlane.transform.position = new Vector3(currentLensPlane.transform.position.x, currentLensPlane.transform.position.y + lensYOffset, currentLensPlane.transform.position.z);
		newLensPlane.transform.rotation = currentLensPlane.transform.rotation;

		// Hookup the new plane to use the rendertexture that hooks up to the new camera. New cool.
		RenderTexture rt = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32);
		Camera nlc = newLensCamera.GetComponent<Camera>();
		nlc.targetTexture = rt;
		//newLensPlane.GetComponent<Renderer>().material = Material(shader);
		newLensPlane.GetComponent<Renderer> ().material.mainTexture = rt;
		newLensCamera.GetComponent<UnityStandardAssets.ImageEffects.Blur>().enabled = true;
	}
}
