# Diagram.NET
_Diagram.NET_ is a free open-source diagramming tools written entirely in C#.

![](Images/screen-01.png)

Put _Diagram.NET_ WinForm Control into your form and, like Microsoft Visio®,
the user can draw shapes and links. With some code you can control, change,
add and delete these elements.

## Background
<details>

This would appear to have been a reasonably popular diagramming control
for _WinForms_ development.

Unfortunately, the [company](http://www.dalssoft.com/) behind this component is now defunct.
Technology has moved on and this component is now not compatible with .NET Core.
The source code is not available, so there is no way to make this component compatible.

Further, the last available [source](https://github.com/dalssoft/diagramnet.git) is v0.4
whereas the last available binary is at v0.5

[JetBrains dotPeek](https://www.jetbrains.com/decompiler/) is a .NET decompiler and assembly browser.
This has allowed us to decompile the component and make it compatible with newer technologies.

</details>

# TODO
* ~~port to .NET Core 6~~
* fix test app
* test round trip serialisation
* ~~company+author+copyright credits~~

