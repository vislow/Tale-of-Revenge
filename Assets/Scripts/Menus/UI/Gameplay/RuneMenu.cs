using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay {
    public class RuneMenu : MonoBehaviour {
        public static RuneMenu instance;

        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject firstSelected;
        [SerializeField] private Animator anim;

        private void Awake() {
            if (instance = null)
                instance = this;
            else
                Destroy(this);
        }

        public void OpenMenu() {
            menu.SetActive(true);
            eventSystem.SetSelectedGameObject(firstSelected);
        }

        public void ActivateBearRune() {
            //PlayerRunes.instance.bearRune = true;
            anim.SetTrigger("Bear");
        }

        public void ActivateWolfRune() {
            //PlayerRunes.instance.wolfRune = true;
        }

        public void ActivateFaeryRune() {
            //PlayerRunes.instance.faeryRune = true;
        }
    }
}
