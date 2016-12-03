using Assets.AI;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Game.Enemies
{
    public class BustController : EnemyController
    {

        /// <summary>
        /// Attack the player and activate armors without screen shake
        /// </summary>
        /// <param name="Player">Player</param>
        /// <param name="Cooldown">Cooldown until next alarm</param>
        /// <param name="Damage">Damage in percent, 0.5f = 50%</param>
        /// <param name="Armors">List of armor AI this bust is connectedd to</param>
        public void Attack(PlayerController Player, float Cooldown, float Damage, List<ArmorAI> Armors)
        {
            Attack(Player, Cooldown, Damage, new Vector2(0, 0), 0, 0, Armors);
        }

        /// <summary>
        /// Attack the player and activate armors with screen shake
        /// </summary>
        /// <param name="Player">Player</param>
        /// <param name="Cooldown">Cooldown until next alarm</param>
        /// <param name="Damage">Damage in percent, 0.5f = 50%</param>
        /// <param name="ShakeDirection">Normal vector for camera shake, should be vector from enemy to player</param>
        /// <param name="ShakeDuration">How long to shake</param>
        /// <param name="ShakeIntensity">How jarring the shake</param>
        /// <param name="Armors">List of armor AI this bust is connectedd to</param>
        public void Attack(PlayerController Player, float Cooldown, float Damage, Vector2 ShakeDirection, float ShakeDuration, float ShakeIntensity, List<ArmorAI> Armors)
        {
            base.Attack(Player, Cooldown, Damage, ShakeDirection, ShakeDuration, ShakeIntensity);
            foreach (ArmorAI armor in Armors)
            {
                armor.ActivateSuit();
            }
        }
    }
}