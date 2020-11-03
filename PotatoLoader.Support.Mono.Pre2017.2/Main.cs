using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelonLoader.Support
{
    internal static class Main
    {
        internal static bool IsDestroying = false;
        internal static GameObject obj = null;
        internal static MelonLoaderComponent comp = null;
        internal static int CurrentScene = -9;

        private static ISupportModule Initialize()
        {
            MelonLoaderComponent.Create();
            return new Module();
        }
    }

    public class MelonLoaderComponent : MonoBehaviour
    {
        internal static readonly List<IEnumerator> QueuedCoroutines = new List<IEnumerator>();

        internal static void Create()
        {
            Main.obj = new GameObject("PotatoLoader");
            DontDestroyOnLoad(Main.obj);
            Main.comp = (MelonLoaderComponent)Main.obj.AddComponent(typeof(MelonLoaderComponent));
            Main.obj.transform.SetAsLastSibling();
            Main.comp.transform.SetAsLastSibling();
        }

        internal static void Destroy()
        {
            Main.IsDestroying = true; if (Main.obj != null) GameObject.Destroy(Main.obj);
        }

        private void Awake()
        {
            foreach (var queuedCoroutine in QueuedCoroutines) StartCoroutine(queuedCoroutine);
            QueuedCoroutines.Clear();
        }

        private void Start() => transform.SetAsLastSibling();

        private void Update()
        {
            transform.SetAsLastSibling();
            if (Main.CurrentScene != Application.loadedLevel)
            {
                SceneHandler.OnSceneLoad(Application.loadedLevel);
                Main.CurrentScene = Application.loadedLevel;
            }
            MelonHandler.OnUpdate();
        }

        private void FixedUpdate() => MelonHandler.OnFixedUpdate();

        private void LateUpdate() => MelonHandler.OnLateUpdate();

        private void OnGUI() => MelonHandler.OnGUI();

        private void OnDestroy()
        { if (!Main.IsDestroying) Create(); }

        private void OnApplicationQuit()
        { Destroy(); MelonHandler.OnApplicationQuit(); }
    }
}