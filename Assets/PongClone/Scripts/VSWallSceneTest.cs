using UnityEngine;

namespace PongClone
{
    public class VSWallSceneTest : MonoBehaviour
    {
        public WallGamePlay gameplay;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                gameplay.ManualStart();
            }
        }
    }
}