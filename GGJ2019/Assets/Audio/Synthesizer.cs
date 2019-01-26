using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    //146.83, 164.81, 174.61,220.00,233.08,293.66, 329.63, 349.23, 440, 466.16

    private double sampling_frequency = 44100.0;
    private double freqMultiplier;
    public float amp;

    //Instrument 1
    public Transform player;
    public Transform target;
    double[] notes = { 110,146.83, 164.81, 174.61, 220.00, 233.08, 293.66, 329.63, 349.23, 440,  466.16 };
    double[] incs = new double[5];
    double[] phases = new double[5];


    public double FundFreq = 440.0;
    private double freq;

    //Oscillators
    public bool SinOsc = true;
    public float SinOscAmp = 0.3f;
    public bool TriOsc = false;
    public float TriOscAmp = 0.3f;
    public bool SawOsc = false;
    public float SawOscAmp = 0.3f;
    public bool SquareOsc = false;
    public float SquareOscAmp = 0.3f;
    public bool subSinOsc = false;
    public float subOscAmp = 0;

    //Sequencer
    private bool hasSeqChanged = false;
    public bool isSeq = false;
    public double seqFreq = 4;
    private double seqInc;
    private double seqPhase;
    private int currentNote = 0;

    //Freq LFO
    public bool isLFOFreq = false;
    public double LFOFreq = 4;
    public float LFOFreqRange = 0.4f;
    private double LFOInc;
    private double LFOPhase;

    //AMP LFO 
    public bool isLFOAmp = false;
    public double LFOAmpFreq = 0.5;
    public float LFOAmpRange = 0.4f;
    private double LFOAmpInc;
    private double LFOAmpPhase;

    //game object with message
    public double distThreshold=3.0;
    bool isCloseToTarget = false;



    private void Awake()
    {
        freqMultiplier = 2.0 * Mathf.PI / sampling_frequency;
    }

    private void Update()
    {
        double dist = Vector3.Distance(player.position, target.position);
       
        if (dist < distThreshold && !isCloseToTarget)
        {
            FundFreq = notes[Random.Range(0, 4)];
            LFOFreq = (distThreshold - dist) * 9;
            isCloseToTarget = true;
            isLFOFreq = true;
            LFOAmpFreq = 6;
        } else if (dist > distThreshold && isCloseToTarget) {

            FundFreq = 440;
            LFOAmpFreq = 0.5;
            isCloseToTarget = false;
            isLFOFreq = false;
        }
        //For theramin effect
        //frequency = fundFrequency * (1.0 + player.normalizedPosX);
        //amp = player.normalizedPosY;

    }


    private void OnAudioFilterRead(float[] data, int channels)
    {

        Instrument1Inc();
        for (int i = 0; i < data.Length; i+= channels)
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
    private void Instrument1Inc(){
        if (isLFOFreq)
        {
            freq = SinRanged(LFOPhase, (float)notes[currentNote] * (1.0f - LFOFreqRange), (float)notes[currentNote] * (1.0f + LFOFreqRange));
        } else {
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

    private void Instrument1UpdatePhases(){
        for (int i = 0; i < incs.Length; i++)
        {
            phases[i] += incs[i];
        }
        LFOPhase += LFOInc;
        LFOAmpPhase += LFOAmpInc;
        seqPhase += seqInc;
    }

    //Change note
    private void ChangeNote(){
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
        if (currentNote>notes.Length-1){
            currentNote = 0;
        }
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

        if (TriOsc){
            sig += (float)(0.5 * Mathf.PingPong((float)phases[0], 1.0f));

            sig *= TriOscAmp;
        }

        if(SawOsc){
            sig += (float)(0.5 * Mathf.Sin((float)phases[0])
                       + 0.25 * Mathf.Sin((float)phases[1]) +
                     0.125* Mathf.Sin((float)phases[2]) + 0.0625* Mathf.Sin((float)phases[3]));
            sig *= SawOscAmp;
        }

        if (SinOsc){
            sig += SinOscAmp * Mathf.Sin((float)phases[0]);

        }

        if (subSinOsc){
            sig += (float)(0.5 * Mathf.Sin((float)phases[4]) * subOscAmp);
        }

        if(isLFOAmp){
            sig *= SinRanged((float)LFOAmpPhase, 0.0f, 1.0f);
        }

        return sig;
    }

    private float SinRanged(double value, float min, float max){

        return (max+min + (max-min)*Mathf.Sin((float)value))/2.0f;
    }



}
