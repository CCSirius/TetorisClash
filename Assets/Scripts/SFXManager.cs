using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Line Clear SFX")]
    public AudioClip[] lineClearClips; // 랜덤 재생

    [Header("Combo SFX")]
    public AudioClip combo4Clip;
    public AudioClip combo5Clip;
    public AudioClip combo6Clip;
    public AudioClip combo7PlusClip;

    [Header("Other SFX")]
    public AudioClip rotateClip;
    public AudioClip dropClip;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLineClear()
    {
        if (lineClearClips.Length > 0)
        {
            int index = Random.Range(0, lineClearClips.Length);
            audioSource.PlayOneShot(lineClearClips[index]);
        }
        else
        {
            Debug.LogWarning("No line clear clips assigned!");
        }
    }

    public void PlayCombo(int combo)
    {
        if (combo == 4 && combo4Clip != null)
            audioSource.PlayOneShot(combo4Clip);
        else if (combo == 5 && combo5Clip != null)
            audioSource.PlayOneShot(combo5Clip);
        else if (combo == 6 && combo6Clip != null)
            audioSource.PlayOneShot(combo6Clip);
        else if (combo >= 7 && combo7PlusClip != null)
            audioSource.PlayOneShot(combo7PlusClip);
    }

    public void PlayRotate()
    {
        audioSource.PlayOneShot(rotateClip);
    }

    public void PlayDrop()
    {
        audioSource.PlayOneShot(dropClip);
    }
}