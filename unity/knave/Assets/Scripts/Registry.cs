using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Registry : MonoBehaviour
{
	private static Registry _instance;

	public Player player;

	public King king;
	
	public Police police1;
	public Police police2;
	public Police police3;
	
	public List<Police> policeList = new List<Police>();
	
	public Townsfolk townsfolk;
	public Drunk drunk;
	
	private void Awake()
	{
		_instance = this;
		policeList.Add(police1);
		policeList.Add(police2);
		policeList.Add(police3);

	}

	public static Registry Instance
	{
		get
		{
			return _instance;
		}
	}
}