using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [Header("Game Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject itemDetailPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowShopPanel()
    {
        shopPanel.SetActive(true);
    }

    public void HideShopPanel()
    {
        shopPanel.SetActive(false);
    }

    public void ShowItemDetailPanel()
    {
        GameStateManager.instance.ChangeGameState(GameState.Shopping);
        itemDetailPanel.SetActive(true);
    }
    
    public void HideItemDetailPanel()
    {
        GameStateManager.instance.ChangeGameState(GameState.Game);
        itemDetailPanel.SetActive(false);
    }
}
