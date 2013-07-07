using UnityEngine;
using System;
using System.Collections.Generic;

public class SpriteAnimation : MonoBehaviour
{
	[Serializable]
	public class Clip
	{
		[SerializeField]
		public int startFrame;

		[SerializeField]
		public int numFrames;

		[SerializeField]
		public int frameTime;

		[SerializeField]
		public WrapMode mode;

		public Clip(int startFrame, int numFrames, int frameTime, WrapMode mode)
		{
			this.startFrame = startFrame;
			this.numFrames = numFrames;
			this.frameTime = frameTime;
			this.mode = mode;
		}
	}

	private Dictionary<string, Clip> clipDictionary;

	private Material material;
	private string currentClipName;
	private Clip currentClip;
	private int currentFrame;

	private int nextFrameTime;

	private float frameWidth;

	public float totalFrames;
	public float framePercentHeight;
	
	private void Awake()
	{
		this.clipDictionary = new Dictionary<string, Clip>();

		if (this.renderer != null)
		{
			this.material = this.renderer.material;
			this.frameWidth = 1.0f / this.totalFrames;

			this.material.mainTextureScale = new Vector2(this.frameWidth, this.framePercentHeight);
		}
	}

	private void Update()
	{
		this.nextFrameTime -= Game.deltaTime();

		if (this.nextFrameTime <= 0)
		{
			++this.currentFrame;
			
			Clip curClip = this.currentClip;
			if (curClip == null)
			{
				Debug.LogError("Cur Clip is Null.");
			}

			if (this.currentFrame > this.currentClip.startFrame + this.currentClip.numFrames - 1)
			{
				switch (this.currentClip.mode)
				{
					case WrapMode.Loop:
						this.currentFrame = this.currentClip.startFrame;
						break;
				}
			}

			updateMaterial();

			this.nextFrameTime = this.currentClip.frameTime;
		}
	}

	private void updateMaterial()
	{
		this.material.mainTextureOffset = new Vector2(this.frameWidth * this.currentFrame, 0);
	}

	public void addClip(string name, Clip clip)
	{
		this.clipDictionary[name] = clip;
	}

	public void play(string clipName, bool randomStartFrame = false)
	{
		if (this.currentClipName != clipName && this.clipDictionary.ContainsKey(clipName))
		{
			this.currentClipName = clipName;
			this.currentClip = this.clipDictionary[clipName];

			this.currentFrame = this.currentClip.startFrame;
			this.nextFrameTime = 0;

			if (randomStartFrame)
			{
				this.currentFrame += UnityEngine.Random.Range(0, this.currentClip.numFrames);
			}
		}
	}
}