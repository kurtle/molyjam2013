using UnityEngine;
using System.Collections.Generic;

public class SpriteAnimation : MonoBehaviour
{
	private struct clip
	{
		public int startIndex;
	}

	private Dictionary<string,clip> clipDictionary;

	private Material material;

	public int totalFrames;

	
	private void Awake()
	{
		if (this.renderer != null)
		{
			this.material = this.renderer.material;
		}
	}

	private void Update()
	{
	
	}
}