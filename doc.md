# Documantation

## Argument Parsing

Parses primary arguments form command line and after that uses config file to fill other paramters. And non-specified paramters have default value specified in code.

The logic is handeled in static class ArgumentParser by calling method ParseArguments(string[] args). If there is any problem detected with format of input an ArgumentException is thrown.

&nbsp;

### Parameters

Parameter names are case in-sensitive. Whitespaces are trimed from the begining and end of entries and keys.

| Parameter      | Description                                        | Default value   |
| :------        |    :------:                                        |          ------:|
| Config         | Path to config file from project directory.        | config.txt      |
| Output         | Path from project directory.                       | output.txt      |
| Width          | sets width of an output image.                     | 640             |
| Height         | sets height of an output image.                    | 360             |

## Scene objects

Scene objects are objects assigneble to a scene. There are three basic types Camera, LightSource and Solid. Each scene object must have programmer defined Loader class used for loading all nessesery data for cration of eqvivalent standard SceneObject. The main benefit is that eaven properties, select private fields of eqvivalent class can be loaded automaticly and proper Scene registration of SceneObjects can be ensured.

&nbsp;

### SceneObject

SceneObject is abstract parent class, common for all Objects in a Scene.
Can't be moved beatween scenes. If some body needs to move SceneObject to different a scene, the expected solution is to create new object with copied properties.

#### Constructors:
| Function | description |
|  :-----  |     -------:|
| SceneObject ( Scene parentScene, Vector3 position, Vector3 rotation) | Creates SceneObject with specified properties. Typicly not used in user code. It is called by constructors of derived classes (forexample class Solid) |

#### Implements methods:
| return type | Function | description |
|:-----       |  :----:  |     -------:|
| Vector3 | GetHeding() | returns object local X axis in Scene Coordintes |
| Vector3 | ConvertFromObjectToWorldSpace(Vector3 vectorInObjectSpace, bool isPosition) | Converts coordinets from local space to World Space |

#### properties:
| type | name | description |
|:-----       |  :----:  |     -------:|
| Vector3 | Position | sets and gets position of the SceneObject |
| Quaternion | Rotatation | gets and sets rotation qvaternion |

public fields:
| type | name | description |
|:-----       |  :----:  |     -------:|
| Vector3 | . | .|

&nbsp;

### SceneObjectLoader

 Each scene object must have programmer defined Loader class used for loading all nessesery data for cration of eqvivalent standard scene object. The main benefit is that properties, select private fields of eqvivalent class can be loaded automaticly and proper registration of SceneObjects to Scene can be ensured.
 
 Be awere that, because of automatic XML loading is required that newly added scene objects must be manualy registered to the ParentClass. ForExample PointLightLoader must be registred to the LightSourceLoader by adding atribute [ XmlInclude( typeof(PointLightLoader) ) ] 
