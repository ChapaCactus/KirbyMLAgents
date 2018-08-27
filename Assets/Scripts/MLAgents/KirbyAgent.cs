using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class KirbyAgent : Agent
    {
        #region enums
        public enum ActionType
        {
            None,

            RightMove,
            LeftMove,
            //Jump,
        }
        #endregion

        #region constants
        private const float MOVE_SPEED = 0.01f;
        #endregion

        #region properties
        public int HP { get; private set; }
        #endregion

        #region public methods
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            CheckHP(() =>
            {
                Done();
            });

            var actionType = ConvertIntToActionType((int)vectorAction[0]);
            Action(actionType);
        }

        public override void AgentOnDone()
        {
        }

        public override void AgentReset()
        {
            HP = 1;
            transform.position = Global.Stage.StartPos;
        }

        public override void CollectObservations()
        {
        }

        public override void InitializeAgent()
        {
        }

        public void SetPosition(float x, float y)
        {
            transform.position = new Vector2(x, y);
        }
        #endregion

        #region private methods
        private void Action(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.None:
                    OnActionStateNone();
                    break;
                case ActionType.RightMove:
                    OnActionStateRightMove();
                    break;
                case ActionType.LeftMove:
                    OnActionStateLeftMove();
                    break;
            }
        }

        private void OnActionStateNone()
        {
            AddReward(-0.01f);
        }

        private void OnActionStateRightMove()
        {
            var x = (transform.position.x + MOVE_SPEED);
            SetPosition(x, transform.position.y);

            AddReward(0.01f);
        }

        private void OnActionStateLeftMove()
        {
            var x = (transform.position.x - MOVE_SPEED);
            SetPosition(x, transform.position.y);

            AddReward(0.01f);
        }

        private void CheckHP(Action onDead)
        {
            if (HP <= 0)
            {
                onDead();
            }
        }

        /// <summary>
        /// AIステートを行動Enumに変換
        /// </summary>
        private ActionType ConvertIntToActionType(int typeInt)
        {
            return (ActionType)typeInt;
        }
        #endregion
    }
}