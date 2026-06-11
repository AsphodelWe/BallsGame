using UnityEngine;

public class PrefsManager : MonoBehaviour
{
    private void Awake()
    {

        if (!PlayerPrefs.HasKey("FirstLaunch_v3"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("FirstLaunch_v3", 1);
            PlayerPrefs.Save();
        }

        //PlayerPrefs.DeleteAll();
    }
}
