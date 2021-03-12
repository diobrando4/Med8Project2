using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicFPSCameraScript : MonoBehaviour
{
	private float mouseX;
	private float mouseY;
	public Vector3 deltaRotation;
	public float mouseSensitivity = 1;
	public Transform playerBody;
	public Rigidbody rb;
	private float xAxisClamp = 0.0f;
	private Vector3 myPos;
	private Vector3 PlayerPos;
	private Vector3 differencePos;

	void Awake()
	{
		Debug.LogWarning("Unlock cursor press q ");
		Cursor.lockState = CursorLockMode.Locked;
	}
	void Start()
	{
		//rb.GetComponent<Rigidbody>().rotation = Quaternion.identity;
		myPos = GetComponent<Transform>().position;
		PlayerPos = rb.GetComponent<Transform>().position;
		differencePos = PlayerPos - myPos;
	}
	void unlockMouse()
	{
		if (Input.GetKeyDown("q"))
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Update is called once per frame
	void Update()
	{
		rotateCamra();
		unlockMouse();
		
	}

	//This method does that when the mouse turns the character body rotates with the camra
	void rotateCamra()
	{

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		float rotAmountX = mouseX * mouseSensitivity;
		float rotAmountY = mouseY * mouseSensitivity;

		xAxisClamp -= rotAmountY;

		Vector3 targetRotationCamra = transform.rotation.eulerAngles;
		Vector3 targetRotationBody = rb.rotation.eulerAngles;

		targetRotationCamra.x -= rotAmountY;//invert the input = -=
		targetRotationBody.y += rotAmountX; //rotates the body
		targetRotationCamra.z = 0; // no cam flip

		//locks the camra rotation's  x coordinat between -90 and 90 degrees 
		// look at the 3D camera degress
		if (xAxisClamp > 90)
		{
			xAxisClamp = 90;
			targetRotationCamra.x = 90;

		}
		else if (xAxisClamp < -90)
		{

			xAxisClamp = -90;
			targetRotationCamra.x = 270;
		}

		transform.rotation = Quaternion.Euler(targetRotationCamra);
		//deltaRotation = Quaternion.Euler(targetRotationBody * Time.deltaTime);
		//playerRB.rotation = Quaternion.Euler(targetRotationBody);
		rb.MoveRotation(Quaternion.Euler(targetRotationBody));


	}

	
}
