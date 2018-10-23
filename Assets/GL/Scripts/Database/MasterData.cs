﻿using UnityEngine;
using UnityEngine.Assertions;

namespace GL.Database
{
    /// <summary>
    /// データベース
    /// </summary>
    [CreateAssetMenu(menuName = "GL/MasterData/Database")]
    public class MasterData : ScriptableObject
    {
        private static MasterData instance;

        [SerializeField]
        private Character character;
        public static Character Character => instance.character;

        [SerializeField]
        private WeaponList weapon;
        public static WeaponList Weapon => instance.weapon;

        [SerializeField]
        private Accessory accessory;
        public static Accessory Accessory => instance.accessory;

        [SerializeField]
        private MaterialList material;
        public static MaterialList Material => instance.material;

        [SerializeField]
        private EnemyParty enemyParty;
        public static EnemyParty EnemyParty => instance.enemyParty;

        public void Setup()
        {
            Assert.IsNull(instance);
            instance = this;
        }
    }
}
