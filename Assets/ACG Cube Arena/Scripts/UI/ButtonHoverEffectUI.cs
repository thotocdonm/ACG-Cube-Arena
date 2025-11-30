using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject button;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.3f;


    void Awake()
    {
        if(border != null)
        {
            border.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(border != null)
        {
            border.SetActive(true);
        }
        if(button != null)
        {
            button.transform.DOScale(1.1f, scaleDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }
        AudioManager.instance.PlayButtonHoverSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (border != null)
        {
            border.SetActive(false);
        }
        if (button != null)
        {
            button.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }

    }

    void OnDisable()
    {
        if (border != null)
        {
            border.SetActive(false);
        }
        if (button != null)
        {
            button.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }
    
}
