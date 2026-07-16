using System.Collections.Generic;
using CustomPlayerEffects;
using MEC;
using Mirror;
using RelativePositioning;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Hazards
{
    public class TantrumEnvironmentalHazard : TemporaryHazard
    {
        public static readonly List<TantrumEnvironmentalHazard> AllTantrums = new List<TantrumEnvironmentalHazard>();

        [SyncVar(hook = nameof(UpdatePosition))]
        private RelativePosition _synchronizedPosition;

        [SerializeField]
        private Transform _correctPosition;

        [SerializeField]
        private DecalProjector _decal;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private float _hazardDuration = 10f;

        protected override float HazardDuration => _hazardDuration;

        private bool _isDespawning;

        public override Vector3 SourcePosition
        {
            get => _correctPosition.position + SourceOffset;
            set => transform.position = value;
        }

        public RelativePosition SynchronizedPosition
        {
            get => _synchronizedPosition;
            set
            {
                if (NetworkServer.active)
                    _synchronizedPosition = value;
            }
        }

        [SyncVar]
        public bool PlaySizzle;

        private void Awake()
        {
            AllTantrums.Add(this);
        }

        private void OnDestroy()
        {
            AllTantrums.Remove(this);
        }

        private void UpdatePosition(RelativePosition oldPos, RelativePosition newPos)
        {
            transform.position = newPos.Position;
        }


        [ClientRpc]
        public void RpcDespawn(bool playSizzle)
        {
            if (!NetworkClient.active)
            {
                Debug.LogError("RPC RpcDespawn called on server.");
                return;
            }

            this._isDespawning = true;

            if (_decal != null)
            {
                Material material = new Material(_decal.material);
                _decal.material = material;
            }

            if (_animator != null)
            {
                _animator.SetBool("queueDestroy", true);
                _animator.SetBool("playSizzle", playSizzle);
            }
        }


        public override void OnStartServer()
        {
            base.OnStartServer();
            SynchronizedPosition = new RelativePosition(transform.position);
        }

        public override void OnEnter(ReferenceHub hub)
        {
            if (NetworkServer.active)
                hub.playerEffectsController.EnableEffect<Sinkhole>();
        }

        public override void OnExit(ReferenceHub hub)
        {
            if (NetworkServer.active)
                hub.playerEffectsController.DisableEffect<Sinkhole>();
        }

        [Server]
        public void RequestDestruction(float waitTime)
        {
            Timing.RunCoroutine(DelayedPuddleRemoval(waitTime));
        }

        private IEnumerator<float> DelayedPuddleRemoval(float waitTime)
        {
            yield return Timing.WaitForSeconds(waitTime);
            NetworkServer.Destroy(gameObject);
        }
    }
}