using System;
using UnityEngine;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>

    [Serializable]
    public class GgEventArgs_GameObjectDestroyed : GgEventArgs
    {
        public int instanceID;
        public GameObject parent;
        public SceneData sceneData;
        
        public GgEventArgs_GameObjectDestroyed(int instanceID, GameObject parent, SceneData sceneData)
        {
            this.instanceID = instanceID;
            this.parent = parent;
            this.sceneData = sceneData;
        }

    } // class end
}