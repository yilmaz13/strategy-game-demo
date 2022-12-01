using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HpSystem
{
    public enum Team
    {
        Player,
        Enemy
    }
    public class DamageInfo
    {
        public float Damage { get; set; }
        public Team Team { get; set; }

        public DamageInfo(float damage, Team team)
        {
            Damage = damage;
            Team = team;
        }
    }
}