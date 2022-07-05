using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HeroView : MonoBehaviour, ITargetSkill {

    #region params
    public bool IsFake;
    [SerializeField] private HeroConfig heroConfig;
    public int HeroId;
    public TeamType TeamType;
    public ITargetSkill MyTarget;
    public HealthBarHero healthBar;
    //Animator
    [SerializeField] private AnimatorHeroView animatorHeroView;
    //Skill
    private List<SkillBase> skillBases;
    private SkillBase normalAttack;
    //[SerializeField] int layer;
    //
    [Header("NavMesh")]
    private Vector3 buidingPostion;
    [SerializeField] private NavMeshAgent meshAgent;

    public Transform GetTransform
    {
        get
        {
            return this.transform;
        }
    }

    public ViewType viewType
    {
        get
        {
            return ViewType.Hero;
        }
    }

    public int Id { get { return this.HeroId; } }

    TeamType ITargetSkill.TeamType { get { return this.TeamType; } }

    #endregion
    #region init
    void Awake()
    {
        this.initComponent();
        this.registerAction();
    }
    public void InitView(int heroId, int heroConfigID, TeamType heroTeamType, Vector3 buidingPosition)
    {
        this.heroConfig = HeroConfigManager.Instance.GetHeroConfig(heroConfigID);
        this.HeroId = heroId;
        this.TeamType = heroTeamType;
        this.buidingPostion = buidingPosition;
        this.buidingPostion.y = 0;
        this.MyTarget = this.GetComponent<ITargetSkill>();
        this.healthBar.setMaxHealth(this.heroConfig.HP);
        //this.layer = (int)Mathf.Log(this.heroConfig.EnemyLayer.value, 2);
        //
        meshAgent.speed = heroConfig.MoveSpeed;
        this.animatorHeroView.SetRun();
        this.setDestination(buidingPosition);
        //
        this.setupSkill();
    }
    private void setupSkill()
    {
        this.skillBases = new List<SkillBase>();
        foreach (var skill in this.heroConfig.SkillConfigs)
        {
            GameObject objSkill = new GameObject();
            objSkill.transform.SetParent(this.transform);
            objSkill.name = skill.FuncSkillName;
            Type skillComponent = System.Type.GetType(skill.FuncSkillName);
            var skillView = objSkill.AddComponent(skillComponent) as SkillBase;
            skillView.Init(this.HeroId, this.TeamType, MyTarget,  skill);
            this.skillBases.Add(skillView);
        }

        //
        this.normalAttack = this.GetSkill(SkillType.NormalAttack);
    }
    private void initComponent()
    {
        this.animatorHeroView = GetComponent<AnimatorHeroView>();
    }
    private void registerAction()
    {
        BattleControl.Instance.OnHeroDie += OnHeroDie;
        BattleControl.Instance.OnUpdateHeroHP += OnUpdateHeroHP;
        BattleControl.Instance.OnHeroRun += OnHeroRun;
    }


    #endregion
    #region Action
    private void OnUpdateHeroHP(int heroId, TeamType teamType, HPType hPType, float currentHP)
    {
        //
        if (this.HeroId == heroId && this.TeamType == teamType)
        {
            switch (hPType)
            {
                case HPType.Reduce:
                    healthBar.setHealth(currentHP);
                    break;
                case HPType.Receive:
                    healthBar.setHealth(currentHP);
                    break;
            }
        }
        Debug.LogError(currentHP);
    }

    private void OnHeroDie(int heroId, TeamType teamType)
    {
        //
        if (this.HeroId == heroId && this.TeamType == teamType)
        {
            healthBar.setHealth(0);
            DestroyImmediate(this.gameObject);
        }
    }

    private void OnHeroRun(int heroId, TeamType teamType)
    {
        //
        if (this.HeroId == heroId && this.TeamType == teamType)
        {
            this.animatorHeroView.SetRun();
        }
    }
    #endregion
    #region View
    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, this.heroConfig.RangeFollowTarget, this.heroConfig.EnemyLayer.value);
        if (cols != null && cols.Length > 0)
        {
            var Itarget = cols[0].transform.GetComponent<ITargetSkill>();
            ResultSkillAction result = this.normalAttack.UpdateSkill(Itarget);
            Debug.Log("result " + result);
            switch (result)
            {
                case ResultSkillAction.Activing:
                    this.meshAgent.enabled = false;
                    break;
                case ResultSkillAction.CanActive:
                    this.meshAgent.enabled = false;
                    break;
                case ResultSkillAction.OverRange:
                    this.setDestination(cols[0].transform.position);
                    break;
                case ResultSkillAction.WaitCountdown:
                    this.meshAgent.enabled = false;
                    break;
            }
            IsFake = false;
        }
        else if (!IsFake)
        {
            IsFake = true;
            this.setDestination(buidingPostion);
        }
    }
        
        void OnDrawGizmos()
    {
        if(this.heroConfig != null)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.heroConfig.RangeFollowTarget);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, this.normalAttack.SkillConfig.RangeAttack);


            Gizmos.color = oldColor;
        }
        
    }
    #endregion
    #region Skill
    private SkillBase GetSkill(SkillType skillType)
    {
       return this.skillBases.First(r => r.SkillConfig.skillType == skillType);
    }

    #endregion
    #region Movement
    private void setDestination(Vector3 target)
    {
        this.meshAgent.enabled = true;
        meshAgent.Warp(this.transform.position);
        meshAgent.SetDestination(target);
    }
    #endregion
}
