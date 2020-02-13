using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeathScreen : MonoBehaviour
{
	public PlayableDirector director;
	public float skipDelay;

	private bool playing;
	private float unlockSkipAt;

	private void Start()
	{
		PlayerCharacter.inst.OnDeath += Inst_OnDeath;
	}

	private void Update()
	{
		if (playing && Time.time > unlockSkipAt && Input.GetKeyDown(KeyCode.Space))
		{
			director.Pause();
			director.time = director.playableAsset.duration;
			director.Resume();
		}
	}

	private void Inst_OnDeath(object sender, System.EventArgs e)
	{
		director.Play();
		playing = true;
		unlockSkipAt = Time.time + skipDelay;
	}
}
