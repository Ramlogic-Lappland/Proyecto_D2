using System;

namespace Gaskellgames
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>

    [Serializable]
    public class GgEventArgs_PrefabInstance : GgEventArgs
    {
        public int instanceID;
        public SceneData sceneData;
        
        public GgEventArgs_PrefabInstance(int instanceID, SceneData sceneData)
        {
            this.instanceID = instanceID;
            this.sceneData = sceneData;
        }

    } // class end
}