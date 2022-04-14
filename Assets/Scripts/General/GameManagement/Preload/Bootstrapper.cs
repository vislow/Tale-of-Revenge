using UnityEngine;
using GameManagement.Preload;

/// <Description> 
// This is the games Bootstrapper 
// The bootstrapper is first thing to run on game start up
// It initializes the App object (located in the resources folder)
// which holds all of the games persistant objects and scripts
// </Description>
public static class Bootstrapper
{
    // This tag makes the bootstrapper code run on startup 
    // without requiring the script actually be in the scene
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (App.instance != null) return;

        GameObject app = Object.Instantiate(Resources.Load("_App")) as GameObject;

        if (app != null) return;

        throw new System.ApplicationException();
    }
}
