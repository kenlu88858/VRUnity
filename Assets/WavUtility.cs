using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static void SaveWav(string filePath, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("SaveWav: clip is null");
            return;
        }

        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        byte[] wavData = ConvertToWav(samples, clip.channels, clip.frequency);
        File.WriteAllBytes(filePath, wavData);
    }

    private static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        int sampleCount = samples.Length;
        int byteCount = sampleCount * sizeof(short);

        // ----- WAV Header -----
        writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
        writer.Write(36 + byteCount);
        writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
        writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
        writer.Write(16); // PCM
        writer.Write((short)1); // PCM
        writer.Write((short)channels);
        writer.Write(sampleRate);
        writer.Write(sampleRate * channels * sizeof(short)); // byte rate
        writer.Write((short)(channels * sizeof(short))); // block align
        writer.Write((short)16); // bits per sample
        writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
        writer.Write(byteCount);

        // ----- WAV Data -----
        foreach (var sample in samples)
        {
            short intSample = (short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue);
            writer.Write(intSample);
        }

        writer.Flush();
        return stream.ToArray();
    }
}

