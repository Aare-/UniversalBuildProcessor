
public partial class BuildProcessor
{
    /**
     * Here put your build-config.json files
     */
    private const string CONFIG_PATH_FOLDER = "Assets/Editor/BuildConfig";
    
    /**
     * Here put your BuildProcessorConfigurations Scriptable Objects
     */
    private const string CONFIG_MODEL_PATH_FOLDER = "Assets/Resources/BuildConfig";

    /**
     * Use this name for QA build config
     */
    private const string QA_CONFIG_JSON_NAME = "qa-build-config.json";
    
    /**
     * Use this name for PROD build config
     */
    private const string PROD_CONFIG_JSON_NAME = "prod-build-config.json";

    /**
     * Path to playfab PlayFabSharedSetting (if exists)
     */
    private const string PLAYFAB_CONFIG_PATH = "Assets/PlayFabSDK/Shared/Public/Resources/PlayFabSharedSettings.asset";
}
