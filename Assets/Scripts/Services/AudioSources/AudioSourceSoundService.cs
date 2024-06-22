using UnityEngine;

public class AudioSourceSoundService : MonoBehaviour
{
    // public variables
    public AudioClip openWindowClip;
    public AudioClip closeWindowClip;
    public AudioClip equipItemClip;
    public AudioClip quitItemClip;
    public AudioClip sellBuyClip;
    public AudioClip errorItemClip;
    public AudioClip tickClip;

    // private variables
    AudioSource audioSource;

	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
	}

    public void PlayOpenWindow()
	{
        PlayClip(openWindowClip);
    }

    public void PlayCloseWindow()
    {
        PlayClip(closeWindowClip);
    }

    public void PlayEquipItem()
    {
        PlayClip(equipItemClip);
    }

    public void PlayQuitItem()
    {
        PlayClip(quitItemClip);
    }

    public void PlaySellBuy()
    {
        PlayClip(sellBuyClip);
    }

    public void PlayError()
    {
        PlayClip(errorItemClip);
    }

    public void PlayTick()
    {
        PlayClip(tickClip);
    }

    private void PlayClip(AudioClip audioClip)
	{
        audioSource.clip = audioClip;
        audioSource.Play();
	}
}