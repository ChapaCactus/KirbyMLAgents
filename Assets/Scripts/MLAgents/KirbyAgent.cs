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
            RightMove,
            LeftMove,

            RightJump,
            LeftJump,
        }

        public enum DirectionType
        {
            Right,
            Left,
        }
        #endregion

        #region constants
        private const float MOVE_SPEED = 0.02f;
        #endregion

        #region variables
        [SerializeField]
        private SpriteRenderer mainRenderer = null;

        [SerializeField]
        private Rigidbody2D rigid2D = null;

        [SerializeField]
        private ContactFilter2D groundContact = default(ContactFilter2D);
        #endregion

        #region properties
        public int HP { get; private set; }

        public DirectionType Direction { get; private set; }

        public float JumpCooltime { get; private set; }
        public bool IsGrounded { get; private set; }
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
            IsGrounded = false;
            JumpCooltime = 0;
            transform.position = Vector2.zero;
        }

        public override void CollectObservations()
        {
            AddVectorObs(GetActionTypeSize());
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
            CheckIsGrounded();
            if (!IsGrounded)// 非接地中は何もしない
            {
                OnActionStateNone();
                return;
            }

            switch (actionType)
            {
                case ActionType.RightMove:
                    OnActionStateRightMove();
                    break;
                case ActionType.LeftMove:
                    OnActionStateLeftMove();
                    break;

                case ActionType.RightJump:
                    if (JumpCooltime <= 0)
                    {
                        OnActionStateRightJump();
                    }
                    else
                    {
                        // ジャンプ出来なければ、移動を行う
                        OnActionStateRightMove();
                    }
                    break;
                case ActionType.LeftJump:
                    if (JumpCooltime <= 0)
                    {
                        OnActionStateLeftJump();
                    }
                    else
                    {
                        // ジャンプ出来なければ、移動を行う
                        OnActionStateLeftMove();
                    }
                    break;
            }

            JumpCooltime -= 0.01f;
        }

        private void OnActionStateNone()
        {
            AddReward(-0.001f);
        }

        private void OnActionStateRightMove()
        {
            var x = (transform.position.x + MOVE_SPEED);
            SetPosition(x, transform.position.y);

            Direction = DirectionType.Right;
            mainRenderer.flipX = true;

            AddReward(-0.001f);
        }

        private void OnActionStateLeftMove()
        {
            var x = (transform.position.x - MOVE_SPEED);
            SetPosition(x, transform.position.y);

            Direction = DirectionType.Left;
            mainRenderer.flipX = false;

            AddReward(-0.001f);
        }

        private void OnActionStateRightJump()
        {
            IsGrounded = false;
            rigid2D.AddForce(new Vector2(2f, 6f), ForceMode2D.Impulse);
            mainRenderer.flipX = true;

            AddReward(-0.001f);

            OnJump();
        }

        private void OnActionStateLeftJump()
        {
            IsGrounded = false;
            rigid2D.AddForce(new Vector2(-2f, 6f), ForceMode2D.Impulse);
            mainRenderer.flipX = false;

            AddReward(-0.001f);

            OnJump();
        }

        private void CheckHP(Action onDead)
        {
            if (HP <= 0)
            {
                onDead();
            }
        }

        private void OnJump()
        {
            JumpCooltime = 3f;
        }

        /// <summary>
        /// AIステートを行動Enumに変換
        /// </summary>
        private ActionType ConvertIntToActionType(int typeInt)
        {
            return (ActionType)typeInt;
        }

        /// <summary>
        /// ActionTypeの要素数取得
        /// </summary>
        private int GetActionTypeSize()
        {
            return Enum.GetValues(typeof(ActionType)).Length;
        }

        private void CheckIsGrounded()
        {
            IsGrounded = rigid2D.IsTouching(groundContact);
        }
        #endregion
    }
}