using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour {
	public int MyHeroId;
	public TeamType MyTeam;
	public SkillConfig SkillConfig;
	public ITargetSkill MyTargetHero;
	[SerializeField] private float currentCountdown;
	[SerializeField] private bool isActiving;
	private ITargetSkill mainTargetSkill;
	private AnimatorHeroView animatorHeroView;
	public virtual void Init(int myHeroId, TeamType myTeam, ITargetSkill myHero, SkillConfig skillConfig)
	{
		this.MyHeroId = myHeroId;
		this.MyTeam = myTeam;
		this.MyTargetHero = myHero;
		this.SkillConfig = skillConfig;
		this.animatorHeroView = GetComponentInParent<AnimatorHeroView>();
	}
	public virtual void EnterSkill()
	{
		//
		isActiving = true;
		//
		this.ActiveSkill();
	}
	public virtual ResultSkillAction UpdateSkill(ITargetSkill targetSkill = null)
	{
		if(this.mainTargetSkill == null)
		{
			this.mainTargetSkill = targetSkill;
		}
		//Debug.Log("targetSkill " + targetSkill);
		//Debug.Log("distance " + Vector3.Distance(this.MyTargetHero.GetTransform.position, targetSkill.GetTransform.position));

		if (targetSkill != null)
		{
			var distance = Vector3.Distance(this.MyTargetHero.GetTransform.position, targetSkill.GetTransform.position);
			if (distance > this.SkillConfig.RangeAttack)
			{
				return ResultSkillAction.OverRange;
			}
		}
		if (isActiving)
		{
			return ResultSkillAction.Activing;
		}
		if (this.canActiveSkill())
		{
			this.EnterSkill();
			return ResultSkillAction.CanActive;
		}
		return ResultSkillAction.WaitCountdown;
	}

	public virtual void ActiveSkill()
	{
		this.StartCoroutine(this.doDelayActiveSkill());
	}
	public virtual void ExitSkill()
	{
		isActiving = false;
	}
	//

	//
	void Update()
	{
		this.currentCountdown += Time.deltaTime;
	}

	private bool canActiveSkill()
	{
		return this.currentCountdown >= this.SkillConfig.CountDown;
	}

	protected virtual IEnumerator doDelayActiveSkill()
	{
		Debug.Log("doDelayActiveSkill ");
		//
		this.animatorHeroView.SetTrigger(this.SkillConfig.TriggerName);
		yield return new WaitForSeconds(this.SkillConfig.ActiveTime);

		//
	List<ITargetSkill> targetSkills = new List<ITargetSkill>();
	//
	switch (this.SkillConfig.SkillTargetType)
	{
		case SkillTargetType.AllEnemy:

			break;
		case SkillTargetType.Ally:

			break;
		case SkillTargetType.MySelf:
			targetSkills.Add(MyTargetHero);
			break;
		case SkillTargetType.SingleEnemy:
			targetSkills.Add(this.mainTargetSkill);
			break;
	}
		//
	BattleControl.Instance.RequestActiveSkill(this.MyTargetHero.Id,this.MyTargetHero.TeamType, targetSkills, this.SkillConfig);
	}
}
