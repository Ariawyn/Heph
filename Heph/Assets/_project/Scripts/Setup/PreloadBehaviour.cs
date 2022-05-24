using Heph.Scripts.Managers.Level;
using UnityEngine;

// GENERALLY PRELOAD SCENES ARE FINE, AND ARE HELPFUL TO STORE SINGLETONS OR MANAGERS WE NEED TO HAVE PERSIST WITHIN MANY SCENES IN THE GAME.
// THERE ARE PROBABLY BETTER WAYS TO DO THAT (WITH INJECTABLE DEPENDANCIES, OR [RuntimeInitializeOnLoadMethod] FOR SPECIFIC PREFABS TO HAVE THEM EXIST IN EACH SCENE)
// BASICALLY I JUST HAVE TO LOOK INTO IT, BUT PRELOAD SCENES ARE FINE.
// TODO: MAYBE LOOK INTO CHANGING HOW WE HANDLE PERSISTENCE BETWEEN SCENES WITHOUT PRELOAD SCENE STUFF. LIKE https://low-scope.com/unity-tips-1-dont-use-your-first-scene-for-global-script-initialization/
namespace Heph.Scripts.Preload
{
    public class PreloadBehaviour : MonoBehaviour
    {
        private LevelManager _levelManager;

        // Start is called before the first frame update
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            this._levelManager = Object.FindObjectOfType<LevelManager>();

#if UNITY_EDITOR

            // OKAY SO THIS IS NOT IDEAL. IN FACT ITS PROBABLY VERY UNIDEAL. IM USING PLAYERPREFS FOR DEV BEHAVIOUR SIMPLY TO MAKE SURE WE DONT HAVE TO PLAY THROUGH MANY LEVELS
            // OR USE A COMMAND TO JUMP TO THE LEVEL WE WANT TO TEST AND ALWAYS GO THROUGH THE PRELOAD STUFF MANUALLY
            // THIS IS VERY DUMB, BUT IT SAVES US TIME IN DEV, WE NEED TO GET RID OF THIS FOR LAUNCH
            // TODO: GET RID OF THIS (OR JUST FIND A BETTER WAY TO HANDLE STUFF WITHOUT PRELOAD (WHICH I KNOW EXISTS))).
            var sceneToStartIn = PlayerPrefs.GetInt("dev_editor_start_scene_index");

            if(sceneToStartIn > 0)
            {
                this._levelManager.LoadLevelFromInt(sceneToStartIn);
            }

#else

        this.level_manager.LoadLevel("TestScene");

#endif
        }
    }
}