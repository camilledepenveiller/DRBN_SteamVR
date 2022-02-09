using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart_Level : MonoBehaviour
{
    //public Langevin_dial langevin_Dial;
    public GameObject Player;

    public void Restart()
    {
        //Debug.Log("restart function called");
        //Debug.Log("reboot_switch state " + reboot_switch);
        //        Application.LoadLevel(Application.loadedLevel);            //old function
        //langevin_Dial.Temp = 0;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("molworkshop");

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }


    //public void Restart_Keep_Loc()
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    //    Player.transform.SetPositionAndRotation(new Vector3(0, 0, 500), new Quaternion(0, 0, 0, 0));
    //}
}
