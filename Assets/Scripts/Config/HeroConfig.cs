using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HeroConfig",menuName ="HeroConfig")]
public class HeroConfig : ScriptableObject{
    public float HeroConfigId;
    public string HeroName;
    public string BundlePath;
    public float MoveSpeed;
    //
    public float HP;
    public float Damage;
    public float Def; 
    //
    public float RangeFollowTarget;
    public LayerMask EnemyLayer;
    //Skill
    public List<SkillConfig> SkillConfigs;
}
