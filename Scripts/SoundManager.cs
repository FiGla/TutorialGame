using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;


	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}
	
    public void PlaySingle (AudioClip audio){
        efxSource.pitch = lowPitchRange;
        efxSource.clip = audio;
        efxSource.Play();
    }

    public void RadomizeEfx(params AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        float randomRange = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomRange;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
