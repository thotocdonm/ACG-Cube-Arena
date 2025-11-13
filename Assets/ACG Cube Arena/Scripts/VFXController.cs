using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private GameObject slashVFX;

    [Header("Anchors")]
    [SerializeField] private GameObject VFX1Anchor;
    [SerializeField] private GameObject VFX2Anchor;
    [SerializeField] private GameObject VFX3Anchor;


    public void SpawnSlashVfx(int index)
    {
        GameObject anchorToSpawn = null;
        switch (index)
        {
            case 1:
                anchorToSpawn = VFX1Anchor;
                break;
            case 2:
                anchorToSpawn = VFX2Anchor;
                break;
            case 3:
                anchorToSpawn = VFX3Anchor;
                break;
        }

        if(anchorToSpawn != null)
        {
            if(index == 3)
            {
                GameObject vfxInstance1 = Instantiate(slashVFX, anchorToSpawn.transform);
                GameObject vfxInstance2 = Instantiate(slashVFX, anchorToSpawn.transform);
                vfxInstance2.transform.localRotation = Quaternion.Euler(0, 180, 0);

                Destroy(vfxInstance1, 1f);
                Destroy(vfxInstance2, 1f);
            }
            else
            {
                GameObject vfxInstance = Instantiate(slashVFX, anchorToSpawn.transform);
                Destroy(vfxInstance, 1f);
            }
        }
    }
    
}
