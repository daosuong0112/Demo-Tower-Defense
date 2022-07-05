using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroConfigManager : MonoSingleton<HeroConfigManager> {
	[SerializeField] private List<HeroConfig> heroConfigs;

	public HeroConfig GetHeroConfig(int heroConfigId)
	{
		return this.heroConfigs.First(r => r.HeroConfigId == heroConfigId);
	}
	
}
