using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Ambient")]
    public AudioSource AmbientSource;
    public AudioClip[] OneshotShorts;

    [Header("SFX")]
    public AudioSource SFX_Source;
    [Space]
    public AudioClip ItemPickup;
    public AudioClip ItemDrop;
    [Space]
    public AudioClip CorrectSort;
    public AudioClip IncorrectSort;
    [Space]
    public AudioClip NewItem;
    public AudioClip Next;
    public AudioClip VentTumble;
    [Space]
    public AudioClip Confetti;
    public AudioClip yay;
    public AudioClip partyHorn;

    [Header("Settings")]
    public float minWait;
    public float maxWait;

    private float timer;
    private float thisWait;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= thisWait)
        {
            PlayAmbientSound(NewRandom());
            NewWait();
            timer = 0;
        }
    }

    private int NewRandom()
    {
        int newRandom;
        newRandom = Random.Range(0, OneshotShorts.Length);

        return newRandom;
    }

    private void NewWait()
    {
        float newWait;
        newWait = Random.Range(minWait, maxWait);

        thisWait = newWait;
    }

    private void PlayAmbientSound(int i)
    {
        AmbientSource.PlayOneShot(OneshotShorts[i]);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX_Source.PlayOneShot(clip);
    }
}
