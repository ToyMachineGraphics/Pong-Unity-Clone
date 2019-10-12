using System;
using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    public enum Handedness
    {
        Left, Right
    }

    [Serializable]
    public class SaveData
    {
        public int pointsTowin = 3;
        public Handedness handedness;
        public bool musicOn = true;
        public bool soundOn = true;

        public const int TOP_N = 10;
        public List<int> localTopPoints = new List<int>(TOP_N);
        //{
        //    0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        //};
        public int lastSessionPoint = -1;

        public SaveData()
        {
            for (int i = 0; i < TOP_N; i++)
            {
                localTopPoints.Add(0);
            }
        }

        public void Save()
        {
            while (localTopPoints.Count > TOP_N)
            {
                localTopPoints.RemoveRange(TOP_N, localTopPoints.Count - TOP_N);
            }
            string json = JsonUtility.ToJson(this);
            Debug.LogFormat("Save: {0}", json);
            PlayerPrefs.SetString("SaveData", json);
        }

        public static SaveData Load()
        {
            string json = PlayerPrefs.GetString("SaveData", "{}");
            SaveData instance = JsonUtility.FromJson<SaveData>(json);
            return instance;
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void WriteToDisk()
        {
            PlayerPrefs.Save();
        }

        public int InsertToBillboard(int point)
        {
            int index = localTopPoints.Count - 1;
            for (; index >= 0; index--)
            {
                if (point <= localTopPoints[index])
                {
                    index++;
                    break;
                }
            }
            if (index < 0)
            {
                index = 0;
            }

            if (index == localTopPoints.Count - 1 && point <= localTopPoints[index])
            {
                localTopPoints.Add(point);
                return localTopPoints.Count - 1;
            }
            else
            {
                localTopPoints.Insert(index, point);
                return index;
            }
        }
    }
}