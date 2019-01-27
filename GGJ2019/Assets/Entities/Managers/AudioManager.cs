using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Synthesizer synthesizer;
    audioStates currentAudioState;

    public enum audioStates {
        walking,
        ghost,
        nearObject,
        discoveringObject
    }

    public void SetAudioState(audioStates audioState)
    {
        currentAudioState = audioState;

    }
}
