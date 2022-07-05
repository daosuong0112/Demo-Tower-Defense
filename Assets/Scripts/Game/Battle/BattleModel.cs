/*using NUnit.Framework;
*/using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleModel : MonoSingleton<BattleModel> {

    #region Params
    public List<TeamInfo> teamInfos;
    #endregion
    #region Init
    public void Init()
    {

    }
    #endregion
    #region Team
    public void CreateTeam(List<int> teamLeft, List<int> teamRight)
    {
        this.teamInfos = new List<TeamInfo>();
        //right
        TeamInfo teamR = new TeamInfo(teamRight, TeamType.TeamRight);
        this.teamInfos.Add(teamR);
        //left
        TeamInfo teamL = new TeamInfo(teamLeft, TeamType.TeamLeft);
        this.teamInfos.Add(teamL);
    }
    public TeamInfo GetTeamInfo(TeamType teamType)
    {
        return this.teamInfos.First(r => r.TeamType == teamType);

    }
    #endregion
    #region Hero
    public HeroInfo CreateHeroInfo(int heroConfigID, TeamType targetTeam)
    {
        TeamInfo targetteamInfo = this.GetTeamInfo(targetTeam);
        Assert.IsTrue(targetteamInfo != null, "team info is null");
        if(targetteamInfo.CheckExistHeroConfig(heroConfigID))
        {
            return targetteamInfo.CreateHeroInfo(heroConfigID);
        }
        return null;
    }
    public HeroInfo GetHeroInfo(int heroId, TeamType teamType)
    {
        return this.GetTeamInfo(teamType).GetHeroInfo(heroId);
    }
    #endregion
}
