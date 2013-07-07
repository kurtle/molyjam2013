using UnityEngine;
using System.Collections;

public class Player : Actor 
{
	private void FixedUpdate()
	{
		Vector3 inputDelta = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			inputDelta.z += 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			inputDelta.x -= 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			inputDelta.z -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			inputDelta.x += 1;
		}

		this.moveDelta(inputDelta.normalized);
	}
}