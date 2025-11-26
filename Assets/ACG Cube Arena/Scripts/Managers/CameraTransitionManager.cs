using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTransitionManager : MonoBehaviour
{
    public static CameraTransitionManager instance;
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera gameplayCamera;
    [SerializeField] private CinemachineVirtualCamera mainMenuCamera;

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

    public void TranstionToGameplay()
    {
        gameplayCamera.Priority = 30;
    }
    
    public void TransitionToMainMenu()
    {
        gameplayCamera.Priority = 10;
    }
}
