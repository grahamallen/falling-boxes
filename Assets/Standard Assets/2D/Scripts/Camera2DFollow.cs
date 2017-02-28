using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 0.1f;
        public float lookAheadFactor = 10;
        public float lookAheadReturnSpeed = 0;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            float yMoveDelta = (target.position - m_LastTargetPosition).y;

            bool updateLookAheadTarget = Mathf.Abs(yMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
				m_LookAheadPos = lookAheadFactor*Vector3.up*Mathf.Cos(yMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

			Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

			transform.position = new Vector3(0f, newPos.y, newPos.z);

            m_LastTargetPosition = target.position;
        }
    }
}
