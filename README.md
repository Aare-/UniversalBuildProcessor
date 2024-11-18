# UniversalBuildProcessor
Universal Build Processor script for unity.
Supports mobile projects for iOS and Android.
Allows to easilly configure unity with values and secrets stored in `build-config.json` during CI build.

## Paths
Paths where this plugin will look for config files are stored inside BuildProcessor.Consts.cs class.
Place the config files named `QA_CONFIG_JSON_NAME` in the `CONFIG_PATH_FOLDER` for the script to pick them up.
`PROD_CONFIG_JSON_NAME` json file should be only used during production build and removed after.
Place your optional and custom Scriptable Objects inside `CONFIG_MODEL_PATH_FOLDER`.

## Sample Config Files
To get started with `build-config.json` you can find sample configs inside `UniversalBuildProcessor\Resources\TestConfigs` folder.
Only the sections with data for Android and iOS is mandatory.

## Mandatory Configs
Values from following classes have to be present in config.json
- `ConfigurationAndroid.cs`
- `ConfigurationiOS.cs`

## Optional Configs
Depending on the plugin usage, there are built in configurations for commonly used integrations.
To make use of them:
1. Create scriptable object in using options menu inside *BuildConfig* with the default
2. Place the created scriptable object inside the `CONFIG_MODEL_PATH_FOLDER` (`Assets/Resources/BuildConfig`)
3. Include keys from matching Configuration class (`UniversalBuildProcessor/Editor/BuildProcessorConfigurations`) in the `build-config.json`
4. Done: you can reference and read values from the scriptable object as needed and they will be up to date in the build.

## Custom Configs
You can create custom config with any string value as needed.
To do that one has to:
1. Create Scriptable Object with public fields
2. Create Configuration class with annotation of type CustomBuildConfiguration and pass type of matching Scriptable Object
3. Field names has to match between these two classes to be filled out
4. You can use JsonProperty to ensure values are required, DebugConfigField attribute to print value during build and EnsureNotPlaceholder attribute to ensure values are different between QA and Prod
5. Add matching fields to both `qa-build-config.json` and `prod-build-config.json`

If in doubt, mirror the implementation of built in optional models found in `UniversalBuildProcessor/Editor/BuildProcessorConfigurations` and `UniversalBuildProcessor/Editor/BuildProcessorModel`

## Annotations
In addition to JsonProperty annotation two additional annotations can be added to config classes:
- `DebugConfigFieldAttribute` Add this one to any field you want to have printed during build (helps verify correct value was used)
- `EnsureNotPlaceholder` Add this one to any field you want to make sure is different between qa and prod config. Build will fail if this is not the case (f.e. you forgot to put prod values)

## Running Build
To make a build using this script unity has to be called from the command line in batchmode.
Following additional command line parameters are required:
- `-buildTarget` Values: `ios` or `android`
- `-buildEnvironment` Values: `qa` or `production`
- `-executeMethod` Value: `BuildProcessor.Build`
- `-outputPath` Value: folder for build files
- `-buildName` Value: name of the output build file

Example of the complete invocation:
`Unity -quit -batchmode -nographics -buildTarget ios -buildEnvironment production -executeMethod BuildProcessor.Build -outputPath "Builds/iOS" -buildName "MyGame" -username "test@unity.com" -password 'secretunitypassword'`
