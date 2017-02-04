# Getting Started
--------------------------------------------------
TiledLoader works by loading in tile and object layers from TMX files created in TiledLoader, instantiating each tile and object as the prefab linked to it in Unity, and applying Tiled Custom Properties to control how each instance will be initialized. This will show you how to use TiledLoader's most basic features to load a map from Tiled into Unity. For information on TiledLoader's more advanced features, see the other help sections below.

## 1. Assign TiledIDs to Prefabs
For each prefab that you wish to instantiate from a tilemap, drag it onto the prefab field in TiledLoader andfill in the name you will give it in Tiled as its TiledID, then press <b>Assign TiledID to Prefab</b>. Prefabs can also be selected by right-clicking them in the Project View and clicking <b>Manage Prefab's TiledID</b> in the context menu.When a TiledID is assigned to a prefab, an asset is created under <i>Assets/<b>TiledLoaderPath</b></i> with the name of the prefab, which links the TiledID to the prefab. These assets can be edited in the inspector, but will update automatically if the prefab is assigned another TiledID.

## 2. Set Up and Create Tiled Map
Open Tiled and create a new map. In an image editor, create a tileset with a tile to represent each prefab you want to be able to include in your level layout. Add the tileset to Tiled. Select all tiles in the tileset and under Custom Properties select <b>Add Property</b>. Add a property named <b>Name</b>. Then, for each tile in the tileset, assign the TiledID assigned to the prefab you want the tile to represent to its <b>Name</b> property. Create a map layout using these tiles in Tiled and save the map as a TMX file somewhere in your Unity project's Assets directory.

## 3. Load Tiled Map into Unity
Back in Unity, press <b>Refresh TMX File List</b> in TiledLoader and select your TMX file from the <b>TMX File</b> dropdown. Set <b>Tiled to Unity Scale Factor</b> to the number of Unity units that you want one tile in Tiled to represent, and press <b>Import TMX File</b>. Your Tiled map will be loaded into Unity, using the prefab you've assigned to each tile.



# Features
--------------------------------------------------
## Tiled Custom Properties
Tiled allows users to create Custom Properties on all tiles, objects, and layers, and assign values to them. TiledLoader works by loading in these properties and using their values to influence how their corresponding GameObjects are initialized. TiledLoader has a number of Custom Properties that it handles by default (see "Default Properties" section), and can be extended to support more (see "Extending TiledLoader" section), but the only Custom Property required for a tile to be handled by TiledLoader is the <b>Name</b> property.

## Tiled Layers
Tiled supports multiple layers for tiles and objects, with each layer able to have its own set of properties. If the <b>Parent Tiles to Layers</b> setting is enabled, layer GameObjects will be created either as empty GameObjects or as instances of the prefab assigned to <b>Layer Prefab</b>, and all tiles in each layer will become children of their layer object. Certain default layer properties will be handled regardles off the value of <b>Parent Tiles to Layers</b> and some will only be handled if it is enabled. See "Default Properties" for more information.

## Visibility
Layers and objects in Tiled can be marked as "invisible" by unchecking their <b>Visible</b> checkbox. TiledLoader will, by default, instantiate invisible layers as inactive GameObjects. Tiles can also be marked as invisible by using the <b>Visible</b> Custom Property. If this is set to false, tile objects will be set as inactive. Tiles in invisible layers will be active or inactive based on their <b>Visible</b> property, but will be treated as inactive since their parent layer object will be inactive. If <b>Ignore Layer Visibility</b> is set to true, layers will all be visible, so tile visibility will depend entirely on the value of their <b>Visible</b> property. If <b>Parent Tiles to Layers</b> is set to false and <b>Ignore Layer Visibility</b> is also false, tiles in visible layers will use their <b>Visible</b> property's value to determine whether they are active or not, but tiles in invisible layers will have this value overridden and will be set as inactive.

