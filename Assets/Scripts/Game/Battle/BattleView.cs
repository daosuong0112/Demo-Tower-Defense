using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BattleView : MonoBehaviour {

    #region params
    [SerializeField] private TeamInfoView[] teamArray;
    [SerializeField] private Transform heroParent;
    #endregion
    #region Init
    void Awake()
    {
        this.initComponent();
        this.registerAction();
    }
    void Start()
    {
        BattleControl.Instance.Init(teamArray);
    }
    private void initComponent()
    {

    }
    private void registerAction()
    {
        BattleControl.Instance.OnCreateHeroRandom += OnCreateHeroRandom;
        BattleControl.Instance.OnCreateNewHero += OnCreateNewHero;
    }



    #endregion

    #region Action
    private void OnCreateHeroRandom(TeamType myTeam)
    {
        this.generateRandomHero(myTeam);
    }

    private void OnCreateNewHero(int heroId, int heroConfigID, TeamType heroTeamType)
    {
        this.createNewHero(heroId, heroConfigID, heroTeamType);
    }
    #endregion

    #region generate hero
    private void generateRandomHero(TeamType heroTeamType)
    {
        Debug.Log("myTeam");
        var prefabMic = Resources.Load<Transform>("Enemy");
        //var prefabTralph = Resources.Load<Transform>("Roles/Tralph");
        TeamInfoView enemyTeam = this.getEnemyTeam(heroTeamType);
        //generate
        Transform m1 = Instantiate(prefabMic, enemyTeam.Transform);
        //Transform t1 = Instantiate(prefabTralph, enemyTeam.Transform);
        //
        Vector3 addVector = new Vector3(UnityEngine.Random.Range(1, 5), 0, UnityEngine.Random.Range(-5, -1));
        m1.position = enemyTeam.Transform.position + addVector;
        //m1.GetComponent<HeroView>().InitView(enemyId, enemyConfigID, enemyTeam.TeamType, heroTeam.Transform.position);
    }
    private void createNewHero(int heroId, int heroConfigID, TeamType heroTeamType)
    {
        TeamInfoView targetTeam = this.getMyTeam(heroTeamType);
        TeamInfoView targetEnemyTeam = this.getEnemyTeam(heroTeamType);

        if (heroConfigID < 9)
        {
            //create hero
            HeroConfig config = HeroConfigManager.Instance.GetHeroConfig(heroConfigID);
            var prefabHero = Resources.Load<Transform>(config.BundlePath);
            Transform heroView = Instantiate(prefabHero, targetTeam.Transform);
            Vector3 addVector = new Vector3(UnityEngine.Random.Range(-5, -1), 0, UnityEngine.Random.Range(1, 5));
            heroView.position = targetTeam.Transform.position + addVector;
            heroView.GetComponent<HeroView>().InitView(heroId, heroConfigID, heroTeamType, targetEnemyTeam.Transform.position);

        } else
        {

            var prefabEnemy = Resources.Load<Transform>("EnemyPrefab_" + heroConfigID);
            //
            Transform enemyView = Instantiate(prefabEnemy, targetEnemyTeam.Transform);
            //random position
            //
            Vector3 addVectorEnemy = new Vector3(UnityEngine.Random.Range(1, 5), 0, UnityEngine.Random.Range(-5, -1));
            enemyView.position = targetEnemyTeam.Transform.position + addVectorEnemy;
            enemyView.GetComponent<HeroView>().InitView(heroId, heroConfigID, targetEnemyTeam.TeamType, targetTeam.Transform.position);
        }
        
    }
    #endregion
    #region Team
    private TeamInfoView getMyTeam(TeamType team)
    {
        return teamArray.First(r => r.TeamType == team);
    }
    private TeamInfoView getEnemyTeam(TeamType team)
    {
        return teamArray.First(r => r.TeamType != team);
    }
    #endregion
    #region Gizmos
    void OnDrawGizmos()
    {
        foreach(var team in this.teamArray)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
            Gizmos.DrawSphere(team.Transform.position, team.Radius);
            Gizmos.color = oldColor;

        }
    }
    #endregion
}
