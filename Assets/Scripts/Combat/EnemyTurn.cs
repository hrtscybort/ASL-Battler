using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class EnemyTurn : State
    {
        public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log($"{BattleSystem.Enemy.Name} attacks!");

            var isDead = BattleSystem.Player.Damage(BattleSystem.Enemy.Attack);

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                BattleSystem.SetState(new Lost(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }
        }
    }
}