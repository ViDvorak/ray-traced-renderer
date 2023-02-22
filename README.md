# RT004
Support for NPGR004 (Photorealistic graphics) lecture

## Project plan
Every `stepNN` directory refers to one item of the lab plan
(see [row 1 of this table](https://docs.google.com/spreadsheets/d/1jnkLW1R7_FYD6QcWP1X_a3_xoVFAU-LzF6ErXKNgqVs/edit?usp=sharing))

More details will be provided in this repository. Checkpoint
information will be included in the associated step directories
(e.g. `step03` contains definition of the `Checkpoint 1` as well).

## src

The src directory contains support files from the lecturer. Default
version uses `.NET 7.0`, you can use the `.NET 6.0` variant if necessary.

## Point table

See [this shared table](https://docs.google.com/spreadsheets/d/1jnkLW1R7_FYD6QcWP1X_a3_xoVFAU-LzF6ErXKNgqVs/edit?usp=sharing)
for current points. Check the dates of individual Checkpoints...

Contact me <pepca@cgg.mff.cuni.cz> for any suggestions, comments or
complaints.

## Notes

* If anything doesn't work well in your Linux/macOS environment,
  you should write me (<pepca@cgg.mff.cuni.cz>) as soon as possible.
  Of course you could report positive experience in Linux/macOS as well.
* After some thinking I've come to a recommendation for you - use
  the `git fork` command at the beginning of the semester and
  use special branches (e.g. `Checkpoint 1` etc.) for archiving your
  progress at the checkpoints.
* It seems like the `System.Numerics` library doesn't support `double`
  types (yet?), so I'm going to use the lightweighted `OpenTK.Mathematics`
  library instead, distributed in [NuGet form](https://www.nuget.org/packages/OpenTK.Mathematics/5.0.0-pre.8)

# Documantation

## Argument Parsing

Parses primary arguments form command line and after that uses config file to fill other paramters. And non-specified paramters have default value specified in code.

The logic is handeled in static class ArgumentParser by calling method ParseArguments(string[] args). If there is any problem detected with format of input an ArgumentException is thrown.

### Parameters
Parameter names are case in-sensitive. Whitespaces are trimed from the begining and end of entries and keys.

| Parameter      | Description                                        | Default value   |
| :------        |    :------:                                        |          ------:|
| Config         | Path to config file from project directory.        | config.txt      |
| Output         | Path from project directory.                       | output.txt      |
| Width          | sets width of an output image.                     | 640             |
| Height         | sets height of an output image.                    | 360             |
