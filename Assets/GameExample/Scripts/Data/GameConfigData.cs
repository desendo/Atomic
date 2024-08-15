using System;

namespace GameExample.Scripts.Data
{
    [Serializable]
    public class GameConfigData : IData
    {
        public GeneralConfig GeneralConfig;
        public HealthConfig[] HealthConfigs;
    }

    [Serializable]
    public abstract class ConfigElementBase : IConfigElement
    {
        public string Id;
        string IConfigElement.Id => Id;
    }
    public interface IConfigElement
    {
        string Id { get; }
    }
    [Serializable]
    public class GeneralConfig
    {
        public float BorderScrollSpeed;
        public float CameraRotateSpeed;
        public float DragScrollSpeed;
    }

    [Serializable]
    public class HealthConfig : ConfigElementBase
    {
        public float Max;
    }

}