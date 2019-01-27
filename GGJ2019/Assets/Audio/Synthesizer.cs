using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    //146.83, 164.81, 174.61,220.00,233.08,293.66, 329.63, 349.23, 440, 466.16

    private double sampling_frequency = 44100.0;
    private double freqMultiplier;
    public float amp = 0.3f;

    //Instrument 1
    //public Transform player;
    public EnemyManager target1;
    public EnemyManager target2;

    double[] notes = { 110, 146.83, 164.81, 174.61,    220.00, 233.08, 293.66, 329.63, 349.23, 440, 466.16 };

    double[] happyNotes = { 146.83, 164.81, 185.00, 196.00, 220.00, 246.94, 277.18, 293.66, 329.633, 369.99, 392.00 };
    double[] pentaNotes = { 146.83, 164.81, 185.00, 220.00, 246.94, 293.66, 329.633, 369.99, 220.00, 246.94, 293.66 };
    double[] nostalgicNotes = { 110, 146.83, 164.81, 174.61, 220.00, 233.08, 293.66, 329.63, 349.23, 440, 466.16 };
    double[] incs = new double[5];
    double[] phases = new double[5];


    public enum Preset {Doom,Happyish,Pacman, Dreamy, Famicom,Custom };
    public Preset preset;

    private double FundFreq = 440.0;
    private double freq;
    //Sequencer
    public enum Scale { Nostalgic, Happy, Pentatonic }
    public Scale myScale;
    private bool hasSeqChanged = false;
    public bool isSeq = false;
    [Range(0.0f, 32.0f)]
    public double seqFreq = 4;
    private double seqInc;
    private double seqPhase;
    private int currentNote = 0;

    [Range(1, 6)]
    public int octave = 3;
    //Oscillators
    public bool SinOsc = true;
    [Range(0.0f, 1.0f)]
    public float SinOscAmp = 0.3f;


    public bool TriOsc = false;
    [Range(0.0f, 1.0f)]
    public float TriOscAmp = 0.3f;
    public bool SawOsc = false;
    [Range(0.0f, 1.0f)]
    public float SawOscAmp = 0.3f;
    public bool SquareOsc = false;
    [Range(0.0f, 1.0f)]
    public float SquareOscAmp = 0.3f;
    public bool subSinOsc = false;
    [Range(0.0f, 1.0f)]
    public float subOscAmp = 0;





    //AMP LFO 
    public bool isLFOAmp = false;
    [Range(0.0f, 32.0f)]
    public double LFOAmpFreq = 0.5;
    [Range(0.0f, 1.0f)]
    public float LFOAmpRange = 0.4f;
    private double LFOAmpInc;
    private double LFOAmpPhase;
    public bool isLockFreqToSeq = false;

    //Freq LFO
    public bool isLFOFreq = false;
    [Range(0.0f, 32.0f)]
    public double LFOFreq = 4;
    [Range(0.0f, 10.0f)]
    public float LFOFreqRange = 0.4f;
    private double LFOInc;
    private double LFOPhase;

    //game object with message
    public double distThreshold = 3.0;
    bool isCloseToTarget = false;



    private void Awake()
    {
        amp = 0.3f;
        freqMultiplier = 2.0 * Mathf.PI / sampling_frequency;
    }

    private void Update()
    {

        //if (target1.isChasing || target2.isChasing){
        //    preset = Preset.Pacman;

        //} else {
        //    preset = Preset.Dreamy;
        //}


        if (isLockFreqToSeq)
        {
            LFOAmpFreq = seqFreq;
        }

        if (myScale==Scale.Nostalgic){
            for (int i = 0; i < notes.Length; i++) notes[i] = octave/3.0f*happyNotes[i];

        } else if (myScale == Scale.Happy){
            for (int i = 0; i < notes.Length; i++) notes[i] = octave / 3.0f * nostalgicNotes[i];
        }
        else if (myScale == Scale.Pentatonic)
        {
            for (int i = 0; i < notes.Length; i++) notes[i] = octave / 3.0f * pentaNotes[i];
        }
        setPreset();
    }

    public void ChoosePreset(AudioManager.audioStates audioState)
    {


        if (audioState.Equals(AudioManager.audioStates.nearObject))
        {
            preset = Preset.Famicom;
        }
        else if (audioState.Equals(AudioManager.audioStates.discoveringObject))
        {
            preset = Preset.Happyish;
        } else if (audioState.Equals(AudioManager.audioStates.ghost))
        {
            preset = Preset.Pacman;
        }
        else if (audioState.Equals(AudioManager.audioStates.walking))
            {
                preset = Preset.Dreamy;
            }
    }


        private void OnAudioFilterRead(float[] data, int channels)
    {

        Instrument1Inc();
        for (int i = 0; i < data.Length; i += channels)
        {
            Instrument1UpdatePhases();
            data[i] = amp * Instrument1Sig();
            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
        }
        if (isSeq)
        {
            ChangeNote();
        }
    }

    //Instrument 1
    private void Instrument1Inc()
    {


        if (isLFOFreq)
        {
            freq = SinRanged(LFOPhase, (float)notes[currentNote] * (1.0f - LFOFreqRange), (float)notes[currentNote] * (1.0f + LFOFreqRange));
        }
        else
        {
            freq = notes[currentNote];
        }
        incs[0] = freq * freqMultiplier;
        incs[1] = incs[0] * 2.0;
        incs[2] = incs[0] * 3.0;
        incs[3] = incs[0] * 4.0;
        //sub oscillator
        incs[4] = incs[0] * 0.5;
        LFOInc = LFOFreq * freqMultiplier;
        LFOAmpInc = LFOAmpFreq * freqMultiplier;
        seqInc = seqFreq * freqMultiplier;
    }

    private void Instrument1UpdatePhases()
    {
        for (int i = 0; i < incs.Length; i++)
        {
            phases[i] += incs[i];
        }
        LFOPhase += LFOInc;
        LFOAmpPhase += LFOAmpInc;
        seqPhase += seqInc;
    }

    //Change note
    private void ChangeNote()
    {
        float s = Mathf.Sin((float)seqPhase);
        float r = s * 0.5f + 0.5f;

        if (hasSeqChanged && s >= 0)
        {

            int t = (int)Mathf.Round((r * 1000 - Mathf.Floor(r * 1000)) * 10);
            currentNote = t;
            hasSeqChanged = !hasSeqChanged;
        }
        else if (!hasSeqChanged && s < 0)
        {
            hasSeqChanged = !hasSeqChanged;

        }
        if (currentNote > notes.Length - 1)
        {
            currentNote = 0;
        }
    }

    private float normalizeAmp()
    {
        float nAmp = 0.00001f;
        if (SinOsc) nAmp += SinOscAmp;
        if (TriOsc) nAmp += TriOscAmp;
        if (SquareOsc) nAmp += SquareOscAmp;
        if (SawOsc) nAmp += SawOscAmp;

        return nAmp;
    }

    //Addittive synth section
    private float Instrument1Sig()
    {
        float sig = 0f;
        if (SquareOsc)
        {
            if (Mathf.Sin((float)phases[0]) >= 0)
            {
                sig += 0.4f;
            }
            else
            {
                sig -= 0.4f;
            }

            sig *= SquareOscAmp;
        }

        if (TriOsc)
        {
            sig += (float)(0.5 * Mathf.PingPong((float)phases[0], 1.0f));

            sig *= TriOscAmp;
        }

        if (SawOsc)
        {
            sig += (float)(0.5 * Mathf.Sin((float)phases[0])
                       + 0.25 * Mathf.Sin((float)phases[1]) +
                     0.125 * Mathf.Sin((float)phases[2]) + 0.0625 * Mathf.Sin((float)phases[3]));
            sig *= SawOscAmp;
        }

        if (SinOsc)
        {
            sig += SinOscAmp * Mathf.Sin((float)phases[0]);

        }

        if (subSinOsc)
        {
            sig += (float)(subOscAmp * Mathf.Sin((float)phases[4]) * subOscAmp);
        }

        sig /= normalizeAmp();

        if (isLFOAmp)
        {
            sig *= SinRanged((float)LFOAmpPhase + Mathf.PI, 1.0f-LFOAmpRange, 1.0f);
        }

        return sig;
    }

    private float SinRanged(double value, float min, float max)
    {

        return (max + min + (max - min) * Mathf.Sin((float)value)) / 2.0f;
    }



    void setPreset()
    {
        if (preset.Equals(Preset.Doom)){
            myScale = Scale.Nostalgic;
            isSeq = true;
            seqFreq = 1;
            octave = 1;
            SinOsc = false;
            SinOscAmp = 0.3f;
            TriOsc = false;
            TriOscAmp = 0.3f;
            SawOsc = true;
            SawOscAmp = 0.54f;
            SquareOsc = true;
            SquareOscAmp = 0.15f;
            subSinOsc = true;
            subOscAmp = 0.27f;
            isLFOAmp = true;
            LFOAmpFreq = 4;
            LFOAmpRange = 0.46f;
            isLockFreqToSeq = false;
            isLFOFreq = false;
            LFOFreq = 4;
            LFOFreqRange = 0.4f;
        } else if (preset.Equals(Preset.Happyish))
        {
            myScale = Scale.Pentatonic;
            isSeq = true;
            seqFreq = 6;
            octave = 3;
            SinOsc = true;
            SinOscAmp = 0.3f;
            TriOsc = true;
            TriOscAmp = 0.53f;
            SawOsc = false;
            SawOscAmp = 0.54f;
            SquareOsc = false;
            SquareOscAmp = 0.15f;
            subSinOsc = true;
            subOscAmp = 0.27f;
            isLFOAmp = true;
            LFOAmpFreq = 6;
            LFOAmpRange = 0.72f;
            isLockFreqToSeq = true;
            isLFOFreq = false;
            LFOFreq = 4;
            LFOFreqRange = 0.4f;
        }
        else if (preset.Equals(Preset.Pacman))
        {
            myScale = Scale.Pentatonic;
            isSeq = false;
            seqFreq = 6;
            octave = 3;
            SinOsc = true;
            SinOscAmp = 0.13f;
            TriOsc = true;
            TriOscAmp = 0.8f;
            SawOsc = false;
            SawOscAmp = 0.54f;
            SquareOsc = false;
            SquareOscAmp = 0.15f;
            subSinOsc = true;
            subOscAmp = 0.65f;
            isLFOAmp = false;
            LFOAmpFreq = 6;
            LFOAmpRange = 0.72f;
            isLockFreqToSeq = true;
            isLFOFreq = true;
            LFOFreq = 5;
            LFOFreqRange = 0.3f;
        }
        else if (preset.Equals(Preset.Dreamy))
        {
            myScale = Scale.Happy;
            isSeq = true;
            seqFreq = 4;
            octave = 2;
            SinOsc = true;
            SinOscAmp = 0.5f;
            TriOsc = true;
            TriOscAmp = 0.18f;
            SawOsc = true;
            SawOscAmp = 0.68f;
            SquareOsc = false;
            SquareOscAmp = 0.15f;
            subSinOsc = false;
            subOscAmp = 0.65f;
            isLFOAmp = true;
            LFOAmpFreq = 1;
            LFOAmpRange = 0.78f;
            isLockFreqToSeq = false;
            isLFOFreq = true;
            LFOFreq = 0.1;
            LFOFreqRange = 0.1f;
        }
        else if (preset.Equals(Preset.Famicom))
        {
            myScale = Scale.Pentatonic;
            isSeq = true;
            seqFreq = 7.4;
            octave = 4;
            SinOsc = true;
            SinOscAmp = 0.53f;
            TriOsc = false;
            TriOscAmp = 0.18f;
            SawOsc = false;
            SawOscAmp = 0.68f;
            SquareOsc = true;
            SquareOscAmp = 0.15f;
            subSinOsc = false;
            subOscAmp = 0.65f;
            isLFOAmp = true;
            LFOAmpFreq = 3.7;
            LFOAmpRange = 0.56f;
            isLockFreqToSeq = false;
            isLFOFreq = false;
            LFOFreq = 0.1;
            LFOFreqRange = 0.1f;
        }
    }



}
