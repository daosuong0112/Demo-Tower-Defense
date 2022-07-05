using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasView : MonoBehaviour {

    #region params
    [SerializeField] private TeamType myTeam;
    [SerializeField] private Transform parentButton;
    #endregion
    #region Init
    void Awake()
    {
        this.initComponent();
        this.registerAction();
    }
    void Start()
    {
    }
    private void initComponent()
    {

    }
    private void registerAction()
    {
        BattleControl.Instance.OnCreateHeroButton += OnCreateHeroButton;
        BattleControl.Instance.myTeam = myTeam;
    }



    #endregion

    #region Action

    private void OnCreateHeroButton(List<int> heroConfig, TeamType teamType)
    {

        if (teamType == this.myTeam)
        {
            this.createHeroButton(heroConfig);
        }
    }
    #endregion
    #region create hero button
    private void createHeroButton(List<int> heroConfig)
    {
        var btnPre = Resources.Load<GameObject>("UI/HeroButton");

        foreach(var heroId in heroConfig)
        {
            /*Sprite iconSprite = Resources.Load<Sprite>("Icon/heroicon_" + heroId);*/
            var btnObj = Instantiate<GameObject>(btnPre, parentButton);
            Button btn = btnObj.GetComponent<Button>();
            /*btnObj.GetComponent<Image>().sprite  = iconSprite;*/

            btnObj.GetComponent<Image>().color = Resources.Load<GameObject>("Hero_" + heroId).GetComponent<Renderer>().sharedMaterial.color;
            btn.onClick.AddListener(() => clickCreateHero(heroId));
        }
    }
    private void clickCreateHero(int heroId)
    {
        Debug.Log("clickCreateHero "+heroId);
        BattleControl.Instance.RequestCreateHero(heroId, this.myTeam);
    }
    #endregion
}
