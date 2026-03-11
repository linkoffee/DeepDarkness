using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] audioClips;
}

public class SfxLib : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string soundName)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupID == soundName)
            {
                return soundEffect.audioClips[Random.Range(0, soundEffect.audioClips.Length)];
            }
        }

        return null;
    }
}
