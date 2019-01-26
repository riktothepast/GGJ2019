using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    private double sampling_frequency = 44100.0;
    private double freqMultiplier;
    public float amp;

    //Instrument 1
    double[] Inst1Incs = new double[5];
    double[] Inst1Phases = new double[5];
    public double Inst1FunFreq1 = 440.0;
    private double Inst1Freq;
    public double Inst1LFOFreq = 4;
    private double Inst1LFOInc;
    private double Inst1LFOPhase;

    //game object with message
    public Player player;
    public double distThreshold=0.1;
    private void Awake()
    {
        freqMultiplier = 2.0 * Mathf.PI / sampling_frequency;
    }

    private void Update()
    {

        //double dist = player.normalizedDistance;
        ////Debug.Log(dist);
        //if (dist < distThreshold)
        //{
        //    frequency2 = (1.0-dist) * 40 + frequency;
        //} else {
        //    frequency2 = (1.0 - dist)*10+ frequency;
        //}

        //For theramin effect
        //frequency = fundFrequency * (1.0 + player.normalizedPosX);
        //amp = player.normalizedPosY;

    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
    
        Instrument1Inc(Inst1Freq);
        for (int i = 0; i < data.Length; i+= channels)
        {
            Instrument1UpdatePhases();
            data[i] = amp * Instrument1Sig();
            if (channels == 2)
            {
                data[i + 1] = data[i];
            }


        }
    }

    //5 osc
    private void Instrument1Inc(double freq){
        Inst1Freq = SinRanged(Inst1LFOPhase, (float)Inst1FunFreq1 * 0.9f,(float) Inst1FunFreq1 * 1.1f);
        Inst1Incs[0] = Inst1Freq * freqMultiplier;
        Inst1Incs[1] = Inst1Incs[0] * 1.2;
        Inst1Incs[2] = Inst1Incs[0] * 1.5;
        Inst1Incs[3] = Inst1Incs[0] * 7.0/4.0;
        Inst1Incs[4] = Inst1Incs[0] * 0.5;
        Inst1LFOInc = Inst1LFOFreq * freqMultiplier;
    }

    private void Instrument1UpdatePhases(){
        for (int i = 0; i < Inst1Incs.Length; i++)
        {
            Inst1Phases[i] += Inst1Incs[i];
        }
        Inst1LFOPhase += Inst1LFOInc;
    }

    private float Instrument1Sig(){
        return (float)(0.5*Mathf.Sin((float)Inst1Phases[0])
                       + 0.25 * Mathf.Sin((float)Inst1Phases[1]) +
                     0.15* Mathf.Sin((float)Inst1Phases[2]) + 0.07* Mathf.Sin((float)Inst1Phases[3]) +
                      0.03* Mathf.Sin((float)Inst1Phases[1]));
    }

    private float SinRanged(double value, float min, float max){

        return (max+min + (max-min)*Mathf.Sin((float)value))/2.0f;
    }



}
