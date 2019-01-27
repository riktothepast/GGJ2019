using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private int sceneToLoadIndex;

    public void Load()
    {
        SceneManager.LoadScene(sceneToLoadIndex);
    }
}
