using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    private enum LoadingState
    {
        None,
        LoadingEnter,
        LoadingExit,
    }

    private LoadingState currentState;

    [Header("Material")]
    [SerializeField] private Material material;

    [Header("Seconds")]
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    private float fadeTimer;

    private const string redIDProperty = "Red ID";
    private int redID;

    private const string blueIDProperty = "Blue ID";
    private int blueID;

    private const string greenIDProperty = "Green ID";
    private int greenID;

    private const string alphaIDProperty = "Alpha ID";
    private int alphaID;

    private Coroutine materialCoroutine;

    private void Awake()
    {
        currentState = LoadingState.None;

        redID = Shader.PropertyToID(redIDProperty);
        blueID = Shader.PropertyToID(blueIDProperty);
        greenID = Shader.PropertyToID(greenIDProperty);
        alphaID = Shader.PropertyToID(alphaIDProperty);

        material.SetFloat(redID, 0);
        material.SetFloat(blueID, 0);
        material.SetFloat(greenID, 0);
        material.SetFloat(alphaID, 0);
    }

    /*private IEnumerator Start()
    {
        var mainCamera = Camera.main;
        var cameraMaterial = mainCamera.TryGetComponent<TestCameraMaterial>(out var cashe) ?
                             mainCamera.GetComponent<TestCameraMaterial>() : 
                             mainCamera.gameObject.AddComponent<TestCameraMaterial>();

        if (cameraMaterial != null)
        {
            cameraMaterial.material= material;

            while(true)
            {
                var nextAlpha = material.GetFloat(alphaIDProperty) + (0.3f * Time.deltaTime);

                if (nextAlpha <= 1.0f)
                    material.SetFloat(alphaIDProperty, nextAlpha);
                else
                    yield break;

                yield return null;
            }
        }

        yield return null;
    }*/

    private void Enter()
    {
        currentState = LoadingState.LoadingEnter;
    }

    private void Update()
    {
        
    }
}
