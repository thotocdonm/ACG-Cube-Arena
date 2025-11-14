using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AudioSource[] ichigoAudioSources;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayIchigoAttackSound(int index)
    {
        AudioSource audioSource = ichigoAudioSources[index];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
