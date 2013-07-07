using UnityEngine;
using System.Collections;

public class Registry : MonoBehaviour
{
	private static Registry _instance;

	public Player player;

	public King king;

	private void Awake()
	{
		_instance = this;
	}

	public static Registry Instance
	{
		get
		{
			return _instance;
		}
	}
}