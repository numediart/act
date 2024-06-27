using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using _Scripts._Version_1._0.Services.RoomServices.LiveStreamingRoomService;
using Newtonsoft.Json;
using UnityEngine.Serialization;

[Serializable]
public class CsvReader : MonoBehaviour
{
    [Header("Instances")] public TextAsset CsvToRead;

    [Header("Options")] public string DecimalSeparatorInCSV;
    public int TotNbColumn;
    public bool IgnoreFirstLine;
    public int FrameToRead;
    public int ColumnIdForFrameNb;
    public int ColumnIdForTimestamp;
    public int NbColumnToIgnoreForAu;
    public int NbColumnToIgnoreForPose;

    [Header("Data check")] public string[] data;
    public FrameList MyFrameList = new FrameList();

    #region Function Called Externally

    public FrameList[] PushMyJsonIntoFrameList(string json)
    {
        FrameList frameList = new FrameList();
        FrameList frameListPose = new FrameList();
        LiveStreamingRoomService.RecordData recordData =
            JsonConvert.DeserializeObject<LiveStreamingRoomService.RecordData>(json);
        int incomingDataSize = recordData.incomingLiveStreamData.Count;
        int incomingPoseSize = recordData.incomingPose.Count;

        frameList.Frames = new Frame[incomingDataSize];
        frameListPose.Frames = new Frame[incomingPoseSize];

        CultureInfo ci = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = DecimalSeparatorInCSV;

        // Tab filling
        for (int i = 0; i < incomingDataSize; i++)
        {
            frameList.Frames[i] = new Frame();

            //Frame Manager
            frameList.Frames[i].Number = i + 1;

            frameList.Frames[i].Timestamp = recordData.incomingLiveStreamData[i].timestamp;

            foreach (var au in recordData.incomingLiveStreamData[i]._actionUnits)
            {
                frameList.Frames[i].BlendShapeDict.Add(au.Name, au.BlendShapeList);
            }
        }

        for (int i = 0; i < incomingPoseSize; i++)
        {
            frameListPose.Frames[i] = new Frame();

            frameListPose.Frames[i].Number = i + 1;

            frameListPose.Frames[i].Timestamp = recordData.incomingPose[i].timestamp;

            frameListPose.Frames[i].PoseDict.Add("pose_Tx", recordData.incomingPose[i].pose_Tx);
            frameListPose.Frames[i].PoseDict.Add("pose_Ty", recordData.incomingPose[i].pose_Ty);
            frameListPose.Frames[i].PoseDict.Add("pose_Tz", recordData.incomingPose[i].pose_Tz);
            frameListPose.Frames[i].PoseDict.Add("pose_Rx", recordData.incomingPose[i].pose_Rx);
            frameListPose.Frames[i].PoseDict.Add("pose_Ry", recordData.incomingPose[i].pose_Ry);
            frameListPose.Frames[i].PoseDict.Add("pose_Rz", recordData.incomingPose[i].pose_Rz);
        }


        return new FrameList[] { frameList, frameListPose };
    }

    public void ReadCsv()
    {
        data = CsvToRead.text.Split(new string[] { ", ", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / TotNbColumn - (IgnoreFirstLine ? 1 : 0);
        MyFrameList.Frames = new Frame[tableSize];

        CultureInfo ci = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = DecimalSeparatorInCSV;

        // Tab filling
        for (int i = 0; i < tableSize; i++)
        {
            MyFrameList.Frames[i] = new Frame();

            //Frame Manager
            MyFrameList.Frames[i].Number = int.Parse(
                data[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + ColumnIdForFrameNb],
                NumberStyles.Any, ci);

            MyFrameList.Frames[i].Timestamp = double.Parse(
                data[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + ColumnIdForTimestamp],
                NumberStyles.Any, ci);

            // Poses
            for (int j = 0; j < NbColumnToIgnoreForAu - NbColumnToIgnoreForPose; j++)
            {
                MyFrameList.Frames[i].PoseDict.Add(data[NbColumnToIgnoreForPose + j],
                    double.Parse(data[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + NbColumnToIgnoreForPose + j],
                        NumberStyles.Any, ci));
            }

            // Action units
            for (int j = 0; j < TotNbColumn - NbColumnToIgnoreForAu; j++)
            {
                MyFrameList.Frames[i].ActionUnitDict.Add(data[NbColumnToIgnoreForAu + j],
                    double.Parse(data[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + NbColumnToIgnoreForAu + j],
                        NumberStyles.Any, ci));
            }
        }
    }

    public FrameList PushMyCsvIntoFrameList(string csv)
    {
        FrameList frameList = new FrameList();

        string[] csvDataTab = csv.Split(new string[] { ", ", "\n" }, StringSplitOptions.None);
        int tableSize = csvDataTab.Length / TotNbColumn - (IgnoreFirstLine ? 1 : 0);
        frameList.Frames = new Frame[tableSize];

        CultureInfo ci = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = DecimalSeparatorInCSV;

        // Tab filling
        for (int i = 0; i < tableSize; i++)
        {
            frameList.Frames[i] = new Frame();

            //Frame Manager
            frameList.Frames[i].Number = int.Parse(
                csvDataTab[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + ColumnIdForFrameNb],
                NumberStyles.Any, ci);

            frameList.Frames[i].Timestamp = double.Parse(
                csvDataTab[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + ColumnIdForTimestamp],
                NumberStyles.Any, ci);

            // Poses
            for (int j = 0; j < NbColumnToIgnoreForAu - NbColumnToIgnoreForPose; j++)
            {
                frameList.Frames[i].PoseDict.Add(csvDataTab[NbColumnToIgnoreForPose + j],
                    double.Parse(
                        csvDataTab[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + NbColumnToIgnoreForPose + j],
                        NumberStyles.Any, ci));
            }

            // Action units
            for (int j = 0; j < TotNbColumn - NbColumnToIgnoreForAu; j++)
            {
                frameList.Frames[i].ActionUnitDict.Add(csvDataTab[NbColumnToIgnoreForAu + j],
                    double.Parse(csvDataTab[TotNbColumn * (i + (IgnoreFirstLine ? 1 : 0)) + NbColumnToIgnoreForAu + j],
                        NumberStyles.Any, ci));
            }
        }

        return frameList;
    }

    public void ClearCsv()
    {
        data = new string[] { };
        MyFrameList = new FrameList();
    }

    public void ReadFrame()
    {
        // To check
        foreach (var pair in MyFrameList.Frames[FrameToRead].PoseDict)
        {
            Debug.Log(pair.Key + " " + pair.Value);
        }
    }

    #endregion
}