using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioClip hitClip;

    public AudioSource hitSource;
    public const float defaultHitTimerSeconds = 0.4f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;
    // Start is called before the first frame update
    void Start()
    {
        hitSource.clip = hitClip;
        hitTimerSeconds = defaultHitTimerSeconds;
        hitTimerRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        ResolveHitSound();
    }

    public void PlayHitSound()
    {
        hitSource.Play();
        hitTimerRunning = true;
    }

    public void ResolveHitSound()
    {
        if (hitTimerRunning)
        {
            hitTimerSeconds -= Time.smoothDeltaTime;
            if (hitTimerSeconds < 0)
            {
                hitSource.Stop();
                hitTimerRunning = false;
                hitTimerSeconds = defaultHitTimerSeconds;
            }
        }
    }
}
