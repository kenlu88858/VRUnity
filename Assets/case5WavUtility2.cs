//case5WavUtility2
using System;
using System.IO;
using UnityEngine;

public static class WavUtility2_5
{
    public static byte[] FromAudioClip(AudioClip clip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            Convert(clip, stream);
            return stream.ToArray();
        }
    }

    public static void Convert(AudioClip clip, Stream stream)
    {
        // Format header
        int sampleCount = clip.samples * clip.channels;
        int dataSize = sampleCount * 2;
        int byteRate = clip.frequency * clip.channels * 2;
        int blockAlign = clip.channels * 2;

        // RIFF header
        WriteString(stream, "RIFF");
        WriteInt(stream, 36 + dataSize);  // ChunkSize
        WriteString(stream, "WAVE");

        // Format chunk
        WriteString(stream, "fmt ");
        WriteInt(stream, 16);  // SubChunk1Size
        WriteShort(stream, 1);  // AudioFormat (PCM)
        WriteShort(stream, (short)clip.channels);
        WriteInt(stream, clip.frequency);
        WriteInt(stream, byteRate);
        WriteShort(stream, (short)blockAlign);
        WriteShort(stream, 16);  // Bits per sample

        // Data chunk
        WriteString(stream, "data");
        WriteInt(stream, dataSize);  // SubChunk2Size

        // Audio samples
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        foreach (float sample in samples)
        {
            short sampleInt = (short)(sample * short.MaxValue);
            WriteShort(stream, sampleInt);
        }
    }

    private static void WriteString(Stream stream, string value)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private static void WriteInt(Stream stream, int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private static void WriteShort(Stream stream, short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }
}
