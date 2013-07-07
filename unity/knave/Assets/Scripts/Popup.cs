using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour
{
	public SpriteAnimation spriteAnimation;

	public SpriteAnimation.Clip[] clips;

	private void Start()
	{
		for (int i = 0; i < clips.Length; ++i)
		{
			this.spriteAnimation.addClip("" + i, this.clips[i]);
		}
		this.spriteAnimation.play("0");
	}

	public void playClip(uint clipIndex)
	{
		if (clipIndex < 0 || clipIndex > this.clips.Length)
		{
			Debug.LogError("BAD CLIP INDEX FOR POPUP " + this);

			return;
		}

		this.spriteAnimation.play("" + clipIndex);
	}
}