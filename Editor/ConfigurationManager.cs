using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;
using UnityEngine;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    public class ConfigurationManager
    {
        private Dictionary<Type, object> _ParsedConfigs = new ();
        
        private string _QAConfigJson;
        
        private string _ProdConfigJson;

        private bool _IsProd;

        public ConfigurationManager(bool isProd, bool loadFromResources, Func<bool, string> getConfigPath)
        {
            _IsProd = isProd;
            _QAConfigJson = loadFromResources
                ? ReadConfigResources(getConfigPath(false))
                : ReadConfig(getConfigPath(false));
            _ProdConfigJson = loadFromResources
                ? ReadConfigResources(getConfigPath(true))
                : ReadConfig(getConfigPath(true));
        }

        private string ReadConfig(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} GetBuildConfig configPath is NULL or empty");
            }

            if (!File.Exists(configPath))
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Config json does not exist at path: {configPath}");
            }
            
            return File.ReadAllText(configPath);
        }
        
        protected string ReadConfigResources(string resourcesPath)
        {
            return Resources.Load(resourcesPath)?.ToString();
        }
        
        public T GetConfig<T>() where T : class
        {
            return GetConfig(typeof(T)) as T;
        }
        
        public object GetConfig(Type type)
        {
            
            if (_ParsedConfigs.TryGetValue(type, out var config))
                return config;
            
            var deserObjProd = JsonConvert.DeserializeObject(_ProdConfigJson, type);
            var deserObjQa = JsonConvert.DeserializeObject(_QAConfigJson, type);
            
            if (deserObjProd == null)
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Could not deserialize JSON to type {type} at path: {_ProdConfigJson}");
            }
            
            if (deserObjQa == null)
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Could not deserialize JSON to type {type} at path: {_QAConfigJson}");
            }
            
            if (_IsProd)
            {
                _ParsedConfigs[type] = deserObjProd;
            }
            else
            {
                _ParsedConfigs[type] = deserObjQa;
            }
            
            PrintConfigDebug(_ParsedConfigs[type]);
            
            if (_IsProd && !VerifyConfigValuesConstraints(deserObjQa, deserObjProd))
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Prod config failed value constraints check");
            }
            
            return _ParsedConfigs[type];
        }

        private void PrintConfigDebug(object config)
        {
            var qaConfigType = config.GetType();
            var fields = qaConfigType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<DebugConfigFieldAttribute>() != null)
                {
                    Debug.Log($"{BuildProcessor.TAG} Using '{field.Name}' with value: '{field.GetValue(config)}'");
                }
            }
        }

        private bool VerifyConfigValuesConstraints(object qaConfig, object prodConfig)
        {
            var isValid = true;

            var qaConfigType = qaConfig.GetType();
            var prodConfigType = prodConfig.GetType();

            if (qaConfigType != prodConfigType)
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Both config should be of the same type but found: {qaConfigType} and {prodConfigType}");
            }

            var fields = qaConfigType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<EnsureNotPlaceholderAttribute>() != null)
                {
                    if (field.GetValue(qaConfig) == null) 
                    {
                        Debug.LogWarning($"{BuildProcessor.TAG} QA config is missing field: {field.Name}");
                        isValid = false;
                        continue;
                    }
                    
                    if (field.GetValue(prodConfig) == null) 
                    {
                        Debug.LogWarning($"{BuildProcessor.TAG} PROD config is missing field: {field.Name}");
                        isValid = false;
                        continue;
                    }
                    
                    var qaValue = field.GetValue(qaConfig).ToString();
                    var prodValue = field.GetValue(prodConfig).ToString();

                    if (String.CompareOrdinal(qaValue, prodValue) == 0)
                    {
                        Debug.LogWarning($"Field value not updated in prod config for field: {field.Name} found: {qaValue}");
                        isValid = false;
                    }
                }
            }
            
            return isValid;
        }
    }
}