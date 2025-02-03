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
        private delegate object GetConfigDelegate(Type type);
        
        private Dictionary<Type, object> _ParsedConfigs = new ();
        
        private string _QAConfigJson;
        
        private string _ProdConfigJson;

        private bool _IsProd;
        
        private GetConfigDelegate _ResolveGetConfig;

        public ConfigurationManager(bool isProd, bool loadFromResources, Func<bool, string> getConfigPath)
        {
            _IsProd = isProd;
            
            _QAConfigJson = loadFromResources
                ? ReadConfigResources(getConfigPath(false))
                : ReadConfig(getConfigPath(false));

            if (_IsProd) 
            {
                _ProdConfigJson = loadFromResources
                    ? ReadConfigResources(getConfigPath(true))
                    : ReadConfig(getConfigPath(true));
                _ResolveGetConfig = ResolveGetConfigProd;
            }
            else 
            {
                _ResolveGetConfig = ResolveGetConfigQA;
            }
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

            return _ResolveGetConfig(type);
        }

        private object ResolveGetConfigQA(Type type) 
        {
            var deserObjQa = DeserializeConfig(_QAConfigJson, type);
            
            _ParsedConfigs[type] = deserObjQa;
                
            PrintConfigDebug(_ParsedConfigs[type]);
            
            return _ParsedConfigs[type];
        }
        
        private object ResolveGetConfigProd(Type type) 
        {
            var deserObjQa = DeserializeConfig(_QAConfigJson, type);
            var deserObjProd = DeserializeConfig(_ProdConfigJson, type); 
                
            _ParsedConfigs[type] = deserObjProd;
                
            PrintConfigDebug(_ParsedConfigs[type]);

            if (!VerifyConfigValuesConstraints(deserObjQa, deserObjProd)) 
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Prod config failed value constraints check");
            }
            
            return _ParsedConfigs[type];
        }

        private object DeserializeConfig(string path, Type type) 
        {
            var deserObj = JsonConvert.DeserializeObject(path, type);

            if (deserObj == null)
            {
                throw new UnityEditor.Build.BuildFailedException($"{BuildProcessor.TAG} Could not deserialize JSON to type {type} at path: {path}");
            }

            return deserObj;
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