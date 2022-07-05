using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Team
public enum TeamType
{
    TeamRight,
    TeamLeft
}
#endregion

#region Skill
public enum SkillType
{
    NormalAttack,
    SpecialSkill,
}
public enum SkillRangeType
{
    Melee,
    Ranged
}
public enum SkillEffectType
{
    Damage,
    HP,
    HPTargetMaxRatio,
    HPDamage,

}
public enum SkillTargetType
{
    MySelf,
    Ally,
    SingleEnemy,
    AllEnemy,
}
public enum ResultSkillAction
{
    Activing, 
    CanActive,
    WaitCountdown,
        OverRange
}
#endregion

#region view
public enum ViewType
{
    Hero,
    Tree, 
    Building
}
#endregion
#region enum
public enum HPType
{
    Reduce,
    Receive
}
#endregion