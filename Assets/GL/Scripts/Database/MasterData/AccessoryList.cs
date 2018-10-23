﻿using UnityEngine;
using UnityEngine.Assertions;

namespace GL.Database
{
    /// <summary>
    /// アクセサリーデータベース
    /// </summary>
    [CreateAssetMenu(menuName = "GL/MasterData/List/Accessory")]
    public sealed class AccessoryList : MasterDataRecordList<AccessoryRecord>
    {
        protected override string FindAssetsFilter => "t:Accessory";

        protected override string[] FindAssetsPaths => new[] { "Assets/GL/MasterData/Accessories" };
    }
}