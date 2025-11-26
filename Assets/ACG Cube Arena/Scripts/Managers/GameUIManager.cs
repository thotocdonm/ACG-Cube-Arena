using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    private Stack<GameObject> panelStack = new Stack<GameObject>();

    [Header("Game Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject itemDetailPanel;

    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;

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

    public void TogglePausePanel()
    {
        if (GameStateManager.instance.CurrentGameState == GameState.Pause)
        {
            GameStateManager.instance.ChangeGameState(GameState.Game);
            pausePanel.SetActive(false);
        }
        else
        {
            GameStateManager.instance.ChangeGameState(GameState.Pause);
            pausePanel.SetActive(true);
        }
    }

    public void ShowPanel(GameObject panel)
    {
        if (panelStack.Count == 0 && GameStateManager.instance.CurrentGameState == GameState.Game)
        {
            GameStateManager.instance.ChangeGameState(GameState.Pause);
        }
        else
        {
            panelStack.Peek().SetActive(false);
        }
        panelStack.Push(panel);
        Debug.Log("PanelStack: " + panelStack.Count);
        panel.SetActive(true);
    }

    public void GoBack()
    {
        if (panelStack.Count == 0) return;

        GameObject currentPanel = panelStack.Pop();
        currentPanel.SetActive(false);
        if (panelStack.Count > 0)
        {
            GameObject previousPanel = panelStack.Peek();
            previousPanel.SetActive(true);
        }
        else
        {
            if (GameStateManager.instance.CurrentGameState == GameState.Pause)
            {
                GameStateManager.instance.ChangeGameState(GameState.Game);
            }
        }
    }
    
    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
           if(GameStateManager.instance.CurrentGameState == GameState.Game)
            {
                ShowPanel(pausePanel);
            }
            else
            {
                GoBack();
            } 
        }
            

        
    }
}