## [PRO] Object Layers
TiledLoader is capable of loading in Tiled object layers as well as tile layers. They work very similarly to tile layers in that the layer itself supports all of the same Custom Properties as tile layers, and the objects in the layer support most of the same Custom Properties as tiles. Objects  will be referred to as tiles everywhere else in this manual.

Objects differ from tiles in that they are matched with prefab based on the object's Name, rather than a <b>Name</b> Custom Property, and each have their own set of Custom Properties, rotation, and scaling, while tiles share the same Custom Properties across all instances. Object visibility is determined by the object's <b>Visible</b> checkbox, just like layers, instead of a Custom Property. They are also not constrained to a grid. These features make Tiled objects ideal for one-off objects or objects which must be different with each instance.

An object's Tiled rotation value will be passed in to TiledLoader as <b>Rotation</b> and will be added to the object's <b>Rotation</b> Custom Property if one is set.

An object's Tiled width and height will be passed in to TiledLoader as <b>ScaleX</b> and <b>ScaleY</b> respectively as a ratio of the object's width and height values to the Tiled map's tile width and tile height values (meaning if the object's width is set to 24 in Tiled and the map's tile width value is set to 16, the value of <b>ScaleX</b> will be 1.5). These scale values will be multiplied by the objects corresponding Custom Properties if they are set.

## [PRO] Unity Layers
Tiles and layers can be assigned to a Unity layer on import. TiledLoader cannot automatically create Unity layers, so a layer with the name assigned to the <b>Layer</b> Custom Property must have already been created in Unity for this to work. TiledLoader handles the <b>Layer</b> Custom Property on both tiles and layers by default. Tiles with this property set will override their layer's property value.

## [PRO] Automatic TiledID Assignment
TiledLoader will automatically assign a TiledID to each prefab in the Unity project by pressing the <b>Assign Default TiledID to all Prefabs</b> button in the TiledLoader window. The TiledID for each prefab will be set to the prefab's name.

## [PRO] Static Import
Tiles and layers can be marked as static on import. Marking GameObjects as static allows for performance optimizations, as well as letting them be handled by Unity's baked lighting system. TiledLoader handles the <b>Static</b> Custom Property on both tiles and layers by default. Tiles with this property set will override their layer's property value.

## [PRO] Combine Meshes and Sprites
If a layer has the <b>Combine</b> Custom Property set, and <b>Parent Tiles to Layers</b> setting is enabled, TiledLoader will combine all meshes contained within the layer into a single mesh and all sprites contained within the layer into larger sprites which will each be stored in the layer object (or as children of a child object in the case of sprites). All other Mesh Renderer and Sprite Renderer components within the layer will be disabled. This is useful for performance optimization of entirely static layers, such as level layouts. Be careful using this for combining sprites with high-resolution textures, since prerendering levels this way means that textures cannot be reused for each tile, and instead potentially very large sprites are created and saved with the scene. This can quickly inflate their size on the disc and the size of builds. (Note that the Read/Write Enabled flag must be enabled for all sprites to be included in combined sprites)



# Default Properties
--------------------------------------------------
TiledLoader uses a few custom properties in Tiled by default to determine how a tilemap will be loaded into Unity. These are properties assigned to each tile type in Tiled by adding the corresponding name to the tile's list of custom properties and assigning it a value. Each of these properties can be renamed in TiledLoader's settings file. This list can be expanded if more features are needed. See "Extending TiledLoader" section below for more information.

## TILE PROPERTIES
### Name [string]
ID to assign to tile to link it to its corresponding prefab in Unity. Should match prefab's TiledID. This is the only property that a tile must have to be imported into Unity by TiledLoader.

### Rotation [float]
Angle in degrees to rotate prefab by around Tiled's out-of-screen axis when it is instantiated in Unity.

### Height [float]
Position along Tiled's out-of-screen axis to instantiate prefab at in Unity. Scaled by Tiled to Unity Scale Factor.

