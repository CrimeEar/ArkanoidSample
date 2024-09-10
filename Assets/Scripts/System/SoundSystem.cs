using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource[] audioSourceShort;

    private int currentSourceIndex;

    public static SoundSystem Instance;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void PlaySound(AudioName typeName, float pitch)
    {
        if (audioSourceShort[currentSourceIndex].isPlaying)
        {
            return;
        }

        audioSourceShort[currentSourceIndex].clip = audioClips[(int)typeName];
        audioSourceShort[currentSourceIndex].pitch = pitch;
        audioSourceShort[currentSourceIndex].Play();

        currentSourceIndex++;
        currentSourceIndex %= audioSourceShort.Length;
    }
}

public enum AudioName { Pop, Show, StartMenu}
