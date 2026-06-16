using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Static field holding the singleton instance
    // Credits to this for the thread-safe singleton pattern implementation:
    // https://dev.to/devsdaddy/everything-you-need-to-know-about-singleton-in-c-and-unity-n40
    public static SoundManager Instance => Nested.Source;
    private static class Nested
    {
        static Nested(){}
        internal static readonly SoundManager Source = CreateSingleton();

        private static SoundManager CreateSingleton()
        {
            GameObject instance = Instantiate(
                (GameObject)Resources.Load(
                    "Managers/SoundManager",
                    typeof(GameObject))
            );

            DontDestroyOnLoad(instance);
            var manager = instance.GetComponent<SoundManager>();
            Debug.Assert(instance != null && manager != null);
            return manager;
        }
    }

    public AudioClip alert;
}
