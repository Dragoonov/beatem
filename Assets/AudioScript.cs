using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioClip hitClip;
    public AudioSource hitSource;

    public AudioClip slowClip;
    public AudioClip mediumClip;
    public AudioClip fastClip;
    public AudioSource speedSource;


    public const float defaultHitTimerSeconds = 0.4f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;
    // Start is called before the first frame update
    void Start()
    {
        hitSource.clip = hitClip;
        speedSource.clip = slowClip;
        speedSource.loop = true;
        hitTimerSeconds = defaultHitTimerSeconds;
        hitTimerRunning = false;
        speedSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ResolveHitSound();
        ResolveLevelSound();
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

    public void ResolveLevelSound()
    {
        float speedLevel = GetComponent<LevelSpeed>().levelSpeed;
        if (speedLevel < 0.08 && speedSource.clip != slowClip)
        {
            speedSource.Stop();
            speedSource.clip = slowClip;
            speedSource.Play();
        }
        else if (speedLevel >= 0.08 && speedLevel < 0.11 && speedSource.clip != mediumClip)
        {
            speedSource.Stop();
            speedSource.clip = mediumClip;
            speedSource.Play();
        }
        else if (speedLevel >= 0.11 && speedSource.clip != fastClip)
        {
            speedSource.Stop();
            speedSource.clip = fastClip;
            speedSource.Play();
        }
    }
}
