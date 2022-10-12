using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

/// <summary>
/// Manage all the SFX behaviour
/// </summary>
///
public class SfxManager : Manager<SfxManager>, IEventHandler
{
    [SerializeField] private AudioSource[] m_SfxAudioSources;
    private List<AudioSource> m_SaveAudioSources;

    #region Event Listeners Methods
    /// <summary>
    /// OnPlaySFXEvent we called <see cref="StartSFX"/> to start the SFX
    /// </summary>
    /// <param name="e">the event</param>
    private void OnPlaySFXEvent(PlaySFXEvent e)
    {
        this.StartSFX(e.eAudioSource, e.eAudioClip);
    }

    /// <summary>
    ///  OnStopSFXEvent we called <see cref="StopSFX"/> to stop the SFX
    /// </summary>
    /// <param name="e"></param>
    private void OnStopSFXEvent(StopSFXEvent e)
    {
        this.StopSFX(e.eAudioSource);
    }
    #endregion

    #region SFX Manager methods
    /// <summary>
    /// OnAwakeSFXManager we init the manager, create an list of audio sources, suscribe to events
    /// </summary>
    private void OnAwakeSFXManager()
    {
        base.InitManager();
        this.m_SaveAudioSources = new List<AudioSource>();
        this.SubscribeEvents();

        if (this.m_SfxAudioSources.Length < 2)
        {
            Tools.LogWarning(this, "You should be set 2 audio sources");
        }
    }


    /**
     * <summary>Start a SFX sound with audiosource or audioclip or both</summary> 
     * <param name="audioSource">The audio source</param>
     * <param name="audioClip">The audio clip</param>
     */
    public void StartSFX(AudioSource audioSource, AudioClip audioClip)
    {
        if (!audioClip)
        {
            this.StartSFX(audioSource);
        }
        else if (!audioSource)
        {
            this.StartSFX(audioClip);
        }
        else
        {
            this.StopSFX(audioSource);
            audioSource.clip = audioClip;
            this.StartSFX(audioSource);
        }
    }

    /**
     * <summary>Start an sfx with audio clip</summary>
     * <remarks>The audioclip will be played on the SFXManger audio source</remarks>
     * <param name="audioClip">The audio clip to play</param>
     */
    public void StartSFX(AudioClip audioClip)
    {
        if (audioClip && !this.IsAlreadyPlayingByAudioSource(audioClip))
        {
            for (int i = 0; i < this.m_SfxAudioSources.Length; i++)
            {
                AudioSource audioSource = this.m_SfxAudioSources[i];
                if (!Object.Equals(audioSource.clip, audioClip) && !audioSource.isPlaying)
                {
                    audioSource.clip = audioClip;
                    this.StartSFX(audioSource);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Verify if the audioClip IsAlreadyPlayingByAudioSource
    /// </summary>
    /// <param name="audioClip">The audio ckip</param>
    /// <returns>if the audioClip IsAlreadyPlayingByAudioSource</returns>
    public bool IsAlreadyPlayingByAudioSource(AudioClip audioClip)
    {
        for (int i = 0; i < this.m_SfxAudioSources.Length; i++)
        {
            AudioSource audioSource = this.m_SfxAudioSources[i];
            if (Object.Equals(audioSource.clip, audioClip) && audioSource.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * <summary>Start a SFX sound with audiosource</summary> 
     * <param name="audioSource">The audio source</param>
     */
    public void StartSFX(AudioSource audioSource)
    {
        if (audioSource) {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                this.m_SaveAudioSources.Add(audioSource);
            }
            else
            {
                this.StopSFX(audioSource);
            }
        }
        
    }

    /**
     * <summary>Stop SFX with audio source</summary> 
     * <param name="audioSource">The audio source will stop sound</param>
     */
    public void StopSFX(AudioSource audioSource)
    {
        if (audioSource) { 
            audioSource.Stop();
            audioSource.clip = null;
            this.m_SaveAudioSources.Remove(audioSource);
        }
    }

    /**
     * <summary>Stop all save SFX</summary> 
     */
    private void StopAllSaveSFX()
    {
        for (int i = 0; i < this.m_SaveAudioSources.Count; i++)
        {
            this.StopSFX(this.m_SaveAudioSources[i]);
        }
    }

    /**
     * <summary>Remove all audiosource in list which not playing</summary>
     */
    private void RemoveAllUselessSFX()
    {
        this.m_SaveAudioSources.RemoveAll((AudioSource currentAudioSource) => { 
            if (!currentAudioSource.isPlaying) currentAudioSource.clip = null; 
            return !currentAudioSource.isPlaying; 
        });
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<PlaySFXEvent>(OnPlaySFXEvent);
        EventManager.Instance.AddListener<StopSFXEvent>(OnStopSFXEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<PlaySFXEvent>(OnPlaySFXEvent);
        EventManager.Instance.RemoveListener<StopSFXEvent>(OnStopSFXEvent);
    }
    #endregion

    #region MonoBehaviour METHODS

    private void Awake()
    {
        this.OnAwakeSFXManager();
    }

    private void FixedUpdate()
    {
       this.RemoveAllUselessSFX();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
        this.StopAllSaveSFX();
    }

    #endregion
}
