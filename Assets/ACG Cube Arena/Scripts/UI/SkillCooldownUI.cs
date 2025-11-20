using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Image cooldownFillImage;
    private Coroutine cooldownCoroutine;

    [Header("Configuration")]
    [SerializeField] private SkillId skillId;

    void Awake()
    {
        GameEventsManager.onSkillCooldownStart += OnSkillCooldownStartCallback;
    }
    
    void OnDestroy()
    {
        GameEventsManager.onSkillCooldownStart -= OnSkillCooldownStartCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        cooldownFillImage.fillAmount = 0;
        cooldownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSkillUsedCallback(float cooldown)
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        cooldownCoroutine = StartCoroutine(CooldownCoroutine(cooldown));
    }

    private void OnSkillCooldownStartCallback(SkillId skillId, float cooldown)
    {
        if (skillId == this.skillId)
        {
            OnSkillUsedCallback(cooldown);
        }
    }
    IEnumerator CooldownCoroutine(float duration)
    {
        cooldownText.gameObject.SetActive(true);
        float remainingTime = duration;
        cooldownFillImage.fillAmount = 1;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            cooldownFillImage.fillAmount = remainingTime / duration;
            cooldownText.text = Mathf.RoundToInt(remainingTime).ToString();
            yield return null;
        }
        cooldownFillImage.fillAmount = 0;
        cooldownText.gameObject.SetActive(false);
        cooldownCoroutine = null;
    }
}
