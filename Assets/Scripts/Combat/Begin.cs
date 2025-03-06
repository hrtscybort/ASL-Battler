using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Begin : State
    {
        public Begin(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            BattleSystem.InitializeEnemy();

            BattleSystem.Interface.InitializeEnemy(BattleSystem.Enemy);

            Debug.Log($"A wild {BattleSystem.Enemy.Name} appeared!");

            yield return new WaitForSeconds(2f);

            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
    }
}