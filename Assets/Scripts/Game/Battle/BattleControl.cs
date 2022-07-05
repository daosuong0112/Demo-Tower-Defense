using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;

public class BattleControl : ApiSingleton<BattleControl> {

    #region Action
    public Action<TeamType> OnCreateHeroRandom;
    public Action<List<int>, TeamType> OnCreateHeroButton;
    public Action OnCreateHeroFail;
    public Action<int, int, TeamType> OnCreateNewHero;
    public Action<int, TeamType> OnHeroDie;
    public Action<int, TeamType> OnHeroRun;
    public Action<int, TeamType, HPType, float> OnUpdateHeroHP;
    public TeamType myTeam;
    #endregion

    #region Init
    public void Init(TeamInfoView[] teamArray)
    {
        BattleModel.Instance.Init();
        //
        //this.fakeNotifyGenerateHeroRandom();
        //this.fakeLoadTeamParams();
        this.loadTeamParams(teamArray);
    }

    #endregion

    #region Request
    public void RequestCreateHero(int heroConfigID, TeamType targetTeam)
    {
        HeroInfo newHeroInfo =  BattleModel.Instance.CreateHeroInfo(heroConfigID, targetTeam);
        HeroInfo newEnemyInfo = BattleModel.Instance.CreateHeroInfo(heroConfigID + 10, targetTeam == TeamType.TeamLeft ? TeamType.TeamRight : TeamType.TeamLeft);
        if (newHeroInfo != null)
        {
            this.notifyOnCreateNewHero(newHeroInfo.HeroID, heroConfigID, targetTeam);
            this.notifyOnCreateNewHero(newEnemyInfo.HeroID, heroConfigID + 10, targetTeam);
            //this.notifyOnCreateHeroRandom();
        }
        else
        {
            //
        }
    }
    #endregion
    #region Skill
    public void RequestActiveSkill(int myHeroId, TeamType myteam, List<ITargetSkill> targetSkills, SkillConfig skillConfig)
    {
        HeroInfo myHero = BattleModel.Instance.GetHeroInfo(myHeroId, myteam);
        if (!myHero.IsDie)
        {
            foreach(var iTarget in targetSkills)
            {
                HeroInfo target = BattleModel.Instance.GetHeroInfo(iTarget.Id, iTarget.TeamType);
                
                    foreach (var effectSkill in skillConfig.SkillEffectConfigs)
                    {
                        switch (effectSkill.SkillEffectType)
                        {
                            case SkillEffectType.Damage:
                                float totalDamage = effectSkill.RatioBaseDamage * myHero.Damage + effectSkill.EffectValue;

                                if (!target.IsDie)
                                {
                                    target.HP -= (totalDamage - target.Def);

                                    if (target.HP <= 0)
                                    {
                                        target.HP = 0;
                                        target.IsDie = true;
                                        //notify hero die
                                        Debug.Log("Hero Die!");
                                        this.notifyOnHeroDie(target.HeroID, target.TeamType);
                                        this.notifyOnHeroRun(myHero.HeroID, myHero.TeamType);
                                    }
                                    else
                                    {
                                        //notify update hp
                                        Debug.Log("Update HP!");

                                        this.notifyOnUpdateHeroHP(target.HeroID, target.TeamType, HPType.Reduce, target.HP);
                                    }

                                }
                                break;
                            case SkillEffectType.HP:
                                float totalReceiveHP = effectSkill.EffectValue;
                                if (!target.IsDie)
                                {
                                    target.HP += totalReceiveHP;
                                    if (target.HP >= target.MaxHP)
                                    {
                                        target.HP = target.MaxHP;
                                    }
                                    //notify update hp
                                    this.notifyOnUpdateHeroHP(target.HeroID, target.TeamType, HPType.Receive, target.HP);

                                }
                                break;
                            case SkillEffectType.HPTargetMaxRatio:
                                float totalReceiveHP2 = effectSkill.EffectValue * target.MaxDamage;
                                if (!target.IsDie)
                                {
                                    target.HP += totalReceiveHP2;
                                    //notify update hp
                                    this.notifyOnUpdateHeroHP(target.HeroID, target.TeamType, HPType.Receive, target.HP);

                                }
                                break;
                            case SkillEffectType.HPDamage:
                                float totalDamage2 = effectSkill.RatioBaseDamage * myHero.Damage;
                                float realDamage = totalDamage2 - target.Def;
                                float HPReceive2 = realDamage * effectSkill.EffectValue;
                                //Step1
                                target.HP -= realDamage;
                                if (target.HP <= 0)
                                {
                                    target.HP = 0;
                                    target.IsDie = true;
                                    //notify hero die
                                    this.notifyOnHeroDie(target.HeroID, target.TeamType);
                                    this.notifyOnHeroRun(myHero.HeroID, myHero.TeamType);
                                }
                                else
                                {
                                    //notify update hp
                                    this.notifyOnUpdateHeroHP(target.HeroID, target.TeamType, HPType.Reduce, target.HP);
                                }

                                //step 2
                                if (!myHero.IsDie)
                                {
                                    myHero.HP += HPReceive2;
                                    //notify update hp
                                    this.notifyOnUpdateHeroHP(myHero.HeroID, myHero.TeamType, HPType.Receive, target.HP);
                                }
                                break;
                        }
                    
                }
            }
        }
    }
    #endregion
    #region Generate Hero

    private void loadTeamParams(TeamInfoView[] teamArray)
    {
        List<int> team = teamArray.First(r => r.TeamType == myTeam).heroId;
        BattleModel.Instance.CreateTeam(teamArray.First(r => r.TeamType != myTeam).heroId, team);
        this.notifyOnCreateHeroButton(team, myTeam);
    }

    private void fakeLoadTeamParams()
    {
        List<int> teamLeft = new List<int>() { 0 };
        List<int> teamRight = new List<int>() { 0, 1 };

        //create team
        BattleModel.Instance.CreateTeam(teamLeft, teamRight);
        //
        this.notifyOnCreateHeroButton(teamLeft, TeamType.TeamLeft);
        this.notifyOnCreateHeroButton(teamRight, TeamType.TeamRight);
    }
    private void fakeNotifyGenerateHeroRandom()
    {
        this.notifyOnCreateHeroRandom();
    }
    #endregion

    #region Notify
    private void notifyOnCreateHeroRandom()
    {
        if (OnCreateHeroRandom != null)
        {
            OnCreateHeroRandom.Invoke(myTeam);
        }
    }
    private void notifyOnCreateHeroButton(List<int> heroConfig, TeamType teamType)
    {
        if(OnCreateHeroButton != null)
        {
            OnCreateHeroButton.Invoke(heroConfig, teamType);
        }
    }
    private void notifyOnCreateNewHero(int heroId, int heroConfigID, TeamType heroTeamType)
    {
        if(OnCreateNewHero != null)
        {
            OnCreateNewHero(heroId, heroConfigID, heroTeamType);
        }
    }
    private void notifyOnHeroDie(int heroId, TeamType teamType)
    {
        if(OnHeroDie != null)
        {
            OnHeroDie(heroId, teamType);
        }
    }

    private void notifyOnHeroRun(int heroId, TeamType teamType)
    {
        if (OnHeroRun != null)
        {
            OnHeroRun(heroId, teamType);
        }
    }
    private void notifyOnUpdateHeroHP(int heroId, TeamType teamType, HPType hPType, float currentHP)
    {
        if(OnUpdateHeroHP != null)
        {
            OnUpdateHeroHP(heroId, teamType, hPType, currentHP);
        }
    }
    #endregion
}
