using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDictionary : MonoBehaviour
{
    [SerializeField]
    List<AudioKey> inspectorKeys;

    AudioSource audioSource;
    Dictionary<string, AudioClip> audioKeys;

    [Serializable]
    private struct AudioKey
    {
        public AudioClip clip;
        public string key;
    }
  

    private void Awake()
    {
        audioKeys = new Dictionary<string, AudioClip>();
        audioSource = GetComponent<AudioSource>();

        foreach(AudioKey key in inspectorKeys)
        {
            audioKeys.Add(key.key, key.clip);
        }
    }

    public bool PlaySound(string key)
    {
        if (!audioKeys.TryGetValue(key, out AudioClip clip))
        {
            Debug.LogError("Key does not exist in audio dictionary");
            return false;
        }

        audioSource.PlayOneShot(clip);
        return true;
    }
}
