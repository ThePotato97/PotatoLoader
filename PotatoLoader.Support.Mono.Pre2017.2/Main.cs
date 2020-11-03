using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotatoLoader.Support
{
    internal static class Main
    {
        internal static bool IsDestroying = false;
        internal static GameObject obj = null;
        internal static PotatoLoaderComponent comp = null;
        internal static int CurrentScene = -9;
        private static ISupportModule Initialize()
        {
            PotatoLoaderComponent.Create();
            return new Module();
        }
    }

    public class PotatoLoaderComponent : MonoBehaviour
    {
        internal static readonly List<IEnumerator> QueuedCoroutines = new List<IEnumerator>();
        internal static void Create()
        {
            Main.obj = new GameObject("PotatoLoader");
            DontDestroyOnLoad(Main.obj);
            Main.comp = (PotatoLoaderComponent)Main.obj.AddComponent(typeof(PotatoLoaderComponent));
            Main.obj.transform.SetAsLastSibling();
            Main.comp.transform.SetAsLastSibling();
        }
        internal static void Destroy() { Main.IsDestroying = true; if (Main.obj != null) GameObject.Destroy(Main.obj); }
        void Awake()
        {
            foreach (var queuedCoroutine in QueuedCoroutines) StartCoroutine(queuedCoroutine);
            QueuedCoroutines.Clear();
        }
        void Start() => transform.SetAsLastSibling();
        void Update()
        {
            transform.SetAsLastSibling();
            if (Main.CurrentScene != Application.loadedLevel)
            {
                SceneHandler.OnSceneLoad(Application.loadedLevel);
                Main.CurrentScene = Application.loadedLevel;
            }
            MelonHandler.OnUpdate();
        }
        void FixedUpdate() => MelonHandler.OnFixedUpdate();
        void LateUpdate() => MelonHandler.OnLateUpdate();
        void OnGUI() => MelonHandler.OnGUI();
        void OnDestroy() { if (!Main.IsDestroying) Create(); }
        void OnApplicationQuit() { Destroy(); MelonHandler.OnApplicationQuit(); }
    }
}