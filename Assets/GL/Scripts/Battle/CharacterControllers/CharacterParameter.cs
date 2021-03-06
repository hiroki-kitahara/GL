﻿using System;
using GL.Database;
using UnityEngine;
using UnityEngine.Assertions;

namespace GL.Battle
{
    [Serializable]
    public class CharacterParameter
    {
        /// <summary>
        /// ヒットポイント
        /// </summary>
        [SerializeField][Range(0.0f, 99999.0f)]
        public int HitPoint;

        /// <summary>
        /// 攻撃力
        /// </summary>
        [SerializeField][Range(0.0f, 1000.0f)]
        public int Strength;

        /// <summary>
        /// 魔法攻撃力
        /// </summary>
        [SerializeField][Range(0.0f, 1000.0f)]
        public int StrengthMagic;

        /// <summary>
        /// 防御力
        /// </summary>
        [SerializeField][Range(0.0f, 1000.0f)]
        public int Defense;

        /// <summary>
        /// 魔法防御力
        /// </summary>
        [SerializeField][Range(0.0f, 1000.0f)]
        public int DefenseMagic;

        /// <summary>
        /// 素早さ
        /// </summary>
        [SerializeField][Range(0.0f, 1000.0f)]
        public int Speed;

        /// <summary>
        /// クリティカル率
        /// </summary>
        /// <remarks>
        /// 整数に統一したいので使うときは百分率に変換する必要があります
        /// </remarks>
        [SerializeField]
        [Range(0.0f, 100.0f)]
        public int Critical;

        /// <summary>
        /// 回避率
        /// </summary>
        /// <remarks>
        /// 整数に統一したいので使うときは百分率に変換する必要があります
        /// </remarks>
        [SerializeField]
        [Range(0.0f, 50.0f)]
        public int Avoidance;

        public CharacterParameter()
        {
            this.Reset();
        }

        public CharacterParameter(CharacterParameter other)
        {
            this.HitPoint = other.HitPoint;
            this.Strength = other.Strength;
            this.StrengthMagic = other.StrengthMagic;
            this.Defense = other.Defense;
            this.DefenseMagic = other.DefenseMagic;
            this.Speed = other.Speed;
            this.Critical = other.Critical;
            this.Avoidance = other.Avoidance;
        }

        public CharacterParameter(CharacterRecord characterRecord, int level)
            : this(characterRecord.GetParameter(level))
        {
        }

        public CharacterParameter(int hitPoint, int strength, int strengthMagic, int defense, int defenseMagic, int speed, int critical, int avoidance)
        {
            this.HitPoint = hitPoint;
            this.Strength = strength;
            this.StrengthMagic = strengthMagic;
            this.Defense = defense;
            this.DefenseMagic = defenseMagic;
            this.Speed = speed;
            this.Critical = critical;
            this.Avoidance = avoidance;
        }

        public void Add(Constants.StatusParameterType type, int value)
        {
            switch (type)
            {
                case Constants.StatusParameterType.HitPoint:
                    this.HitPoint += value;
                    break;
                case Constants.StatusParameterType.Strength:
                    this.Strength += value;
                    break;
                case Constants.StatusParameterType.StrengthMagic:
                    this.StrengthMagic += value;
                    break;
                case Constants.StatusParameterType.Defense:
                    this.Defense += value;
                    break;
                case Constants.StatusParameterType.DefenseMagic:
                    this.DefenseMagic += value;
                    break;
                case Constants.StatusParameterType.Speed:
                    this.Speed += value;
                    break;
                case Constants.StatusParameterType.Critical:
                    this.Critical += value;
                    break;
                case Constants.StatusParameterType.Avoidance:
                    this.Avoidance += value;
                    break;
                default:
                    Assert.IsTrue(false, $"{type}は未対応の値です");
                    break;
            }
        }

        public int Get(Constants.StatusParameterType type)
        {
            switch (type)
            {
                case Constants.StatusParameterType.HitPoint:
                    return this.HitPoint;
                case Constants.StatusParameterType.Strength:
                    return this.Strength;
                case Constants.StatusParameterType.StrengthMagic:
                    return this.StrengthMagic;
                case Constants.StatusParameterType.Defense:
                    return this.Defense;
                case Constants.StatusParameterType.DefenseMagic:
                    return this.DefenseMagic;
                case Constants.StatusParameterType.Speed:
                    return this.Speed;
                case Constants.StatusParameterType.Critical:
                    return this.Critical;
                case Constants.StatusParameterType.Avoidance:
                    return this.Avoidance;
                default:
                    Assert.IsTrue(false, $"{type}は未対応の値です");
                    return 0;
            }
        }

        public void Reset()
        {
            this.HitPoint = 0;
            this.Strength = 0;
            this.StrengthMagic = 0;
            this.Defense = 0;
            this.DefenseMagic = 0;
            this.Speed = 0;
            this.Critical = 0;
            this.Avoidance = 0;
        }

#if UNITY_EDITOR
        public string ToCSV()
        {
            return string.Join(
                ",",
                this.HitPoint.ToString(),
                this.Strength.ToString(),
                this.StrengthMagic.ToString(),
                this.Defense.ToString(),
                this.DefenseMagic.ToString(),
                this.Speed.ToString(),
                this.Critical.ToString(),
                this.Avoidance.ToString()
                );
        }

        public int[] ToArray()
        {
            return new int[]
            {
                this.HitPoint,
                this.Strength,
                this.StrengthMagic,
                this.Defense,
                this.DefenseMagic,
                this.Speed,
                this.Critical,
                this.Avoidance
            };
        }
#endif
    }
}
