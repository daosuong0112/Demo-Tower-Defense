using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TeamInfo {
	public static int countHeroId;
	//
	public List<HeroInfo> HeroInfos;
	public List<int> HeroConfigList;
	public TeamType TeamType;
	public TeamInfo(List<int> listHeroConfigID, TeamType teamType)
	{
		this.HeroConfigList = listHeroConfigID;
		this.TeamType = teamType;
		this.HeroInfos = new List<HeroInfo>();
	}



	#region Hero
	public HeroInfo GetHeroInfo(int heroId)
	{
		return this.HeroInfos.First(r => r.HeroID == heroId);
	}
	public bool CheckExistHeroConfig(int heroConfigId)
	{
		return this.HeroConfigList.Exists(r => r == heroConfigId);
	}

	public HeroInfo CreateHeroInfo(int heroConfigId)
	{
		HeroInfo heroInfo = new HeroInfo(heroConfigId, ++countHeroId, this.TeamType);
		this.HeroInfos.Add(heroInfo);
		return heroInfo;
	}

	#endregion
}