### Visible [bool]
Whether to instantiate the tile as active or not. See <b>Visibility</b> under "Features" section above for more information.

### [PRO] ScaleX [float]
Amount to scale the prefab along Tiled-s X-axis when it is instantiated in Unity. Negative values will be treated as positive.

### [PRO] ScaleY [float]
Amount to scale the prefab along Tiled-s Y-axis when it is instantiated in Unity. Negative values will be treated as positive.

### [PRO] ScaleZ [float]
Amount to scale the prefab along Tiled-s out-of-screen axis when it is instantiated in Unity. Negative values will be treated as positive.

### [PRO] Static [bool]
Override for Import Static. Whether to import this tile as static or not.

### [PRO] Layer [string]
Unity layer to assign prefab instance. Overrides layer's Layer property if set. If layer is not a valid layer in Unity, instance's layer will not be changed.

## LAYER PROPERTIES
### Height [float]
Unity layer to assign prefab instance. Overrides layer's Layer property if set. If layer is not a valid layer in Unity, instance's layer will not be changed.

### [PRO] Combine [bool]
Whether to combine all meshes of prefabs in the layer into one mesh. Material of resulting mesh will be set to Default Combine Material. Note that this operation will disable all GameObjects in the layer which contain a MeshFilter component, so meshes should be moved to a child object if their GameObjects contain other components. This property will be ignored if <b>Parent Tiles to Layers</b> is turned off.

### [PRO] Static [bool]
Override for Import Static. Whether to import this layer object as static or not.

### [PRO] Layer [string]
Unity layer to assign layer object and tile instances in the layer too. If <b>Parent Tiles to Layers</b> is disabled, only tile instances will be assigned the layer. If layer is not a valid layer in Unity, layer will not be changed.



# Settings
--------------------------------------------------
All descriptions here can be viewed as tooltips by hovering the mouse over the setting in the TiledLoader settings inspector.

## CUSTOM PROPERTY NAMES
These are the names of the default Tiled custom properties to read into Unity. These are initally set to their default values, but can be configured here. For explanations of what each of these mean, see the "Default Properties" section.

### ID Property
Custom property name to use for matching tiles to prefabs

### Rotation Property
Custom property name to use for rotation of prefabs around Tiled's Z axis (out of the screen)

### Z-Axis Property
Custom property name to use for offsets along Tiled's Z-axis for layers and tiles

### Visible Property
Custom property name to use for whether tile instances are set as active on instantiation or not

### [PRO] Scale-X Property
Custom property name to use for scaling of prefabs along Tiled's X axis

### [PRO] Scale-Y Property
Custom property name to use for scaling of prefabs along Tiled's Y axis

### [PRO] Scale-Z Property
Custom property name to use for scaling of prefabs along Tiled's Z axis (out of the screen)

### [PRO] Combine Property
Custom property name to use for combining meshes in layers

### [PRO] Static Property
Custom property name to use for static overrides

### [PRO] Layer Property
Custom property name to use for Unity layers

## DEFAULT VALUES
These are the values that TiledLoader will use by default.

### Default Scaling Factor
Path relative to project's Assets directory to store TiledLoader assets at. TiledLoader assets are loaded from this directory and all subdirectories of it

### [PRO] Default Import Static
Default value for whether to instantiate all tile prefabs and layer objects as static or not

### [PRO] Default Combine Material
Default material to use for combined meshes

### [PRO] Default Object Prefab
Prefab to instantiate for unnamed Tiled Objects. An empty GameObject will be instantiated if this is set to None

## PARENT STRUCTURE
These are settings that configure the structure of how prefabs are instantiated in Unity.

### Parent Tiles to Layers
Parent all tile objects within each layer to a layer object

### Parent Layers to TiledLoader
Parent all layer objects to a TiledLoader object. Only available if Parent Tiles to Layers is enabled

