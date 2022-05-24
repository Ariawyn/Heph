using UnityEngine;
using UnityEngine.SceneManagement;

// THIS IS A DUMB LITTLE CLASS MADE JUST TO SAVE ON EFFORT OF TESTING DIFFERENT SCENES WITHOUT MANUALLY OR HAVING TO DO SOMETHING TO JUMP
// FROM THE PRELOAD SCENE BACK TO THE ONE WE NEED TO TEST. NOT IDEAL, BUT HEY IT WORKS. OBVIOUSLY WE WANT TO GET RID OF THIS BEFORE WE SUBMIT
// THE GAME SOMEWHERE, OR WHEN WE FIND A BETTER SOLUTION (WHICH DEFINITELY EXISTS).
namespace Heph.Scripts.Preload
{
    public class EditorPreloadIntegration : MonoBehaviour
    {
        #if UNITY_EDITOR

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static void EnsureInitPreloadScene()
            {

                var startSceneIndex = SceneManager.GetActiveScene().buildIndex;

                if(startSceneIndex == 0)
                {
                    return;
                }

                // OKAY SO THIS IS NOT IDEAL. IN FACT ITS PROBABLY VERY UNIDEAL. IM USING PLAYERPREFS FOR DEV BEHAVIOUR SIMPLY TO MAKE SURE WE DONT HAVE TO PLAY THROUGH MANY LEVELS
                // OR USE A COMMAND TO JUMP TO THE LEVEL WE WANT TO TEST AND ALWAYS GO THROUGH THE PRELOAD STUFF MANUALLY
                // THIS IS VERY DUMB, BUT IT SAVES US TIME IN DEV, WE NEED TO GET RID OF THIS FOR LAUNCH
                // TODO: GET RID OF THIS (OR JUST FIND A BETTER WAY TO HANDLE STUFF WITHOUT PRELOAD (WHICH I KNOW EXISTS))).
                PlayerPrefs.SetInt("dev_editor_start_scene_index", startSceneIndex);
                PlayerPrefs.Save();

                SceneManager.LoadScene(0);
            }

        #endif
    }
}