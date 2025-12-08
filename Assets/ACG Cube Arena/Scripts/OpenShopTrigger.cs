using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopTrigger : MonoBehaviour
{
    public static OpenShopTrigger instance;

    private bool isShopOpen = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isShopOpen)
        {
            GameUIManager.instance.ShowShopPanel();
            isShopOpen = true;
        }
    }
    
    public void ResetShopTrigger()
    {
        isShopOpen = false;
    }
}
