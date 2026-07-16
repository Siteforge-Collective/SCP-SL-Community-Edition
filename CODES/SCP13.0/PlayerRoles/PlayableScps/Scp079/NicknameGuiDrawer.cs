using System;
using System.Collections.Generic;
using PlayerRoles.FirstPersonControl.Thirdperson;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
    public class NicknameGuiDrawer : MonoBehaviour
    {
        private const float DefaultDistance = 17f;
        private const float AdditionalSize = 3f;
        private const float HeadSize = 270f;
        private const float ScpHeadApproximation = 1.5f;

        private static Vector3 _scpHeadOffset = Vector3.zero;

        private Camera _cam;
        private RectTransform _fullscreenRect;
        private RectTransform _template;
        private bool _forceShowNicknames;

        private readonly Queue<RectTransform> _pool = new Queue<RectTransform>();
        private readonly HashSet<RectTransform> _instances = new HashSet<RectTransform>();

        public void Init(Camera cam, RectTransform fullScreenRect, RectTransform template, bool forceShowNicknames = false)
        {
            this._cam = cam;
            this._fullscreenRect = fullScreenRect;
            this._template = template;
            this._forceShowNicknames = forceShowNicknames;
        }

        public void TryDrawPlayer(CharacterModel model, string nickname, float zoom, Vector3 curPos, bool isScp = false)
        {
            if (model != null)
            {
                HitboxIdentity[] hitboxes = model.Hitboxes;
                int length = hitboxes.Length;

                for (int i = 0; i < length; i++)
                {
                    HitboxIdentity hitboxIdentity = hitboxes[i];

                    if (hitboxIdentity.HitboxType != (HitboxType)2)
                        continue;

                    Collider[] targetColliders = hitboxIdentity.TargetColliders;
                    if (targetColliders.Length == 0)
                        goto INSN_18048AE5E;

                    Collider collider = targetColliders[0];
                    if (collider == null)
                        goto INSN_18048AE5E;

                    CapsuleCollider capsule = collider as CapsuleCollider;

                    Vector3 centerPos;
                    if (capsule != null)
                    {
                        centerPos = hitboxIdentity.transform.TransformPoint(capsule.center);
                    }
                    else
                    {
                        centerPos = hitboxIdentity.transform.TransformPoint(collider.bounds.center);
                    }

                    this.DrawRectangleIfVisible(centerPos, zoom, nickname, curPos);
                    return;

                INSN_18048AE5E:
                    return;
                }
            }

            if (model != null)
            {
                Vector3 position = model.transform.position;
                this.DrawRectangleIfVisible(position + Vector3.up * 1.8f, zoom, nickname, curPos);
            }
        }

        private void DrawRectangleIfVisible(Vector3 pos, float zoom, string nickname, Vector3 curPos)
        {
            float maxDistance = zoom * DefaultDistance;
            Transform camTransform = _cam.transform;

            if (Vector3.Distance(camTransform.position, pos) <= maxDistance)
            {
                int visionLayerMask = PlayerRoles.PlayableScps.VisionInformation.VisionLayerMask;

                if (!Physics.Linecast(camTransform.position, pos, visionLayerMask))
                {
                    this.DrawRectangle(pos, zoom, nickname, curPos);
                }
            }
        }

        private void DrawRectangle(Vector3 targetPos, float zoom, string nickname, Vector3 curPos)
        {
            if (this._fullscreenRect == null) return;

            if (_cam.transform.InverseTransformPoint(targetPos).z <= 0) return;

            RectTransform rectTransform;
            if (!this._pool.TryDequeue(out rectTransform))
            {
                rectTransform = Instantiate<RectTransform>(this._template, this._template.parent);
            }

            float distance = Vector3.Distance(targetPos, _cam.transform.position);
            Vector3 screenPoint = this._cam.WorldToScreenPoint(targetPos);

            rectTransform.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y);

            float sizeValue = (zoom * HeadSize) / distance;
            rectTransform.sizeDelta = new Vector2(sizeValue, sizeValue);

            rectTransform.gameObject.SetActive(true);

            TextMeshProUGUI textMesh = rectTransform.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                string textToDisplay = string.Empty;

                if (this._forceShowNicknames)
                {
                    textToDisplay = nickname;
                }
                else
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
                    {
                        textToDisplay = nickname;
                    }
                }

                textMesh.text = textToDisplay;
            }

            this._instances.Add(rectTransform);
        }

        public void ClearAll()
        {
            foreach (RectTransform instance in this._instances)
            {
                instance.gameObject.SetActive(false);
                this._pool.Enqueue(instance);
            }
            this._instances.Clear();
        }
    }
}