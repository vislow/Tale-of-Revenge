using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameManagement.Preload
{
    public static class Initialization
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Bootstrap()
        {
            if (App.instance != null) return;

            var app = Object.Instantiate(Resources.Load("_App")) as GameObject;

            if (app == null)
                throw new ApplicationException();
        }
    }
}
