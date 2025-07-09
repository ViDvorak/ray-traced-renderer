# Specification

The program should be ray-tracing renderer for basic geometrical 3D shapes. Capable of computing antialiasing, reflections, refractions and being easely expandable.

The program will have "output" and "config" command line arguments, which sets path to files. The output file can be specified also in a config file, but the cmd argument is prefered.

## Command Line Argments

The program will receve path to XML configuration file, containing definition of a scene, and path to output image file of HDR type .pfm. It will be created if doesn't exists.

## Rendering

The rendered image is created by camera object in a scene in a prespctive view.
During ray-traceing the reflection and the refraction rays are considered and traced. While the ray color contribution is noticible or the recursion is not too deep. And The maximal depth is configurable.
The defined objecs are spheres and planes, but it should be easy to add other objects.

## Scene definition

The scene is defined in config file using XML.

## Output

The ouput is an image at path specified by cmd arumment or config file of HDR type .pfm.

## Perforamce

The performance is dependent on applied setting. Mainly on the camera resolution and max ray recursion depth.

## Libraries

The program uses only standard library. Notable is System.Xml.Serialization for parsing XML config file.
