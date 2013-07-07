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
		
		if (Input.GetKey(KeyCode.E))
		{
			if ((Registry.Instance.townsfolk.transform.position - this.transform.position).magnitude < 100)
			{
				Registry.Instance.townsfolk.stealFrom();
			}
		}

		this.moveDelta(inputDelta.normalized);
	}
}