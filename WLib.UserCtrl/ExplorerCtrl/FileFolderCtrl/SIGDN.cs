﻿/*---------------------------------------------------------------- 
// auth： https://github.com/code-mirage/FolderBrowserDialog
// date： 
// desc： Visual Studio 2017 default FolderBrowserDialog not practical The component will be overridden
// mdfy:  Windragon
//----------------------------------------------------------------*/


namespace WLib.UserCtrls.ExplorerCtrl.FileFolderCtrl
{
    public enum SIGDN : uint
    {
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        SIGDN_FILESYSPATH = 0x80058000,
        SIGDN_NORMALDISPLAY = 0,
        SIGDN_PARENTRELATIVE = 0x80080001,
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        SIGDN_URL = 0x80068000
    }
}
