# CSLibreCell - a FreeCell implementation

Copyright tristhaus 2021 and later.

## For Users

CSLibreCell is a FreeCell implementation that keeps track of which deals you have already played (how far along your _"journey"_ is). The first stage contains the winnable games from 1 to 32000 (_i.e._ the original Microsoft FreeCell set), the second stage from 32001 to 64000. Check the Status window for your progress.

The implementation is terminal-based and has a text interface, which is resizable and zoomable in modern terminals such as the PowerShell. It supports mouse and keyboard input. The former will highlight the first cell/column you selected, the latter allows for configuring custom keys. Check the Help window for a listing of active keys. See the `sample` directory for an example file to be placed in `$APPDATA/CSLibreCell`, which is usually found at `C:\Users\yourname\AppData\Roaming\CSLibreCell`. The `Properties` as available via the context menu of the title bar allow to change the zoom and dimensions of the window permanently. 

CSLibreCell currently supports English and German localization. Other translations will not be made by me, so do not hold me to them.

![main](/../screenshot/main.png?raw=true) ![status](/../screenshot/status.png?raw=true)

## For Developers

Currently, I am not looking for code contributions to this project. If you would like to create a translation, feel free to do so in a pull request.

## License
All source code licensed under GPL v3 (see LICENSE for terms). Icon licensed to this project from Flaticon (see [Attributions](#attributions)).

## Attributions
Icon (two aces) attributed to: Icons made by [Freepik](https://www.freepik.com) from [Flaticon](https://www.flaticon.com/)

[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library (via Nuget) by James Newton-King used under the [MIT license](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md).

[Terminal.Gui](https://github.com/migueldeicaza/gui.cs) library (via Nuget) by Miguel de Icaza, Charlie Kindel (github: @tig), github: @BDisp used under the [MIT license](https://github.com/migueldeicaza/gui.cs/blob/master/LICENSE).

