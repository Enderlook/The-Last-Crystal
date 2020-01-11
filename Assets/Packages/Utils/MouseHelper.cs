#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utils
{
    public class MouseHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        public static Vector2 GetMousePositionInEditor() =>
            /* Draw closest node to mouse
             * https://answers.unity.com/questions/1321651/i-need-to-get-a-vector2-of-the-mouse-position-whil.html
             * http://answers.unity.com/answers/1323496/view.html */
            HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(1);
#endif

        public static Vector2 GetMousePositionInGame() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        public static Vector2 GetMousePositionInGame(Camera camera) => camera.ScreenToWorldPoint(Input.mousePosition);
    }
}