using System.IO;
using NUnit.Framework;
using UnityEditor.Build;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

namespace Plugins.UniversalBuildProcessor.Tests.Editor
{
    public class ConfigurationManagerTests
    {
        private const string RESOURCES_CONFIG_PATH = "TestConfigs";
        
        private const string QA_ANDROID_APP_NAME = "QA Android App";
        private const string QA_IOS_APP_NAME = "QA iOS App";
        private const string QA_APPSFLYER_APP_ID = "id123456789";
        
        private const string PROD_ANDROID_APP_NAME = "PROD Android App";
        private const string PROD_IOS_APP_NAME = "PROD iOS App";
        private const string PROD_APPSFLYER_APP_ID = "id987654321";
        
        private const string TEST_CONFIG_QA = "test-qa-build-config";
        private const string TEST_CONFIG_PROD = "test-prod-build-config";

        #region Utils
        private static ConfigurationManager GetManager(
                bool isProd,
                string qaConfigName = TEST_CONFIG_QA,
                string prodConfigName = TEST_CONFIG_PROD
            )
        {
            return new ConfigurationManager(
                isProd,
                true,
                (loadProdConfig) => Path.Join(
                    RESOURCES_CONFIG_PATH, 
                    loadProdConfig 
                        ? prodConfigName 
                        : qaConfigName));
        }
        #endregion
        
        [Test]
        public void TestDoesNotAllowLoadingConfigFromResources()
        {
            Assert.Throws<BuildFailedException>(() =>
            {
                var _ = new ConfigurationManager(
                    true,
                    false,
                    (_) => Path.Join(RESOURCES_CONFIG_PATH, TEST_CONFIG_QA));
            });
        }
        
        [Test]
        public void TestDoesNotAllowDuplicatedValues()
        {
            Assert.Throws<BuildFailedException>(() =>
            {
                var configManager = new ConfigurationManager(
                    true,
                    true,
                    (_) => Path.Join(RESOURCES_CONFIG_PATH, TEST_CONFIG_QA));
                var appsflyerConfig = configManager.GetConfig<ConfigurationAppsFlyer>();

                Assert.AreEqual(appsflyerConfig.AppsFlyerAppId, QA_APPSFLYER_APP_ID);
            });
        }
        
        [Test]
        public void TestConfigParse()
        {
            Assert.IsNotNull(GetManager(false));
            Assert.IsNotNull(GetManager(true));
        }

        [Test]
        public void TestConfigQaReadAndroid()
        {
            var manager = GetManager(false);
            var androidConfig = manager.GetConfig<ConfigurationAndroid>();
            
            Assert.AreEqual(androidConfig.AndroidAppName, QA_ANDROID_APP_NAME);
        }
        
        [Test]
        public void TestConfigProdReadAndroid()
        {
            var manager = GetManager(true);
            var androidConfig = manager.GetConfig<ConfigurationAndroid>();
            
            Assert.AreEqual(androidConfig.AndroidAppName, PROD_ANDROID_APP_NAME);
        }
        
        [Test]
        public void TestConfigQaReadIOS()
        {
            var manager = GetManager(false);
            var iOSConfig = manager.GetConfig<ConfigurationiOS>();
            
            Assert.AreEqual(iOSConfig.IOSAppName, QA_IOS_APP_NAME);
        }
        
        [Test]
        public void TestConfigProdReadIOS()
        {
            var manager = GetManager(true);
            var iOSConfig = manager.GetConfig<ConfigurationiOS>();
            
            Assert.AreEqual(iOSConfig.IOSAppName, PROD_IOS_APP_NAME);
        }
        
        [Test]
        public void TestConfigReadAppsFlyer()
        {
            var manager = GetManager(false);
            var appsflyerConfig = manager.GetConfig<ConfigurationAppsFlyer>();
            
            Assert.AreEqual(appsflyerConfig.AppsFlyerAppId, QA_APPSFLYER_APP_ID);
            
            manager = GetManager(true);
            appsflyerConfig = manager.GetConfig<ConfigurationAppsFlyer>();
            
            Assert.AreEqual(appsflyerConfig.AppsFlyerAppId, PROD_APPSFLYER_APP_ID);
        }
    }
}