using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Final Boss Pattern Icon", menuName = "Final Boss Pattern Icon")]
public class FinalBossPatternIconSO : ScriptableObject
{
    [Header("Elements")]
    public FinalBossPattern pattern;
    public Sprite icon;

}
