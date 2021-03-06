﻿using System;
using GL.Battle;
using GL.Database;
using GL.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace GL.Battle.CharacterControllers
{
    [Serializable]
    public class Resistance
    {
        [SerializeField][Range(0.0f, 1.0f)]
        public float Poison;

        [SerializeField][Range(0.0f, 1.0f)]
        public float Paralysis;

        [SerializeField][Range(0.0f, 1.0f)]
        public float Sleep;

        [SerializeField][Range(0.0f, 1.0f)]
        public float Confuse;

        [SerializeField][Range(0.0f, 1.0f)]
        public float Berserk;

        [SerializeField][Range(0.0f, 1.0f)]
        public float Vitals;

        public Resistance()
        {
            this.Reset();
        }

        public Resistance(Resistance other)
        {
            this.Poison = other.Poison;
            this.Paralysis = other.Paralysis;
            this.Sleep = other.Sleep;
            this.Confuse = other.Confuse;
            this.Berserk = other.Berserk;
            this.Vitals = other.Vitals;
        }

        public Resistance(float poison, float paralysis, float sleep, float confuse, float berserk, float vitals)
        {
            this.Poison = poison;
            this.Paralysis = paralysis;
            this.Sleep = sleep;
            this.Confuse = confuse;
            this.Berserk = berserk;
            this.Vitals = vitals;
        }

        public Resistance(CharacterRecord characterRecord)
            : this(characterRecord.Resistance)
        {
        }

        public void Add(Constants.StatusAilmentType statusAilmentType, float value)
        {
            Assert.IsTrue(statusAilmentType.IsNegative(), string.Format("{0}はNegativeではありません", statusAilmentType));
            switch (statusAilmentType)
            {
                case Constants.StatusAilmentType.Poison:
                    this.Poison += value;
                    break;
                case Constants.StatusAilmentType.Paralysis:
                    this.Paralysis += value;
                    break;
                case Constants.StatusAilmentType.Sleep:
                    this.Sleep += value;
                    break;
                case Constants.StatusAilmentType.Confuse:
                    this.Confuse += value;
                    break;
                case Constants.StatusAilmentType.Berserk:
                    this.Berserk += value;
                    break;
                case Constants.StatusAilmentType.Vitals:
                    this.Vitals += value;
                    break;
                default:
                    Assert.IsTrue(false, string.Format("未対応の値です {0}", statusAilmentType));
                    break;
            }
        }
        
        public float Get(Constants.StatusAilmentType statusAilmentType)
        {
            Assert.IsTrue(statusAilmentType.IsNegative(), string.Format("{0}はNegativeではありません", statusAilmentType));
            switch (statusAilmentType)
            {
                case Constants.StatusAilmentType.Poison:
                    return this.Poison;
                case Constants.StatusAilmentType.Paralysis:
                    return this.Paralysis;
                case Constants.StatusAilmentType.Sleep:
                    return this.Sleep;
                case Constants.StatusAilmentType.Confuse:
                    return this.Confuse;
                case Constants.StatusAilmentType.Berserk:
                    return this.Berserk;
                case Constants.StatusAilmentType.Vitals:
                    return this.Vitals;
                default:
                    Assert.IsTrue(false, string.Format("未対応の値です {0}", statusAilmentType));
                    return 0.0f;
            }
        }

        /// <summary>
        /// 指定したタイプの蓄積値が満たされたか返す
        /// </summary>
        public bool IsFull(Constants.StatusAilmentType statusAilmentType)
        {
            return this.Get(statusAilmentType) >= 1.0f;
        }

        public void Reset()
        {
            this.Poison = 0.0f;
            this.Paralysis = 0.0f;
            this.Sleep = 0.0f;
            this.Confuse = 0.0f;
            this.Berserk = 0.0f;
            this.Vitals = 0.0f;
        }
    }
}
