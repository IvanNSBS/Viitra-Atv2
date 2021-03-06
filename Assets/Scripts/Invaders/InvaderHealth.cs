﻿using Common;
using DG.Tweening;
using UnityEngine;

namespace Invaders
{
    #pragma warning disable CS0649
    public class InvaderHealth : EntityHealth
    {
        #region Fields
        private Sequence m_damageSequence;
        private bool m_dying;
        private InvadersController m_controllerReference;
        private BulletShooter m_shooterReference;
        #endregion Fields
        
        
        #region Methods
        public void Initialize(BulletShooter shooter, InvadersController controller)
        {
            m_shooterReference = shooter;
            m_controllerReference = controller;
        }
        #endregion Methods
        
        
        #region EntityHealth Methods
        protected override void Die()
        {
            if (m_dying)
                return;

            m_dying = true;
            m_damageSequence.Kill();
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(new Vector3(0.75f, 0.75f, 1), 0.15f));
            sequence.Append(transform.DOScale(new Vector3(1.9f, 1.9f, 1), 0.5f).SetEase(Ease.OutQuint));
            sequence.AppendCallback(() =>
            {
                SpawnDamageEffect(gameObject.transform.position);

                if(m_controllerReference)
                    m_controllerReference.RemoveShooter(m_shooterReference);
                
                onDeath?.Invoke();
                Destroy(gameObject);
            });
        }

        protected override void DamageTakenAnimation()
        {
            if (m_dying)
                return;
            
            m_damageSequence = DOTween.Sequence();
            m_damageSequence.Append(transform.DOShakeScale(0.4f, new Vector3(0.8f, 0.9f), 15));
        }
        #endregion EntityHealth Methods
    }
}