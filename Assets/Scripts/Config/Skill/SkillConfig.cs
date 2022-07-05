using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillConfig {
	public int SkillConfigId;
	public SkillType skillType;
	public SkillRangeType SkillRangeType;
	public string FuncSkillName;
	public float RangeAttack;
	public float CountDown;
	public float ActiveTime;
	public string TriggerName;
	//
	public SkillTargetType SkillTargetType;
	//
	public List<SkillEffectConfig> SkillEffectConfigs;
}
