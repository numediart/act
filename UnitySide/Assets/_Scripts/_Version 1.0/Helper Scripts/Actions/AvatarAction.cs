using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[Serializable]
public class AvatarAction
{
    // Filled by the config file
    public int Intensity;
    public string CsvFilePath; //.csv
    public string AudioFilePath; //.mp4

    // Filled by the functions
    public string Csv;
    public AudioClip Audio;
    public bool ContainsAudio;
    public double Duration;
    public double PoseDuration;
    public FrameList ActionFrameList = new FrameList();
    public FrameList PoseFrameList = new FrameList();
    public FrameManager FrameManager = new FrameManager();

    public void Init()
    {
        // Find the audio and reads it
        if (!string.IsNullOrEmpty(AudioFilePath))
        {
            Audio = GetAudioClipFromFile(AudioFilePath);
            if (Audio != null)
                ContainsAudio = true;
        }

        bool isCsv = false;
        //Find the csv and reads it
        if (!string.IsNullOrEmpty(CsvFilePath))
            // check extension to know if it's a json or a csv
            if (Path.GetExtension(CsvFilePath) == ".json")
            {
                Csv = GetJsonFromFile(CsvFilePath);
                isCsv = true;
            }
            else if (Path.GetExtension(CsvFilePath) == ".csv")
            {
                Csv = GetCsvFromFile(CsvFilePath);
                isCsv = true;
            }

        // If success
        if (!string.IsNullOrEmpty(Csv) && !isCsv)
        {
            FrameList[] frameLists = MainManager.Instance.CsvReader.PushMyJsonIntoFrameList(Csv);
            ActionFrameList = frameLists[0];
            PoseFrameList = frameLists[1];

            Duration = ActionFrameList.Frames[^1].Timestamp;
            PoseDuration = PoseFrameList.Frames[^1].Timestamp;

            FrameManager.Init(ActionFrameList.Frames, PoseFrameList.Frames);
        }
        else if(!string.IsNullOrEmpty(Csv) && isCsv)
        {
            FrameList[] frameList = MainManager.Instance.CsvReader.PushMyCsvIntoFrameList(Csv);
            ActionFrameList = frameList[0];
            PoseFrameList = frameList[1];
            Duration = ActionFrameList.Frames[^1].Timestamp;
            PoseDuration = PoseFrameList.Frames[^1].Timestamp;
            FrameManager.Init(ActionFrameList.Frames, PoseFrameList.Frames);
        }
     


    }

    // TODO : Test, seems to work
    private AudioClip GetAudioClipFromFile(string filePath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
        www.SendWebRequest();

        while (!www.isDone)
        {
            // Waiting for the request to complete
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load MP4 audio clip: " + www.error);
            return null;
        }

        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
        return audioClip;
    }

    private string GetJsonFromFile(string filePath)
    {
        using FileStream fs = File.OpenRead(CsvFilePath);
        return fs == null ? null : new StreamReader(fs, Encoding.UTF8).ReadToEnd();
    }

    private string GetCsvFromFile(string filePath)
    {
        using StreamReader sr = File.OpenText(CsvFilePath);
        StringBuilder sb = new StringBuilder();
        string s = "";
        while ((s = sr.ReadLine()) != null)
        {
            sb.Append(s);
            sb.Append("\n");
        }

        return sb.ToString();
    }

    public void ForceFinish()
    {
        FrameManager.FrameNb = 0;
        FrameManager.PoseFrameNb = 0;
        if (ContainsAudio)
            MainManager.Instance.AudioController.StopAudioClip();
    }
}