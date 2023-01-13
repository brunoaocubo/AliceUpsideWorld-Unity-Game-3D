using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject TelaInicial;
    [SerializeField] private GameObject TelaConfig;
    [SerializeField] private GameObject TelaCreditos;
    [SerializeField] private Image loadingFill;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void SceneGame(int sceneID) 
    {
        StartCoroutine(LoadingScreen(sceneID));
    }

    public void MenuConfig() 
    {
        TelaInicial.SetActive(false);
        TelaConfig.SetActive(true);
        TelaCreditos.SetActive(false);
    }

    public void MenuCréditos()
    {
        TelaInicial.SetActive(false);
        TelaConfig.SetActive(false);
        TelaCreditos.SetActive(true);
    }

    public void ButtonReturn() 
    {
        TelaInicial.SetActive(true);
        TelaConfig.SetActive(false);
        TelaCreditos.SetActive(false);
    }

    IEnumerator LoadingScreen(int sceneID) 
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        loadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            float progressOperation = Mathf.Clamp01(operation.progress / 0.9f);
            loadingFill.fillAmount = progressOperation;

            yield return null;
        } 
    }
}
