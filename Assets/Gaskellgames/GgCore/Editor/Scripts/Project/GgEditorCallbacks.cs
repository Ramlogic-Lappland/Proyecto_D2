#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gaskellgames.EditorOnly
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    [InitializeOnLoad]
    public class GgEditorCallbacks : AssetPostprocessor, ISerializationCallbackReceiver
    {
        #region Variables: AfterInitializeOnLoad

        /// <summary>
        /// Called the frame after InitializeOnLoad takes place.
        /// </summary>
        public static event Action OnSafeInitialize;
        
        #endregion
        
        #region Variables: AssemblyReload

        /// <summary>
        /// Called before AssemblyReload takes place.
        /// </summary>
        public static event Action OnBeforeAssemblyReload;

        /// <summary>
        /// Called after AssemblyReload takes place.
        /// </summary>
        public static event Action OnAfterAssemblyReload;

        #endregion
        
        #region Variables: Serialization

        /// <summary>
        /// Called before serialization takes place.
        /// </summary>
        public static event Action OnBeforeSerialization;

        /// <summary>
        /// Called after serialization takes place.
        /// </summary>
        public static event Action OnAfterSerialization;

        #endregion
        
        #region Variables: AssetPostprocessor

        /// <summary>
        /// Called when an asset has been deleted. [Passes asset as <see cref="UnityEngine.Object"/>]
        /// </summary>
        public static event OnAssetImportedDelegate OnAssetImported;
        public delegate void OnAssetImportedDelegate(Object asset);

        /// <summary>
        /// Called when an asset has been deleted. [Passes asset path as <see cref="string"/>]
        /// </summary>
        public static event OnAssetDeletedDelegate OnAssetDeleted;
        public delegate void OnAssetDeletedDelegate(string assetFilePath);

        /// <summary>
        /// Called when an asset has been moved. [Passes asset as <see cref="UnityEngine.Object"/> along with new asset path and old asset path as strings]
        /// </summary>
        public static event OnAssetMovedDelegate OnAssetMoved;
        public delegate void OnAssetMovedDelegate(Object asset, string newAssetFilePath, string oldAssetFilePath);

        #endregion
        
        #region Variables: ObjectChangeEvents

        /// <summary>
        /// Called when an open scene has been changed 'dirtied' without any more specific information available.
        /// </summary>
        public static event OnSceneUpdatedDelegate OnSceneUpdated;
        public delegate void OnSceneUpdatedDelegate(GgEventArgs_SceneData args0);

        /// <summary>
        /// Called when a GameObject has been created, possibly with additional objects below it in the hierarchy.
        /// </summary>
        public static event OnGameObjectCreatedDelegate OnGameObjectCreated;
        public delegate void OnGameObjectCreatedDelegate(GgEventArgs_GameObject args0);

        /// <summary>
        /// Called when a GameObject has been destroyed, including any parented to it in the hierarchy.
        /// </summary>
        public static event OnGameObjectDestroyedDelegate OnGameObjectDestroyed;
        public delegate void OnGameObjectDestroyedDelegate(GgEventArgs_GameObjectDestroyed args0);

        /// <summary>
        /// Called when the structure of a GameObject has changed and any GameObject in the hierarchy below it might have changed.
        /// </summary>
        public static event OnGameObjectHierarchyUpdatedDelegate OnGameObjectHierarchyUpdated;
        public delegate void OnGameObjectHierarchyUpdatedDelegate(GgEventArgs_GameObject args0);

        /// <summary>
        /// Called when the structure of a GameObject has changed.
        /// </summary>
        public static event OnGameObjectStructureUpdatedDelegate OnGameObjectStructureUpdated;
        public delegate void OnGameObjectStructureUpdatedDelegate(GgEventArgs_GameObject args0);

        /// <summary>
        /// Called when the parent or parent scene of a GameObject has changed.
        /// </summary>
        public static event OnGameObjectParentUpdatedDelegate OnGameObjectParentUpdated;
        public delegate void OnGameObjectParentUpdatedDelegate(GgEventArgs_GameObjectChangedParent args0);

        /// <summary>
        /// Called when the properties of a GameObject has changed.
        /// </summary>
        public static event OnGameObjectPropertiesUpdatedDelegate OnGameObjectPropertiesUpdated;
        public delegate void OnGameObjectPropertiesUpdatedDelegate(GgEventArgs_GameObject args0);

        /// <summary>
        /// Called when the properties of a Component has changed.
        /// </summary>
        public static event OnComponentPropertiesUpdatedDelegate OnComponentPropertiesUpdated;
        public delegate void OnComponentPropertiesUpdatedDelegate(GgEventArgs_Component args0);

        /// <summary>
        /// Called when an asset object has been created.
        /// </summary>
        public static event OnAssetObjectCreatedDelegate OnAssetObjectCreated;
        public delegate void OnAssetObjectCreatedDelegate(GgEventArgs_AssetObject args0);

        /// <summary>
        /// Called when an asset object has been destroyed.
        /// </summary>
        public static event OnAssetObjectDestroyedDelegate OnAssetObjectDestroyed;
        public delegate void OnAssetObjectDestroyedDelegate(GgEventArgs_AssetObjectDestroyed args0);

        /// <summary>
        /// Called when a property of an asset object in memory has changed.
        /// </summary>
        public static event OnAssetObjectPropertiesUpdatedDelegate OnAssetObjectPropertiesUpdated;
        public delegate void OnAssetObjectPropertiesUpdatedDelegate(GgEventArgs_AssetObject args0);

        /// <summary>
        /// Called when a prefab instance in an open scene has been updated due to a change to the source prefab.
        /// </summary>
        public static event OnPrefabInstanceUpdatedDelegate OnPrefabInstanceUpdated;
        public delegate void OnPrefabInstanceUpdatedDelegate(GgEventArgs_PrefabInstance args0);

        #endregion
        
        #region Variables: PlaymodeStateChange
        
        /// <summary>
        /// Called when the editor changes playmode state to EditMode.
        /// </summary>
        public static event Action OnEnteredEditMode;
        
        /// <summary>
        /// Called when the editor changes playmode state to ExitingEditMode.
        /// </summary>
        public static event Action OnExitingEditMode;
        
        /// <summary>
        /// Called when the editor changes playmode state to PlayMode.
        /// </summary>
        public static event Action OnEnteredPlayMode;
        
        /// <summary>
        /// Called when the editor changes playmode state to ExitingPlayMode.
        /// </summary>
        public static event Action OnExitingPlayMode;

        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region Debug Logs

        private static void DebugLog(string format, params object[] args)
        {
            //Debug.LogFormat(format, args);
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region Constructor

        static GgEditorCallbacks()
        {
            DebugLog("OnInitializeOnLoad");
            
            AssemblyReloadEvents.beforeAssemblyReload -= AssemblyReloadEvents_BeforeAssemblyReload;
            AssemblyReloadEvents.beforeAssemblyReload += AssemblyReloadEvents_BeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= AssemblyReloadEvents_AfterAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += AssemblyReloadEvents_AfterAssemblyReload;
            
            EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
            
            ObjectChangeEvents.changesPublished -= ObjectChangeEvents_ChangesPublished;
            ObjectChangeEvents.changesPublished += ObjectChangeEvents_ChangesPublished;

            HandleOnSafeInitialize();
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region Private Functions: AfterInitializeOnLoad

        private static async void HandleOnSafeInitialize()
        {
            await TaskExtensions.WaitUntilNextFrame();
            OnSafeInitialize?.Invoke();
            DebugLog("OnSafeInitialize");
        }

        #endregion
        
        #region Private Functions: AssemblyReload

        private static void AssemblyReloadEvents_BeforeAssemblyReload()
        {
            OnBeforeAssemblyReload?.Invoke();
            DebugLog("OnBeforeAssemblyReload");
        }

        private static void AssemblyReloadEvents_AfterAssemblyReload()
        {
            OnAfterAssemblyReload?.Invoke();
            DebugLog("OnAfterAssemblyReload");
        }

        #endregion
        
        #region Private Functions: Serialization

        public void OnBeforeSerialize()
        {
            OnBeforeSerialization?.Invoke();
            DebugLog("OnBeforeSerialization");
        }

        public void OnAfterDeserialize()
        {
            OnAfterSerialization?.Invoke();
            DebugLog("OnAfterSerialization");
        }

        #endregion
        
        #region Private Functions: AssetPostprocessor

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            HandleImportedAssets(importedAssets);
            HandleDeletedAssets(deletedAssets);
            HandleMovedAssets(movedAssets, movedFromAssetPaths);
        }
        
        private static string FixStringSeparators(string path)
        {
            return path.Replace("\\", "/");
        }

        private static void HandleImportedAssets(string[] importedAssets)
        {
            foreach (string assetPath in importedAssets)
            {
                string filepath = FixStringSeparators(Path.GetDirectoryName(assetPath));
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(filepath);
                if (asset == null) { continue; }
                OnAssetImported?.Invoke(asset);
                DebugLog("OnAssetImported: {0} at filepath {1}", asset.name, filepath);
            }
        }

        private static void HandleDeletedAssets(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                string filepath = FixStringSeparators(Path.GetDirectoryName(assetPath));
                OnAssetDeleted?.Invoke(filepath);
                DebugLog("OnAssetDeleted: {0}", filepath);
            }
        }

        private static void HandleMovedAssets(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0; i < movedAssets.Length; i++)
            {
                string newFilePath = FixStringSeparators(Path.GetDirectoryName(movedAssets[i]));
                string oldFilePath = FixStringSeparators(Path.GetDirectoryName(movedFromAssetPaths[i]));
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(newFilePath);
                OnAssetMoved?.Invoke(asset, newFilePath, oldFilePath);
                DebugLog("OnAssetMoved: {0} at filepath {1} from filepath {2}", asset.name, newFilePath, oldFilePath);
            }
        }

        #endregion
        
        #region Private Functions: ObjectChangeEvents

        private static void ObjectChangeEvents_ChangesPublished(ref ObjectChangeEventStream stream)
        {
            for (int i = 0; i < stream.length; ++i)
            {
                ObjectChangeKind type = stream.GetEventType(i);
                switch (type)
                {
                    case ObjectChangeKind.ChangeScene:
                        stream.GetChangeSceneEvent(i, out ChangeSceneEventArgs changeSceneEvent);
                        OnSceneUpdated?.Invoke(new GgEventArgs_SceneData(changeSceneEvent.scene));
                        DebugLog("OnSceneUpdated: {0}", changeSceneEvent.scene.name);
                        break;

                    case ObjectChangeKind.CreateGameObjectHierarchy:
                        stream.GetCreateGameObjectHierarchyEvent(i, out CreateGameObjectHierarchyEventArgs createGameObjectHierarchyEvent);
                        GameObject newGameObject = EditorUtility.InstanceIDToObject(createGameObjectHierarchyEvent.instanceId) as GameObject;
                        OnGameObjectCreated?.Invoke(new GgEventArgs_GameObject(newGameObject));
                        DebugLog("OnGameObjectCreated: {0}", newGameObject.name);
                        break;

                    case ObjectChangeKind.DestroyGameObjectHierarchy:
                        stream.GetDestroyGameObjectHierarchyEvent(i, out DestroyGameObjectHierarchyEventArgs destroyGameObjectHierarchyEvent);
                        GameObject destroyParentGo = EditorUtility.InstanceIDToObject(destroyGameObjectHierarchyEvent.parentInstanceId) as GameObject;
                        OnGameObjectDestroyed?.Invoke(new GgEventArgs_GameObjectDestroyed(destroyGameObjectHierarchyEvent.instanceId, destroyParentGo, new SceneData(destroyGameObjectHierarchyEvent.scene)));
                        DebugLog("OnGameObjectDestroyed: {0} in scene {1} with parent {2}", destroyGameObjectHierarchyEvent.instanceId, destroyGameObjectHierarchyEvent.scene.name, destroyParentGo ? destroyParentGo.name : "Root");
                        break;

                    case ObjectChangeKind.ChangeGameObjectStructureHierarchy:
                        stream.GetChangeGameObjectStructureHierarchyEvent(i, out ChangeGameObjectStructureHierarchyEventArgs changeGameObjectStructureHierarchy);
                        GameObject gameObject = EditorUtility.InstanceIDToObject(changeGameObjectStructureHierarchy.instanceId) as GameObject;
                        OnGameObjectHierarchyUpdated?.Invoke(new GgEventArgs_GameObject(gameObject));
                        DebugLog("OnGameObjectHierarchyUpdated: {0}", gameObject.name);
                        break;

                    case ObjectChangeKind.ChangeGameObjectStructure:
                        stream.GetChangeGameObjectStructureEvent(i, out ChangeGameObjectStructureEventArgs changeGameObjectStructure);
                        GameObject gameObjectStructure = EditorUtility.InstanceIDToObject(changeGameObjectStructure.instanceId) as GameObject;
                        OnGameObjectStructureUpdated?.Invoke(new GgEventArgs_GameObject(gameObjectStructure));
                        DebugLog("OnGameObjectStructureUpdated: {0}", gameObjectStructure.name);
                        break;

                    case ObjectChangeKind.ChangeGameObjectParent:
                        stream.GetChangeGameObjectParentEvent(i, out ChangeGameObjectParentEventArgs changeGameObjectParent);
                        GameObject gameObjectChanged = EditorUtility.InstanceIDToObject(changeGameObjectParent.instanceId) as GameObject;
                        GameObject newParentGo = EditorUtility.InstanceIDToObject(changeGameObjectParent.newParentInstanceId) as GameObject;
                        GameObject previousParentGo = EditorUtility.InstanceIDToObject(changeGameObjectParent.previousParentInstanceId) as GameObject;
                        OnGameObjectParentUpdated?.Invoke(new GgEventArgs_GameObjectChangedParent(gameObjectChanged, previousParentGo, newParentGo, new SceneData(changeGameObjectParent.previousScene), new SceneData(changeGameObjectParent.newScene)));
                        DebugLog("OnGameObjectParentUpdated: {0} parented to {1} in scene {2} from {3} in scene {4}", gameObjectChanged ? gameObjectChanged.name : "Null", newParentGo ? newParentGo.name : "Root", changeGameObjectParent.previousScene.IsValid() ? changeGameObjectParent.previousScene.name : "Null", previousParentGo ? previousParentGo.name : "Root", changeGameObjectParent.newScene.IsValid() ? changeGameObjectParent.newScene.name : "Null");
                        break;

                    case ObjectChangeKind.ChangeGameObjectOrComponentProperties:
                        stream.GetChangeGameObjectOrComponentPropertiesEvent(i, out ChangeGameObjectOrComponentPropertiesEventArgs changeGameObjectOrComponent);
                        Object goOrComponent = EditorUtility.InstanceIDToObject(changeGameObjectOrComponent.instanceId);
                        switch (goOrComponent)
                        {
                            case GameObject go:
                                OnGameObjectPropertiesUpdated?.Invoke(new GgEventArgs_GameObject(go));
                                DebugLog("OnGameObjectPropertiesUpdated: {0}", go.name);
                                break;
                            case Component component:
                                OnComponentPropertiesUpdated?.Invoke(new GgEventArgs_Component(component));
                                DebugLog("OnGameObjectPropertiesUpdated: {0} on {1}", component.name, component.gameObject.name);
                                break;
                        }
                        break;

                    case ObjectChangeKind.CreateAssetObject:
                        stream.GetCreateAssetObjectEvent(i, out CreateAssetObjectEventArgs createAssetObjectEvent);
                        Object createdAsset = EditorUtility.InstanceIDToObject(createAssetObjectEvent.instanceId);
                        string createdAssetPath = AssetDatabase.GUIDToAssetPath(createAssetObjectEvent.guid);
                        OnAssetObjectCreated?.Invoke(new GgEventArgs_AssetObject(createdAsset, createdAssetPath));
                        DebugLog("OnAssetObjectCreated: {0}", createdAsset.name);
                        break;

                    case ObjectChangeKind.DestroyAssetObject:
                        stream.GetDestroyAssetObjectEvent(i, out DestroyAssetObjectEventArgs destroyAssetObjectEvent);
                        OnAssetObjectDestroyed?.Invoke(new GgEventArgs_AssetObjectDestroyed(destroyAssetObjectEvent.instanceId, destroyAssetObjectEvent.guid));
                        DebugLog("OnAssetObjectDestroyed: {0}", destroyAssetObjectEvent.instanceId);
                        break;

                    case ObjectChangeKind.ChangeAssetObjectProperties:
                        stream.GetChangeAssetObjectPropertiesEvent(i, out ChangeAssetObjectPropertiesEventArgs changeAssetObjectPropertiesEvent);
                        Object changeAsset = EditorUtility.InstanceIDToObject(changeAssetObjectPropertiesEvent.instanceId);
                        string changeAssetPath = AssetDatabase.GUIDToAssetPath(changeAssetObjectPropertiesEvent.guid);
                        OnAssetObjectPropertiesUpdated?.Invoke(new GgEventArgs_AssetObject(changeAsset, changeAssetPath));
                        DebugLog("OnAssetObjectPropertiesUpdated: {0}", changeAsset);
                        break;

                    case ObjectChangeKind.UpdatePrefabInstances:
                        stream.GetUpdatePrefabInstancesEvent(i, out UpdatePrefabInstancesEventArgs updatePrefabInstancesEvent);
                        foreach (int instanceID in updatePrefabInstancesEvent.instanceIds)
                        {
                            OnPrefabInstanceUpdated?.Invoke(new GgEventArgs_PrefabInstance(instanceID, new SceneData(updatePrefabInstancesEvent.scene)));
                            DebugLog("OnPrefabInstanceUpdated: {0} in scene: {1}", instanceID, updatePrefabInstancesEvent.scene.name);
                        }
                        break;
                }
            }
        }
        
        #endregion
        
        #region Private Functions: PlayModeStateChanged

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnEnteredEditMode?.Invoke();
                    DebugLog("OnEnteredEditMode");
                    break;
                
                case PlayModeStateChange.ExitingEditMode:
                    OnExitingEditMode?.Invoke();
                    DebugLog("OnExitingEditMode");
                    break;
                
                case PlayModeStateChange.EnteredPlayMode:
                    OnEnteredPlayMode?.Invoke();
                    DebugLog("OnEnteredPlayMode");
                    break;
                
                case PlayModeStateChange.ExitingPlayMode:
                    OnExitingPlayMode?.Invoke();
                    DebugLog("OnExitingPlayMode");
                    break;
            }
        }
        
        #endregion
        
    } // class end
}

#endif