using UnityEngine;

using LevelLoading.ScriptableObjects;

namespace LevelLoading
{
    public class PassageManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

        public static PassageManager instance;

        public SceneHandle sceneHandle;

        public PassageController[] passages;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <Description> Custom Methods </Description>

        public void LoadLevel(PassageController controller)
        {
            if (controller == null) return;

            PassageHandle passageHandle = sceneHandle.passages[controller.id];

            if (passageHandle == null) return;

            LevelLoader.instance.LoadLevel(passageHandle);
        }
    }
}