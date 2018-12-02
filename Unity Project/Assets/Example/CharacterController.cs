using System;
using System.Collections;
using System.Collections.Generic;
using FarrokhGames.SpriteAnimation;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	ISpriteAnimator _animator;
	AudioSource _audioSource;

	[SerializeField] AudioClip[] _footsteps;

	void Awake()
	{
		// TODO: Make sure it auto plays first anim
		// TODO: Draw an idle animation

		_animator = GetComponent<ISpriteAnimator>();
		_animator.OnTrigger += HandleTrigger;

		_audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		var walkAxis = Input.GetAxis("Horizontal");

		if (walkAxis == 0)
		{
			_animator.Play("Idle");
		}
		else
		{
			_animator.Play("Walk");
			_animator.Flip = walkAxis < 0;
		}
	}

	void HandleTrigger(string trigger)
	{
		if (trigger == "footstep")
		{
			if (_footsteps.Length > 0)
			{
				var clip = _footsteps[UnityEngine.Random.Range(0, _footsteps.Length - 1)];
				_audioSource.PlayOneShot(clip);
			}
		}
	}
}