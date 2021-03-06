﻿using UnityEngine;
using UnityEngine.Assertions;
using System;
using GL.Battle.CharacterControllers;
using System.Linq;
using HK.GL.Extensions;
using GL.Database;
using HK.Framework.Text;
using GL.Battle.CharacterControllers.JobSystems;
using static GL.Database.CharacterRecord;
using System.Collections.Generic;
using GL.Extensions;
using GL.Battle;

#if UNITY_EDITOR
using UnityEditor;
namespace GL.DeveloperTools
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnemyImporter
    {
        [MenuItem("GL/MasterData/Import Enemy")]
        public static void Import()
        {
            EnemyAIImporter.Import();
            var enemyData = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GL/MasterData/RawData/GL - Enemy.csv")
                .text
                .Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var enemyOtherData = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GL/MasterData/RawData/GL - EnemyOther.csv")
                .text
                .Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var enemyMaterialData = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GL/MasterData/RawData/GL - EnemyMaterial.csv")
                .text
                .Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var enemyName = "";
            OtherData otherData = null;
            var typeId = 0;
            var enemyNameAsset = AssetDatabase.LoadAssetAtPath<StringAsset>("Assets/GL/StringAssets/EnemyName.asset");
            var job = AssetDatabase.LoadAssetAtPath<Job>("Assets/GL/MasterData/Jobs/Enemy/0000.asset");
            var colorAsset = AssetDatabase.LoadAssetAtPath<ColorAsset>("Assets/GL/ColorAssets/EnemyIcon.asset");
            for (var i = 1; i < enemyData.Length; i++)
            {
                var splitEnemyData = enemyData[i].Split(',');
                var tempEnemyName = splitEnemyData[1];
                var level = int.Parse(splitEnemyData[4]);
                if(enemyName != tempEnemyName)
                {
                    otherData = new OtherData(enemyOtherData.Find(x => x.IndexOf(tempEnemyName) >= 0));
                    typeId++;
                }
                enemyName = tempEnemyName;
                var materialLotteries = GetMaterialLotteries(enemyMaterialData, tempEnemyName, level);
                var iconId = int.Parse(splitEnemyData[2]);
                var colorId = int.Parse(splitEnemyData[3]);
                var path = "Assets/GL/MasterData/Characters/Enemy/";
                var fileName = splitEnemyData[0].RemoveNewLine();
                var enemyAsset = ImporterUtility.GetOrCreate<CharacterRecord>(path, fileName);
                var parameter = new CharacterParameter(
                    int.Parse(splitEnemyData[6]),
                    int.Parse(splitEnemyData[7]),
                    int.Parse(splitEnemyData[8]),
                    int.Parse(splitEnemyData[9]),
                    int.Parse(splitEnemyData[10]),
                    int.Parse(splitEnemyData[11]),
                    int.Parse(splitEnemyData[12]),
                    int.Parse(splitEnemyData[13])
                );
                enemyAsset.SetAsEnemy(
                    AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/GL/Textures/Enemy/Enemy{iconId}.png"),
                    colorAsset.Colors[colorId - 1],
                    int.Parse(splitEnemyData[5]),
                    enemyNameAsset.CreateOrGetFinder(enemyName),
                    job,
                    parameter,
                    parameter,
                    otherData.Resistance,
                    otherData.Attribute,
                    new CharacterRecord.GrowthCurve(),
                    new CharacterRecord.ExperienceData(),
                    int.Parse(splitEnemyData[14]),
                    int.Parse(splitEnemyData[15]),
                    materialLotteries,
                    level
                );

                EditorUtility.SetDirty(enemyAsset);
            }

            AssetDatabase.SaveAssets();
        }

        private static MaterialLottery[] GetMaterialLotteries(string[] materialData, string enemyName, int level)
        {
            var materialDatabase = AssetDatabase.LoadAssetAtPath<MaterialList>("Assets/GL/MasterData/Database/Material.asset");
            return materialData
                .Where(m =>
                {
                    var s = m.Split(',');
                    return s[0] == enemyName && int.Parse(s[1]) == level;
                })
                .Select(m =>
                {
                    var s = m.Split(',');
                    return new MaterialLottery(materialDatabase.List.Find(x => x.MaterialName == s[2]), float.Parse(s[3]));
                })
                .ToArray();
        }

        public class OtherData
        {
            public Resistance Resistance { get; private set; }

            public Battle.CharacterControllers.Attribute Attribute { get; private set; }

            public OtherData(string data)
            {
                var s = data.Split(',');
                this.Resistance = new Resistance(
                    float.Parse(s[1]),
                    float.Parse(s[2]),
                    float.Parse(s[3]),
                    float.Parse(s[4]),
                    float.Parse(s[5]),
                    float.Parse(s[6])
                );
                this.Attribute = new Battle.CharacterControllers.Attribute(
                    float.Parse(s[7]),
                    float.Parse(s[8]),
                    float.Parse(s[9]),
                    float.Parse(s[10]),
                    float.Parse(s[11]),
                    float.Parse(s[12]),
                    float.Parse(s[13])
                );
            }
        }
    }
}
#endif
