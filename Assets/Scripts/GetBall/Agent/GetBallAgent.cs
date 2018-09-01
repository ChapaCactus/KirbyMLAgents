using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class GetBallAgent : Agent
    {
        #region constants
        private const float START_POSITION_Y = -0.7f;

        private const int MOVE_MIN_POSITION = -10;
        private const int MOVE_MAX_POSITION = 10;
        private const int START_MIN_POSITION = -6;
        private const int START_MAX_POSITION = 6;

        private const int SMALL_GOAL_POSITION = -7;
        private const int BIG_GOAL_POSITION = 7;
        // 各ゴールの報酬
        private const float SMALL_GOAL_REWARD = 1f;
        private const float BIG_GOAL_REWARD = 2f;

        private const float DECISION_INTERVAL = 0.25f;
        #endregion

        #region variables
        [SerializeField]
        private Academy m_academy = null;

        [SerializeField]
        private GameObject m_smallGoal = null;
        [SerializeField]
        private GameObject m_bigGoal = null;
        #endregion

        #region properties
        public int position { get; private set; }

        public Move[] moves { get; private set; }
        public Transform cachedTransform { get; private set; }

        private float m_timer { get; set; }
        #endregion

        #region unity callbacks
        private void FixedUpdate()
        {
            WaitTimeInterface();
        }
        #endregion

        #region public methods
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            var actionIndex = (int)vectorAction[0];
            if (actionIndex <= -1)
                return;

            position += moves[actionIndex].Value;
            if (position < MOVE_MIN_POSITION) { position = MOVE_MIN_POSITION; }
            if (position > MOVE_MAX_POSITION) { position = MOVE_MAX_POSITION; }
            Debug.Log(position);

            cachedTransform.position = new Vector2(position, cachedTransform.position.y);

            if (position == SMALL_GOAL_POSITION)
            {
                // 小ゴール衝突
                AddReward(SMALL_GOAL_REWARD);
                Done();
            }
            else if (position == BIG_GOAL_POSITION)
            {
                // 大ゴール衝突
                AddReward(BIG_GOAL_REWARD);
                Done();
            }
            else
            {
                // 衝突無し
                AddReward(-0.01f);
            }
        }

        public override void AgentOnDone()
        {
        }

        public override void InitializeAgent()
        {
            moves = new Move[]
            {
                Move.CreateLeftMove(),
                Move.CreateRightMove(),
            };
            cachedTransform = GetComponent<Transform>();
        }

        public override void AgentReset()
        {
            m_timer = DECISION_INTERVAL;
            // 初期位置決定
            var randomStartX = UnityEngine.Random.Range(START_MIN_POSITION, (START_MAX_POSITION + 1));
            position = randomStartX;
            cachedTransform.position = new Vector2(position, START_POSITION_Y);

            m_smallGoal.transform.position = new Vector2(SMALL_GOAL_POSITION, START_POSITION_Y);
            m_bigGoal.transform.position = new Vector2(BIG_GOAL_POSITION, START_POSITION_Y);
        }

        public override void CollectObservations()
        {
            AddVectorObs(position);
        }
        #endregion

        #region private methods
        private void WaitTimeInterface()
        {
            if (m_academy.GetIsInference())
            {
                if (m_timer > 0)
                {
                    m_timer -= Time.deltaTime;
                }
                else
                {
                    m_timer = DECISION_INTERVAL;
                    RequestDecision();
                }
            }
            else
            {
                // 学習時はフレーム毎に呼び出す
                RequestDecision();
            }
        }
        #endregion
    }
}