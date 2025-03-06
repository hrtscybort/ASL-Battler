using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class PlayerTurn : State
    {
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Choose an action.");

            if (BattleSystem.Player.CurrentMana < BattleSystem.Player.MaxMana)
            {
                BattleSystem.Interface.DisableSpecialButton();
            }
            else
            {
                BattleSystem.Interface.EnableSpecialButton();
            }

            yield break;
        }

        public override IEnumerator Attack()
        {
            var isDead = BattleSystem.Enemy.Damage(BattleSystem.Player.Attack);

            BattleSystem.Player.Charge(BattleSystem.Player.Charging);

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                BattleSystem.SetState(new Won(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
            }
        }

        public override IEnumerator Heal()
        {
            Debug.Log($"{BattleSystem.Player.Name} gained health!");

            BattleSystem.Player.Heal(BattleSystem.Player.Healing);

            yield return new WaitForSeconds(1f);

            BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }

        public override IEnumerator Special()
        {
            var isDead = false;

            if (BattleSystem.Player.CurrentMana < BattleSystem.Player.MaxMana)
            {
                Debug.Log($"Need more mana!");
            }
            else
            {
                Debug.Log($"{BattleSystem.Player.Name} performed a special attack!");

                isDead = BattleSystem.Enemy.Damage(BattleSystem.Player.Special);

                BattleSystem.Player.Discharge();
            }

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                BattleSystem.SetState(new Won(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
            }
        }

        public override IEnumerator Defend()
        {
            Debug.Log($"{BattleSystem.Player.Name}'s defense rose!");

            BattleSystem.Player.Defend(BattleSystem.Player.Defending);

            yield return new WaitForSeconds(1f);

            BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }
    }
}