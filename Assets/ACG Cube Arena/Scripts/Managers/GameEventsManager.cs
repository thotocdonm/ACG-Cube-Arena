using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventsManager
{
    public static event Action<SkillId, float> onSkillCooldownStart;

    public static event Action onPlayerAttackAttempted;
    public static event Action onPlayerSkillAttempted;

    public static event Action<FinalBossPatternIconSO, float> onFinalBossPatternStarted;

    public static void TriggerSkillCooldownStart(SkillId skillId, float cooldown)
    {
        onSkillCooldownStart?.Invoke(skillId, cooldown);
    }

    public static void TriggerPlayerAttackAttempted()
    {
        onPlayerAttackAttempted?.Invoke();
    }

    public static void TriggerPlayerSkillAttempted()
    {
        onPlayerSkillAttempted?.Invoke();
    }

    public static void TriggerFinalBossPatternStarted(FinalBossPatternIconSO iconSO, float duration)
    {
        onFinalBossPatternStarted?.Invoke(iconSO, duration);
    }
}
