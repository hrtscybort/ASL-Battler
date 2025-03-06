using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Won : State
    {
        public Won(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Enemy defeated! Prepare for the next round...");

            yield return new WaitForSeconds(2f);

            BattleSystem.Interface.Initialize(BattleSystem.Player, BattleSystem.Enemy);

            BattleSystem.SetState(new Begin(BattleSystem));
        }
    }
}