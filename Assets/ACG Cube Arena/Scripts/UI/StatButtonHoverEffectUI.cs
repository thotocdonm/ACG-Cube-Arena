using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatButtonHoverEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject background;


    void Awake()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(background != null)
        {
            background.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }
}
