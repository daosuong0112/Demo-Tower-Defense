using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHeroView : MonoBehaviour {
	[SerializeField] private Animator animator;
	void Awake()
	{
		this.animator = GetComponent<Animator>();
	}


	public void SetIdle()
	{
		this.animator.SetTrigger("Idle");
	}
	public void SetRun()
	{
		this.animator.SetTrigger("Run");
	}
	public void SetDeath()
	{
		this.animator.SetTrigger("Death");
	}
	public void SetTrigger(string triggerName)
	{
		this.animator.SetTrigger(triggerName);
	}
}
