using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds a key value pair that allows a gameobject to have many audio clips. Use the key given in the inspector to play the sound
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

    /// <summary>
    /// Plays an audioclip from a key given
    /// </summary>
    /// <param name="key"></param>
    /// <returns>bool: false if key doesn't exist, true if there are no errors</returns>
    public bool PlaySound(string key)
    {
        if (!audioKeys.TryGetValue(key, out AudioClip clip))
        {
            Debug.LogError($"Key \"{key}\" does not exist in audio dictionary for {gameObject.name}");
            return false;
        }

        audioSource.PlayOneShot(clip);
        return true;
    }
}
