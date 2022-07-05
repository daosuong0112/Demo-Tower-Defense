using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroInfo {
    public HeroConfig HeroConfig;
    public int HeroID;
    public TeamType TeamType;
    //data
    public bool IsDie;
    public float HP;
    public float Damage;
    public float Def;
    //
    public float MaxHP;
    public float MaxDamage;
    public float MaxDef;

    public HeroInfo(int heroConfigID , int heroId, TeamType teamType)
    {
        this.HeroConfig = HeroConfigManager.Instance.GetHeroConfig(heroConfigID);
        this.HeroID = heroId;
        this.TeamType = teamType;
        this.ResetInfo();
    }

    public void ResetInfo()
    {
        this.IsDie = false;
        this.HP = this.HeroConfig.HP;
        this.Damage = this.HeroConfig.Damage;
        this.Def = this.HeroConfig.Def;
        //
        this.MaxHP = this.HeroConfig.HP;
        this.MaxDamage = this.HeroConfig.Damage;
        this.MaxDef = this.HeroConfig.Def;
    }
}
