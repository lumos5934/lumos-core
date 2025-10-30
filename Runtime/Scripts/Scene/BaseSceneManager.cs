using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public abstract class BaseSceneManager : MonoBehaviour
    {
        #region --------------------------------------------------- PROPERTIES




        #endregion

        #region --------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            StartCoroutine(InitAsync());
        }

        protected virtual void OnDestroy()
        {
            Global.Unregister(this);
        }


        #endregion

        #region --------------------------------------------------- INIT


        private IEnumerator InitAsync() 
        {
            yield return new WaitUntil(() => Bootstrap.IsInitialized);

            Global.Register(this);
        }


        #endregion
    }
}