using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Music")]
    public AudioSource[] musicList;

    GameObject[] allAudio;

    private void Awake()
    {
        instance = this;
    }

    #region Music
    public void PlayMusicProgress()
    {
        musicList[0].pitch -= 0.05f;
    }

    public void PlayGameOver()
    {
        musicList[0].Stop();
        allAudio = GameObject.FindGameObjectsWithTag("Helicopter");

        foreach (GameObject source in allAudio)
        {
            source.GetComponent<AudioSource>().Stop();
        }
        musicList[2].Play();
    }
    #endregion

    #region SFX
    // Jouer depuis les scripts des habitants/policiers/joueur avec eux des listes d'AudioSource qu'ils ont.
    // C'est pout tout ce qui est des toux, grognements, blabla et tout.
    public void PlayVoices(AudioSource[] voicesList)
    {
        int nbVoices = Random.Range(0, voicesList.Length);
        voicesList[nbVoices].Play();
    }

    public void PlayMask(AudioSource mask)
    {
        mask.Play();
    }

    public void PlayAlarm(AudioSource alarm)
    {
        alarm.pitch = Random.Range(0.5f, 1);
        alarm.Play();
    }
    #endregion
}
