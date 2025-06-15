#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings;


namespace KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager
{
    public class LogSettingsManager : MonoSingleton<LogSettingsManager>
    {
        [SerializeField] private List<LogTypeSettingsSO> logTypeAssets;
        public List<LogTypeSettingsSO> LogTypeAssets => logTypeAssets;
        
        private Dictionary<AppLogType, LogTypeSettingsSO> settingsMap;

        [SerializeField] private bool writeLogsToFile = true;
        public bool shouldSaveLogsOnExit => writeLogsToFile;

        protected override void Init()
        {
            BuildSettingsMap();
        }

        private void BuildSettingsMap()
        {
            settingsMap = logTypeAssets.ToDictionary(asset => asset.type);
        }

        public LogTypeSettingsSO GetSettingsForType(AppLogType type)
        {
            if (settingsMap == null || !settingsMap.TryGetValue(type, out var setting))
            {
                Debug.LogWarning($"[LogSettingsManager] Missing settings for log Type {type}.");
                return null;
            }

            return setting;
        }

        public IEnumerable<AppLogType> GetLogTypesInAssets()
        {
            return logTypeAssets != null
                ? logTypeAssets.Where(x => x != null).Select(x => x.type)
                : Enumerable.Empty<AppLogType>();
        }
    }
}




   