using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    #region DeclaredFields

    [SerializeField] private AudioSource asource;
    [SerializeField] private AudioClip [] gamesounds;

    #endregion

    #region SetAndPlayGameSounds

    public static GameSounds Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void PlaySounds(int sound_indx)
    {
        asource.clip = gamesounds[sound_indx];
        asource.Play();
    }

    #endregion
}