### [PRO] Layer Prefab
Prefab to instantiate each layer object from. An empty GameObject will be instantiated if this is set to None

## [PRO] EXTENSION COMPONENTS
These are components that are added to all instantiated tiles and layers respectively which can be used to handle additional Tiled Custom Properties, or to provide common behavior across all instances instantiated by TiledLoader.

### [PRO] Instance Component
Component to attach to all tile objects instantiated by TiledLoader (useful for implementing HandleInstanceProperties() for all tile instances). For more information, see "Advanced" section in Help Manual

### [PRO] Layer Component
Component to attach to all layer objects instantiated by TiledLoader (useful for implementing HandleLayerProperties() for all layers). For more information, see "Advanced" section in Help Manual

## MISCELLANEOUS
### TiledLoader Path
Path relative to project's Assets directory to store TiledLoader assets at. TiledLoader assets are loaded from this directory and all subdirectories of it

### Map Tiled Y To Unity Z
Whether to map Tiled's Y-axis to Unity's Z-axis or Unity's Y-axis. This will default to unchecked for 2D games and checked for 3D games. This should usually be unchecked if using TiledLoader for a 2D game

### Ignore Layer Visibility
Whether to instantiate layer objects inactive if set to invisible in Tiled. If Parent Tiles to Layers is not enabled, tile instances in invisible layers will be instantiated as inactive regardless of their visibility state if this setting is set to false

### Dock Windows
Attempt to dock TiledLoader next to the Hierarchy and the help manual next to the inspector when they are opened. Otherwise open as floating windows

### Warn Unmatched Tiles
Show warnings for tiles without matching prefab



# Extending TiledLoader
--------------------------------------------------
TiledLoader can be easily extended if additional Custom Properties are required for either layers or tiles. This can be accomplished in your own scripts without having to modify TiledLoader's code. TiledLoader calls two different messages on instantiated objects for this purpose: 
<i>HandleInstanceProperties()</i>, and <i>HandleLayerProperties()</i>. These methods can be implemented on scripts attached to prefabs, or on the scripts set as <b>Instance Component</b> and <b>Layer Component</b> in settings to make use of additional Custom Properties of tiles and layers respectively.

Since these methods will be called from the Unity Editor, the script component that these are implemented on must have the attritube [ExecuteInEditMode]. The properties assigned to a tile or layer in Tiled will be stored in the TiledLoaderProperties component attached to each tile or layer object. To access these, simply call <i>GetComponent<TiledLoaderProperties>()</i> on the object to get the component, and then access the properties using the component's API.

<b>HandleInstanceProperties</b> is called after a prefab has been instantiated, its scaled position and its rotation have been set, and it has had its parent set to its layer if <b>Parent Tiles to Layers</b> is enabled. Use <i>TiledLoader.TiledToUnityScaleFactor</i> to access the Tiled to Unity Scaling Factor used by TiledLoader to scale positions.

<b>HandleLayerProperties</b> is called after a layer object has been instantiated and all tiles in the layer have been instantiated and have had the layer object set as their parent. This will only be called if <b>Parent Tiles to Layers</b> is enabled in TiledLoader's settings, since otherwise there will not be layer objects to call this function on.



# About
--------------------------------------------------
<b>TiledLoader</b> by Bradley Anderson  
https://github.com/branderson/TiledLoader  
brad.anderson1995@gmail.com  

<b>TiledSharp</b> Copyright 2012 Marshall Ward  
https://github.com/marshallward/TiledSharp  
Apache 2.0 License  
tiledsharp@marshallward.org  

<b>DotNetZip</b>  
https://dotnetzip.codeplex.com/   
Microsoft Public License (Ms-PL)  

<b>Roboto Font</b> Apache 2.0 License  

<b>Tiled</b> Copyright 2008-2016 Thorbjorn Lindeijer  
http://www.mapeditor.org/   
TiledLoader is currently not released under any license.



