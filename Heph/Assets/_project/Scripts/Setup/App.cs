using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heph
{
    public class App : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var app = Instantiate(Resources.Load("__app")) as GameObject;
            if(app == null) 
                throw new System.ApplicationException();
            
            DontDestroyOnLoad(app);
        }
    }
}
