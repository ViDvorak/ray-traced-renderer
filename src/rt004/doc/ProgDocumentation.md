# Programming Documentation

- [Programming Documentation](#programming-documentation)
  - [Program structure](#program-structure)
    - [Expandable classes](#expandable-classes)
      - [Camera](#camera)
      - [LightSource](#lightsource)
      - [Solid](#solid)
      - [Texture](#texture)
      - [Material](#material)
      - [LightModelComputation](#lightmodelcomputation)
      - [Adding new class](#adding-new-class)
    - [Nonexpendable classes](#nonexpendable-classes)
      - [InnerSceneObject](#innersceneobject)
      - [Scene](#scene)
      - [RendererSettings](#renderersettings)
    - [Structures](#structures)
      - [Vector2D](#vector2d)
      - [Vector3D](#vector3d)
      - [Point2D](#point2d)
      - [Point3D](#point3d)
      - [Ray](#ray)
      - [BasicPlane](#basicplane)

## Program structure

The program is structured in a way, such that the program can be easily modified and added new SceneObjects and rendering types.

![image](/images/class_structure.png)The figure describes the class structure of the program.

![image](/images/ray-traced_renderer_flowchart.png)
Flowchart of the program

The Flow of the program: On a Scene is called on mainCamera render function. The camera has specified LightModelComputation object which is used for rendering of each pixel separately.
Objects, in scene are saved in a tree structure. Where inner tree nodes are represented by InnerSceneObjects and leaves are Solids.

### Expandable classes

The program is prepared for expansion using inheritance of Camera, LightSource, Solid, Texture, Material, LightModelComputation. To add new objects with different functionality.

#### Camera

Camera specifies how and in what order are pixels rendered by Computation model set in RenderingSettings. The camera optionally the camera can be made to handle more advanced features for example multithreading or antialiasing.

#### LightSource

Represents object (usually invisible) emitting a light. The light can be emitted in different modes, for example: from point, from a plane parallel to each other.

#### Solid

Is a physical object interacting with light in a Scene.

#### Texture

Texture represents an information about a surface of a solid. Based on the position. For Image usage there needs to be an UV unwrap process.

#### Material

Defines surface properties of a Solid.

#### LightModelComputation

Computes light color and how does it reflect and refract on a solid surface.

#### Adding new class

When adding a new class, inheriting from a expandable class. There must be also created a loader class with non-parametric constructor and CreateInstance() method returning the added class. And the loader class must be registered to a loader of class, from which it is derived by tag [XmlInclude( [TYPE OF THE NEW CLASS] )] associated with the parent class.

```C#
[XmlInclude(typeof(ImageTexture)), XmlInclude(typeof(MonochromeTextureLoader))]
abstract public class TextureLoader{
  ...
}

public class ImageTextureLoader{
  public string path;

  ImageTexture CreateInstance(){
    return ImageTexture.FromFile(path);
  }
}
```

### Nonexpendable classes

There is some classes, which are not intended to be inherited from. They are
Scene, RendererSettings, ArgumentParser, Geometry, InnerSceneObject. But they can be directly modified.

#### InnerSceneObject

Represents an empty object used only for association purposes. To connect objects under one parent. which enables modifying their position and rotation in relation to the parent object.

#### Scene

Is a container for SceneObjects.

#### RendererSettings

Contains all settings for rendering with their default values.

### Structures

There is couple of new structures usually wrappers to allow only geometrically sensible operations. And structures to ease calculations for common primitives.

#### Vector2D

Represents vector in 2 dimensional space. With overridden operators such that, they can interact only with appropriate types. Function Equals and operators == and != are overridden to direct comparison of values.

| Add with  | Result type |
| :------:  | :---------: |
| --as unary op-- | Vector2D |
| Vector2D  | Vector2D |
| Point2D   | Point2D  |

| Subtract with | Result type |
| :--------: | :---------: |
| --as unary op--| Vector2D |
| Vector2D   | Vector2D  |
| Point2D    | Point2D |

| Product with | Result type |
| :----------: | :---------: |
| Vector2D | Vector2D |
| Vector2d | Vector2D |
| double   | Vector2D |

| Division with | Result type |
| :-----------: | :---------: |
| Vector2D      | Vector2D    |
| double        | Vector2D    |

#### Vector3D

Represents vector in 3 dimensional space. With overridden operators such that, they can interact only with appropriate types. Function Equals and operators == and != are overridden to direct comparison of values.

| Add with  | Result type |
| :------:  | :---------: |
| Vector3D  | Vector3D |
| Point3D   | Point3D  |

| Subtract with | Result type |
| :--------: | :---------: |
| --as unary op--| Vector3D |
| Vector3D   | Vector3D  |
| Point3D    | Point3D |

| Product with | Result type |
| :----------: | :---------: |
| Vector3D | Vector3D |
| Vector3d | Vector3D |
| double   | Vector3D |

| Division with | Result type |
| :-----------: | :---------: |
| Vector3D      | Vector3D    |
| double        | Vector3D    |

#### Point2D

Represents point in 2 dimensional space with double precision. It wraps Vector2d and overrides operators such that, they can interact only with only appropriate types. Function Equals and operators ==, != are overridden to use direct comparison of values not pointer comparison.

Note: Vector2d is imported structure from OpenTK.Mathematics

| Add with  | Result type |
| :------:  | :---------: |
| Vector2D  | Point2D |

| Subtract with | Result type |
| :--------: | :---------: |
| Point2D    | Vector2D |
| Vector2D   | Point2D |

| Product with | Result type |
| :----------: | :---------: |
| Vector2d | Vector2D |
| double   | Point2D |

| Division with | Result type |
| :-----------: | :---------: |
| Vector2d      | Point2D    |
| double        | Point2D    |

#### Point3D

Represents point in 3 dimensional space with double precision. It wraps Vector3d and overrides operators such that, they can interact only with only appropriate types. Function Equals and operators ==, != are overridden to use direct comparison of values not pointer comparison.

Note: Vector2d is imported structure from OpenTK.Mathematics

| Add with  | Result type |
| :------:  | :---------: |
| Vector2D  | Point2D |

| Subtract with | Result type |
| :--------: | :---------: |
| Point2D    | Vector2D |
| Vector2D   | Point2D |

| Product with | Result type |
| :----------: | :---------: |
| Vector2d | Vector2D |
| double   | Point2D |

| Division with | Result type |
| :-----------: | :---------: |
| Vector2d      | Point2D    |
| double        | Point2D    |

#### Ray

Ray represents a half-line by point and a vector. It is used in scene ray casting to calculate intersections with objects.

#### BasicPlane

BasicPlane represents infinite plane by a point on the plane and normal vector. It can find interception between two planes of a plane and a ray.
