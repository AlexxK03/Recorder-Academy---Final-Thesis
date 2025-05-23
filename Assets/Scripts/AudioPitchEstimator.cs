﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fundamental frequency estimation using Summation of Residual Harmonics (SRH)
// T. Drugman and A. Alwan: "Joint Robust Voicing Detection and Pitch Estimation Based on Residual Harmonics", Interspeech'11, 2011.

public class AudioPitchEstimator : MonoBehaviour
{
    [Tooltip("Lowest frequency that can estimate [Hz]")]
    [Range(40, 150)]
    public int frequencyMin = 40;

    [Tooltip("Highest frequency that can estimate [Hz]")]
    [Range(300, 1200)]
    public int frequencyMax = 600;

    [Tooltip("Number of overtones to use for estimation")]
    [Range(1, 8)]
    public int harmonicsToUse = 5;

    [Tooltip("Frequency bandwidth of spectral smoothing filter [Hz]\nWider bandwidth smoothes the estimation, however the accuracy decreases.")]
    public float smoothingWidth = 500;

    [Tooltip("Threshold to judge silence or not\nLarger the value, stricter the judgment.")]
    public float thresholdSRH = 7;

    const int spectrumSize = 1024;
    const int outputResolution = 200; // frequency axis resolution (decreasing this will reduce the calculation load)
    float[] spectrum = new float[spectrumSize];
    float[] specRaw = new float[spectrumSize];
    float[] specCum = new float[spectrumSize];
    float[] specRes = new float[spectrumSize];
    float[] srh = new float[outputResolution];

    public List<float> SRH => new List<float>(srh);

    /// <summary>
    /// Estimates the fundamental frequency
    /// </summary>
    /// <param name="audioSource">Input audio source</param>
    /// <returns>Fundamental frequency [Hz] (float.NaN if it does not exist)</returns>
    public float Estimate(AudioSource audioSource)
    {
        var nyquistFreq = AudioSettings.outputSampleRate / 2.0f;

        // オーディオスペクトルを取得 Get audio spectrum
        if (!audioSource.isPlaying) return float.NaN;
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        // 振幅スペクトルの対数を計算
        // 以降のスペクトルはすべて対数振幅で扱う（ここは元論文と異なる）
        //Calculate the logarithm of the amplitude spectrum
        //Spectra are treated as logarithmic amplitudes 
        for (int i = 0; i < spectrumSize; i++)
        {
            // 振幅ゼロのとき-∞になってしまうので小さな値を足しておく
            //When the amplitude is zero, it becomes -∞, so add a small value.
            specRaw[i] = Mathf.Log(spectrum[i] + 1e-9f);
        }

        // スペクトルの累積和（あとで使う
        //Cumulative sum of spectra (to be used later)
        specCum[0] = 0;
        for (int i = 1; i < spectrumSize; i++)
        {
            specCum[i] = specCum[i - 1] + specRaw[i];
        }

        // 残差スペクトルを計算
        //Calculate residual spectrum
        var halfRange = Mathf.RoundToInt((smoothingWidth / 2) / nyquistFreq * spectrumSize);
        for (int i = 0; i < spectrumSize; i++)
        {
            // スペクトルを滑らかに（累積和を使って移動平均）
            //Smooth the spectrum (moving average using cumulative sum)
            var indexUpper = Mathf.Min(i + halfRange, spectrumSize - 1);
            var indexLower = Mathf.Max(i - halfRange + 1, 0);
            var upper = specCum[indexUpper];
            var lower = specCum[indexLower];
            var smoothed = (upper - lower) / (indexUpper - indexLower);

            // Remove smooth components from original spectrum
            specRes[i] = specRaw[i] - smoothed;
        }

        // SRH (Summation of Residual Harmonics) calculate the score of のスコアを計算
        float bestFreq = 0, bestSRH = 0;
        for (int i = 0; i < outputResolution; i++)
        //Debug.Log("The best Frequency" + bestFreq);
        {
            var currentFreq = (float)i / (outputResolution - 1) * (frequencyMax - frequencyMin) + frequencyMin;

            // 現在の周波数におけるSRHのスコアを計算: 論文の式(1)
            //Calculate the SRH score at the current frequency: paper formula (1)
            var currentSRH = GetSpectrumAmplitude(specRes, currentFreq, nyquistFreq);
            for (int h = 2; h <= harmonicsToUse; h++)
            {
                // h倍の周波数では、信号が強いほど良い
                //At h times the frequency, the stronger the signal the better
                currentSRH += GetSpectrumAmplitude(specRes, currentFreq * h, nyquistFreq);

                // h-1倍 と h倍 の中間の周波数では、信号が強いほど悪い
                //At frequencies between h-1 and h times, the stronger the signal, the worse it is.
                currentSRH -= GetSpectrumAmplitude(specRes, currentFreq * (h - 0.5f), nyquistFreq);
            }
            srh[i] = currentSRH;

            // スコアが最も大きい周波数を記録
            //Record the frequency with the highest score
            if (currentSRH > bestSRH)
            {
                bestFreq = currentFreq;
                bestSRH = currentSRH;
            }
        }

        // SRHのスコアが閾値に満たない → 明確な基本周波数が存在しないとみなす
        //SRH score is below the threshold → It is assumed that there is no clear fundamental frequency !!!
        if (bestSRH < thresholdSRH) return float.NaN;

        return bestFreq;
    }

    // スペクトルデータからfrequency[Hz]における振幅を取得する
    //Obtain the amplitude at frequency [Hz] from spectrum data
    float GetSpectrumAmplitude(float[] spec, float frequency, float nyquistFreq)
    {
        var position = frequency / nyquistFreq * spec.Length;
        var index0 = (int)position;
        var index1 = index0 + 1; // 配列の境界チェックは省略 Skip array bounds checking
        var delta = position - index0;
        return (1 - delta) * spec[index0] + delta * spec[index1];
    }

    public void TestMethod()
    {
        Debug.Log("PitchEstimator has access to audioPitchEstimator");
    }

}
