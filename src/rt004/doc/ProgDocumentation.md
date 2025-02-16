z# Programing Documentation

This is high level programing description of ray-traced renderer made by Vít Dvořák.

- [Program structure](#program-structure)
  - [Expandable classes](#expandable-classes)
    - [Camera](#camera)
    - [LightSource](#lightsource)
    - [Solid](#solid)
    - [Texture](#texture)
    - [Material](#material)
    - [LightModelComputation](#lightmodelcomputation)
    - [Adding new class](#adding-new-class)
  - [Nonexpanable classes](#nonexpanable-classes)
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

The program is structured in a way, such that the program can be easely modified and added new SceneObjects and rendering types.

![image](/images/class_structure.png)The figure describes the class structure of the program.

![image](/images/ray-traced_renderer_flowchart.png)
Flowchart of the program

### Expandable classes

The program is prepared for expansion using inheritance of Camera, LightSource, Solid, Texture, Material, LightModelComputation. To add new objects with different functionality.

#### Camera

Camera specifies how and in what order are pixles rendered by Computation model set in RenderingSettings. The camera optianally the camera can be made to handel more advanced features for example multithreding or antialiasing.

#### LightSource

Represents object (usaually invisible) emitting a light. The light can be emitted in different modes, for example: from point, from a plane parallel to each other.

#### Solid

Is a physical object interacting with light in a Scene.

#### Texture

Texture represents an information about a surface of a solid. Based on the position. For Image usage there needs to be an UV unwrep process.

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
  ...
}
```

### Nonexpanable classes

There is some classes, which are not intended to be inhereted from. Thay are
Scene, RendererSettings, ArgumentParser, Geometry, InnerSceneObject. But thay can be directly modified.

#### InnerSceneObject

Represents an empty object used only for association purpouses. To connect objects under one parent. whych enables modifying theyer position and rotaion in relation to the parent object.

#### Scene

Is a container for SceneObjects.

#### RendererSettings

Constains all settings for rendering with theier default values.

### Structures

There is couple of new structures usually wrapers to allow only geometricly sansible operations. And structures to ease calculations for common primitives.

#### Vector2D

Represents vector in 2 dimensional space. With overriden operators such that, thay can interact only with apropriate types. Function Equals and operators == and != are overriden to direct comparisson of values.

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

Represents vector in 3 dimensional space. With overriden operators such that, thay can interact only with apropriate types. Function Equals and operators == and != are overriden to direct comparisson of values.

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

Represents point in 2 dimensional space with double precision. It wraps Vector2d and overrides operators such that, thay can interact only with only apropriate types. Function Equals and operators ==, != are overriden to use direct comparisson of values not pointer comparrison.

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

Represents point in 3 dimensional space with double precision. It wraps Vector3d and overrides operators such that, thay can interact only with only apropriate types. Function Equals and operators ==, != are overriden to use direct comparisson of values not pointer comparrison.

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

BasicPlane represents infinite plane by a point on the plane and normal vector. It can find intercection between two planes of a plane and a ray.
