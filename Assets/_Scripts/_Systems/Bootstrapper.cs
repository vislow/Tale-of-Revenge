using UnityEngine;

namespace Root.GameManagement.Preload
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            GameObject systems = Object.Instantiate(Resources.Load("Systems")) as GameObject;

            if (systems != null) return;

            throw new System.ApplicationException();
        }
    }
}