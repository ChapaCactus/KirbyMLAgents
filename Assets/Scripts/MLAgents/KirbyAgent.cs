using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(RayPerception))]
    public class KirbyAgent : Agent
    {
        #region enums
        public enum ActionType
        {
            RightMove = 0,
            LeftMove = 1,

            RightJump = 2,
            LeftJump = 3,
        }

        public enum DirectionType
        {
            Right,
            Left,
        }
        #endregion

        #region constants
        private const float MOVE_SPEED = 0.02f;
        private const float DEAD_Y_POSITION = -5f;
        #endregion

        #region variables
        [SerializeField]
        private SpriteRenderer mainRenderer = null;

        [SerializeField]
        private ContactFilter2D groundContact = default(ContactFilter2D);
        #endregion

        #region properties
        public int HP { get; private set; }

        public DirectionType Direction { get; private set; }

        public bool IsGrounded { get; private set; }

        private Rigidbody2D Rigidbody2D { get; set; }
        private RayPerception RayPerception { get; set; }
        #endregion

        #region public methods
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            if(transform.position.y <= DEAD_Y_POSITION)
            {
                AddReward(-10f);
                Done();
                return;
            }

            CheckHP(onDead: () =>
            {
                AddReward(-10);
                Done();
                return;
            });

            var actionType = ConvertIntToActionType((int)vectorAction[0]);
            Action(actionType);

            // 行動終了毎に負の報酬を与える
            AddReward(-0.0001f);
        }

        public override void AgentOnDone()
        {
        }

        public override void AgentReset()
        {
            HP = 1;
            IsGrounded = false;
            transform.position = Vector2.zero;
        }

        public override void CollectObservations()
        {
            var rayDistance = 1.5f;
            float[] rayAngles = { 180f, 360f };
            var detectableObjects = new[] { "Block", "Goal", "Enemy" };

            AddVectorObs(RayPerception.Perceive(rayDistance, rayAngles
                                                , detectableObjects, 0, 0));
            AddVectorObs(transform.position);
        }

        public override void InitializeAgent()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            RayPerception = GetComponent<RayPerception>();
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
                    if (IsGrounded)
                    {
                        OnActionStateRightJump();
                    }
                    break;
                case ActionType.LeftJump:
                    if (IsGrounded)
                    {
                        OnActionStateLeftJump();
                    }
                    break;
            }
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
            Rigidbody2D.AddForce(new Vector2(2f, 6f), ForceMode2D.Impulse);
            mainRenderer.flipX = true;

            AddReward(-0.001f);

            OnJump();
        }

        private void OnActionStateLeftJump()
        {
            IsGrounded = false;
            Rigidbody2D.AddForce(new Vector2(-2f, 6f), ForceMode2D.Impulse);
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
            var cache = IsGrounded;
            IsGrounded = Rigidbody2D.IsTouching(groundContact);

            if(!cache && IsGrounded)
            {
                // 着地した瞬間なので加速度を切る
                Rigidbody2D.velocity = Vector2.zero;
            }
        }
        #endregion
    }
}