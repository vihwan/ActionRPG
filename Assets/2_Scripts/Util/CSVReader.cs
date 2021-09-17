using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SG
{
    public static class CSVReader
    {
        public static TextAsset textAssetData;

        public static List<LevelToExperience> ReadCSV_ExpTable()
        {
            textAssetData = Resources.Load<TextAsset>("CSV/ExpTable");
            List<LevelToExperience> levelToExperiences = new List<LevelToExperience>();
            string[] data = textAssetData.text.Split(new string[] {"\n"}, StringSplitOptions.None);
            
            //csv파일 마지막에는 엔터가 하나 있어 빈 줄이 하나 추가되어있다. 따라서 -1을 해줘야한다.
            //i = 0번째는 데이터의 속성을 나타내는 구간이므로 1부터 시작해야한다.
            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] row = data[i].Split(new char[]{','});
                LevelToExperience levelToexp = new LevelToExperience();
                int.TryParse(row[0], out levelToexp.Level);
                int.TryParse(row[1], out levelToexp.Experience);
                levelToExperiences.Add(levelToexp);
            }

            return levelToExperiences;
        }
    }
}