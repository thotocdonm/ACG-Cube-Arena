using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        SaveLoadManager.instance.LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
