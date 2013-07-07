using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	public float speed;

	public void moveDelta(Vector3 delta)
	{
		this.transform.localPosition += this.speed * delta;
	}
}