using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Animate(int damage, bool isCritical, bool isPlayer = false)
    {
        damageText.text = isCritical ? damage.ToString() + " !" : damage.ToString();
        damageText.faceColor = isCritical ? Color.yellow : Color.white;
        if (isPlayer)
        {
            damageText.faceColor = Color.red;
        }
        damageText.fontSizeMax = isCritical ? 12 : 10;
        damageText.transform.rotation = Quaternion.Euler(70, 0, 0);

        animator.Play("DmgText");
    }
}
