using UnityEngine;

public class RandomSong : MonoBehaviour {

    public AudioClip[] songs;

    void Start() {
        GetComponent<AudioSource>().clip = songs[Random.Range(0, songs.Length)];
        GetComponent<AudioSource>().Play();
    }
}