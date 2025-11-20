using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventsManager
{
    public static event Action<SkillId, float> onSkillCooldownStart;

    public static void TriggerSkillCooldownStart(SkillId skillId, float cooldown)
    {
        onSkillCooldownStart?.Invoke(skillId, cooldown);
    }
}
