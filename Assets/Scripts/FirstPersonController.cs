using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
	
	public float movementSpeed = 10.0f;
	public float mouseSensitivity = 5.0f;
	public float upDownRange = 60.0f;
	public float jumpSpeed = 20.0f;
	public float sprintMultiplier = 10.5f;
	
	private float verticalRotation = 0;
	private float verticalVelocity = 0;
	CharacterController characterController;
	
	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update() {
		
		// Left Right Camera Rotation
		float leftRightRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, leftRightRotation, 0);
		
		// Up Down Camera Rotation
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
		
		// Move Forward, Backward, Left, and Right
		
		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float leftRightSpeed = Input.GetAxis("Horizontal")  * movementSpeed;
		
		if(Input.GetButton("Sprint")){
			forwardSpeed = forwardSpeed * sprintMultiplier;
			leftRightSpeed = leftRightSpeed * sprintMultiplier;
			
		}
		
		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		
		if(characterController.isGrounded && Input.GetButtonDown("Jump")) {
			verticalVelocity = jumpSpeed;
		}
		
		Vector3 speed = new Vector3(leftRightSpeed, verticalVelocity, forwardSpeed);
		speed = transform.rotation * speed;
		
		characterController.Move(speed * Time.deltaTime);
	}
}