using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;   
using Unity.Entities.Serialization;
using Unity.Scenes;
using Unity.Entities;

public class SubScenePreloader : MonoBehaviour
{
    public List<EntitySceneReference> SubSceneReferences;
    
    [Header("Events")]
    public UnityEvent OnAllScenesLoaded;
    public UnityEvent<float> OnLoadingProgressUpdated;
    
    private int totalScenesToLoad;
    private int loadedScenesCount;
    private bool isLoading = false;
    private List<Entity> loadedSceneEntities = new List<Entity>();
    
    // C# events for external subscribers
    public event Action OnAllScenesLoadedEvent;
    public event Action<float> OnLoadingProgressUpdatedEvent;

    void Start()
    {
        StartLoadingScenes();
    }
    
    public void StartLoadingScenes()
    {
        if (isLoading) return;
        
        isLoading = true;
        loadedScenesCount = 0;
        totalScenesToLoad = SubSceneReferences.Count;
        loadedSceneEntities.Clear();
        
        if (totalScenesToLoad == 0)
        {
            OnAllScenesComplete();
            return;
        }
        
        StartCoroutine(LoadAllScenesAsync());
    }
    
    private IEnumerator LoadAllScenesAsync()
    {
        // Start loading all scenes using SceneSystem
        foreach (var subSceneReference in SubSceneReferences)
        {
            var sceneEntity = SceneSystem.LoadSceneAsync(
                World.DefaultGameObjectInjectionWorld.Unmanaged,
                subSceneReference,
                new SceneSystem.LoadParameters
                {
                    Flags = SceneLoadFlags.LoadAdditive | SceneLoadFlags.BlockOnStreamIn
                }
            );
            
            loadedSceneEntities.Add(sceneEntity);
        }
        
        // Wait for all scenes to complete loading
        while (loadedScenesCount < totalScenesToLoad)
        {
            int completedScenes = 0;
            
            // Check each scene loading status
            for (int i = 0; i < loadedSceneEntities.Count; i++)
            {
                var sceneEntity = loadedSceneEntities[i];
                
                if (SceneSystem.IsSceneLoaded(World.DefaultGameObjectInjectionWorld.Unmanaged, sceneEntity))
                {
                    completedScenes++;
                }
            }
            
            // Update loaded count
            if (completedScenes > loadedScenesCount)
            {
                loadedScenesCount = completedScenes;
                
                // Calculate progress (0-1)
                float progress = (float)loadedScenesCount / totalScenesToLoad;
                
                // Invoke progress events
                OnLoadingProgressUpdated?.Invoke(progress);
                OnLoadingProgressUpdatedEvent?.Invoke(progress);
            }
            
            yield return null;
        }
        
        // All scenes loaded
        OnAllScenesComplete();
    }
    
    private void OnAllScenesComplete()
    {
        isLoading = false;
        
        // Invoke completion events
        OnAllScenesLoaded?.Invoke();
        OnAllScenesLoadedEvent?.Invoke();
        
        Debug.Log($"All {totalScenesToLoad} subscenes loaded successfully!");
    }
    
    // Public method to check if loading is complete
    public bool IsLoadingComplete => !isLoading && loadedScenesCount >= totalScenesToLoad;
    
    // Public method to get current loading progress (0-1)
    public float GetLoadingProgress()
    {
        if (totalScenesToLoad == 0) return 1f;
        return (float)loadedScenesCount / totalScenesToLoad;
    }
    
    // Method to unload all loaded scenes
    public void UnloadAllScenes()
    {
        foreach (var sceneEntity in loadedSceneEntities)
        {
            if (SceneSystem.IsSceneLoaded(World.DefaultGameObjectInjectionWorld.Unmanaged, sceneEntity))
            {
                SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged, sceneEntity);
            }
        }
        
        loadedSceneEntities.Clear();
        loadedScenesCount = 0;
    }
}
