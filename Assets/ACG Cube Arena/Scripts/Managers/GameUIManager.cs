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
        GameStateManager.instance.ChangeGameState(GameState.Shopping);
        shopPanel.SetActive(true);
        ShopManager.instance.GenerateItems();
    }

    public void HideShopPanel()
    {
        GameStateManager.instance.ChangeGameState(GameState.Game);
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
