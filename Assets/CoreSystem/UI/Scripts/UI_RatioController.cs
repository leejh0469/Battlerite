using UnityEngine;

namespace Coresystem.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UI_RatioController : MonoBehaviour
    {
        public enum H_Alignment { Left, Center, Right, Stretch, Custom }
        public enum V_Alignment { Top, Middle, Bottom, Stretch, Custom }

        [Header("Horizontal Settings")]
        public H_Alignment hAlign = H_Alignment.Stretch; // 기본값: 꽉 채우기
        [Range(-0.5f, 1.5f)] public float widthRatio = 1f;      // 가로 크기 (1 = 100%)
        [Range(-0.5f, 1.5f)] public float customXOrigin = 0.5f; // Custom 모드일 때 중심점

        [Header("Vertical Settings")]
        public V_Alignment vAlign = V_Alignment.Stretch; // 기본값: 꽉 채우기
        [Range(-0.5f, 1.5f)] public float heightRatio = 1f;     // 세로 크기 (1 = 100%)
        [Range(-0.5f, 1.5f)] public float customYOrigin = 0.5f; // Custom 모드일 때 중심점

        private void Start()
        {
            UpdateUILayout();
        }

        private void OnValidate()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != null) UpdateUILayout();
            };
    #endif
        }

        public void UpdateUILayout()
        {
            RectTransform rect = GetComponent<RectTransform>();
            if (rect == null) return;

            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.zero;

            // 수평 기준 크기 비율 및 위치 설정
            switch (hAlign)
            {
                case H_Alignment.Left: // 왼쪽 기준
                    anchorMin.x = 0;
                    anchorMax.x = widthRatio;
                    break;

                case H_Alignment.Right: // 오른쪽 기준
                    anchorMin.x = 1f - widthRatio;
                    anchorMax.x = 1f;
                    break;

                case H_Alignment.Center: // 중앙 기준
                    float halfW = widthRatio / 2f;
                    anchorMin.x = 0.5f - halfW;
                    anchorMax.x = 0.5f + halfW;
                    break;

                case H_Alignment.Stretch: // 꽉 채우기
                    anchorMin.x = 0;
                    anchorMax.x = 1;
                    break;

                case H_Alignment.Custom: // 사용자 지정 중심점
                    float halfCustomW = widthRatio / 2f;
                    anchorMin.x = customXOrigin - halfCustomW;
                    anchorMax.x = customXOrigin + halfCustomW;
                    break;
            }

            // 수직 기준 크기 비율 및 위치 설정
            switch (vAlign)
            {
                case V_Alignment.Bottom: // 바닥 기준 (0에서 시작해서 위로)
                    anchorMin.y = 0;
                    anchorMax.y = heightRatio;
                    break;

                case V_Alignment.Top: // 천장 기준 (1에서 시작해서 아래로)
                    anchorMin.y = 1f - heightRatio;
                    anchorMax.y = 1f;
                    break;

                case V_Alignment.Middle: // 중앙 기준
                    float halfH = heightRatio / 2f;
                    anchorMin.y = 0.5f - halfH;
                    anchorMax.y = 0.5f + halfH;
                    break;

                case V_Alignment.Stretch: // 꽉 채우기
                    anchorMin.y = 0;
                    anchorMax.y = 1;
                    break;

                case V_Alignment.Custom: // 사용자 지정 중심점
                    float halfCustomH = heightRatio / 2f;
                    anchorMin.y = customYOrigin - halfCustomH;
                    anchorMax.y = customYOrigin + halfCustomH;
                    break;
            }

            // 앵커 적용 및 오프셋 초기화
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}