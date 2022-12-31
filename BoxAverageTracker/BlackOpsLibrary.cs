using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BoxAverageTracker
{
    public static class Constants
    {
        #region Members
        public const int C_PROCESS_ALL_ACCESS = 0x1F0FFF;

        public const int C_TIMEADDRESS = 0x01A79870;
        public const int C_MAPADDRESS = 0x02F67B6C;
        public const int C_MOONTEDDYBEAR = 0x0328F674;

        public const int C_WHITEPOINTSADDRESS = 0x01C0A6C8;
        public const int C_WHITEKILLSADDRESS = 0x01C0A6CC;
        public const int C_WHITEDOWNSADDRESS = 0x01C0A6E4;
        public const int C_WHITEREVIVESADDRESS = 0x01C0A6E8;

        public const int C_WHITEWEAPONSLOT1 = 0x01C08EFC;
        public const int C_WHITEWEAPONSLOT2 = 0x01C08F04;
        public const int C_WHITEWEAPONSLOT3 = 0x01C08F0C;
        public const int C_WHITEWEAPONSLOT4 = 0x01C08F14;
        public const int C_WHITEWEAPONSLOT5 = 0x01C08F1C;
        public const int C_WHITEWEAPONSLOT6 = 0x01C08F24;
        public const int C_WHITEWEAPONSLOT7 = 0x01C08F2C;
        public const int C_WHITEWEAPONSLOT8 = 0x01C08F34;
        public const int C_WHITEWEAPONSLOT9 = 0x01C08F3C;

        public const int C_WHITEAMMOSSLOT1 = 0x01C08E88;
        public const int C_WHITEAMMOSSLOT1_2 = 0x01C08F00;
        public const int C_WHITEAMMOSSLOT2 = 0x01C08E98;
        public const int C_WHITEAMMOSSLOT2_2 = 0x01C08F10;
        public const int C_WHITEAMMOSSLOT3 = 0x01C08EA0;
        public const int C_WHITEAMMOSSLOT3_2 = 0x01C08F18;



        public const int C_BLUEPOINTSADDRESS = 0x01C0C3F0;
        public const int C_BLUEKILLSADDRESS = 0x01C0C3F4;
        public const int C_BLUEDOWNSADDRESS = 0x01C0C40C;
        public const int C_BLUEREVIVESADDRESS = 0x01C0C410;

        public const int C_BLUEWEAPONSLOT1 = 0x01C0AC24;
        public const int C_BLUEWEAPONSLOT2 = 0x01C0AC2C;
        public const int C_BLUEWEAPONSLOT3 = 0x01C0AC34;
        public const int C_BLUEWEAPONSLOT4 = 0x01C0AC3C;
        public const int C_BLUEWEAPONSLOT5 = 0x01C0AC44;
        public const int C_BLUEWEAPONSLOT6 = 0x01C0AC4C;
        public const int C_BLUEWEAPONSLOT7 = 0x01C0AC54;
        public const int C_BLUEWEAPONSLOT8 = 0x01C0AC5C;
        public const int C_BLUEWEAPONSLOT9 = 0x01C0AC64;

        public const int C_BLUEAMMOSSLOT1 = 0x01C0ABB0;
        public const int C_BLUEAMMOSSLOT1_2 = 0x01C0AC28;
        public const int C_BLUEAMMOSSLOT2 = 0x01C0ABC0;
        public const int C_BLUEAMMOSSLOT2_2 = 0x01C0AC38;
        public const int C_BLUEAMMOSSLOT3 = 0x01C0ABC8;
        public const int C_BLUEAMMOSSLOT3_2 = 0x01C0AC46;



        public const int C_YELLOWPOINTSADDRESS = 0x01C0E118;
        public const int C_YELLOWKILLSADDRESS = 0x01C0E11C;
        public const int C_YELLOWDOWNSADDRESS = 0x01C0E134;
        public const int C_YELLOWREVIVESADDRESS = 0x01C0E138;

        public const int C_YELLOWWEAPONSLOT1 = 0x01C0C94C;
        public const int C_YELLOWWEAPONSLOT2 = 0x01C0C954;
        public const int C_YELLOWWEAPONSLOT3 = 0x01C0C95C;
        public const int C_YELLOWWEAPONSLOT4 = 0x01C0C964;
        public const int C_YELLOWWEAPONSLOT5 = 0x01C0C96C;
        public const int C_YELLOWWEAPONSLOT6 = 0x01C0C974;
        public const int C_YELLOWWEAPONSLOT7 = 0x01C0C97C;
        public const int C_YELLOWWEAPONSLOT8 = 0x01C0C984;
        public const int C_YELLOWWEAPONSLOT9 = 0x01C0C98C;

        public const int C_YELLOWAMMOSSLOT1 = 0x01C0C8D8;
        public const int C_YELLOWAMMOSSLOT1_2 = 0x01C0C950;
        public const int C_YELLOWAMMOSSLOT2 = 0x01C0C8E8;
        public const int C_YELLOWAMMOSSLOT2_2 = 0x01C0C8E8;
        public const int C_YELLOWAMMOSSLOT3 = 0x01C0C8F0;
        public const int C_YELLOWAMMOSSLOT3_2 = 0x01C0C96E;



        public const int C_GREENPOINTSADDRESS = 0x01C0FE40;
        public const int C_GREENKILLSADDRESS = 0x01C0FE44;
        public const int C_GREENDOWNSADDRESS = 0x01C0FE5C;
        public const int C_GREENREVIVESADDRESS = 0x01C0FE60;

        public const int C_GREENWEAPONSLOT1 = 0x01C0E674;
        public const int C_GREENWEAPONSLOT2 = 0x01C0E67C;
        public const int C_GREENWEAPONSLOT3 = 0x01C0E684;
        public const int C_GREENWEAPONSLOT4 = 0x01C0E68C;
        public const int C_GREENWEAPONSLOT5 = 0x01C0E694;
        public const int C_GREENWEAPONSLOT6 = 0x01C0E69C;
        public const int C_GREENWEAPONSLOT7 = 0x01C0E6A4;
        public const int C_GREENWEAPONSLOT8 = 0x01C0E6AC;
        public const int C_GREENWEAPONSLOT9 = 0x01C0E6B4;

        public const int C_GREENAMMOSSLOT1 = 0x01C0E600;
        public const int C_GREENAMMOSSLOT1_2 = 0x01C0E678;
        public const int C_GREENAMMOSSLOT2 = 0x01C0E610;
        public const int C_GREENAMMOSSLOT2_2 = 0x01C0E688;
        public const int C_GREENAMMOSSLOT3 = 0x01C0E618;
        public const int C_GREENAMMOSSLOT3_2 = 0x01C0E696;
        #endregion
    }

    public static class BlackOpsLibrary
    {
        #region Imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int _DwDesiredAccess, bool _InheritHandle, int _DwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); //ShowWindow needs an IntPtr

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int _HandleProcess, int _LpBaseAddress, byte[] _LpBuffer, int _DwSize, ref int _LpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(int _HandleProcess, int _LpBaseAddress, byte[] _LpBuffer, int _DwSize, ref int _LpNumberOfBytesRead);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr _HandleWindow, int _Msg, int _WParam, int _LParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        #region Members
        public static Process m_BlackOpsProcess = null;
        public static IntPtr? m_ProcessHandle = IntPtr.Zero;

        public static Dictionary<int, string> m_AscensionWeapons = new Dictionary<int, string>
                    {
                        { 7, "Python" },
                        { 8, "Python" },
                        { 9, "CZ75" },
                        { 10, "CZ75" },
                        { 16, "G11" },
                        { 17, "G11" },
                        { 18, "FAMAS" },
                        { 19, "FAMAS" },
                        { 29, "Spectre" },
                        { 30, "Spectre" },
                        { 32, "CZ75x2" },
                        { 34, "CZ75x2" },
                        { 39, "SPAS-12" },
                        { 40, "SPAS-12" },
                        { 41, "HS-10" },
                        { 43, "HS-10" },
                        { 44, "AUG" },
                        { 45, "AUG" },
                        { 47, "Galil" },
                        { 48, "Galil" },
                        { 49, "Commando" },
                        { 50, "Commando" },
                        { 51, "FN FAL" },
                        { 52, "FN FAL" },
                        { 53, "Dragunov" },
                        { 54, "Dragunov" },
                        { 55, "L96A1" },
                        { 56, "L96A1" },
                        { 57, "RPK" },
                        { 58, "RPK" },
                        { 59, "HK21" },
                        { 60, "HK21" },
                        { 61, "M72 LAW" },
                        { 62, "M72 LAW" },
                        { 63, "China Lake" },
                        { 64, "China Lake" },
                        { 65, "Gersh" },
                        { 66, "Matrioshka" },
                        { 67, "Ray Gun" },
                        { 68, "Ray Gun" },
                        { 69, "Thundergun" },
                        { 70, "Thundergun" },
                        { 71, "Crossbow" },
                        { 72, "Crossbow" },
                        { 73, "Ballistic Knife" },
                        { 74, "Ballistic Knife" },
                        { 75, "Ballistic Knife" },
                        { 76, "Ballistic Knife" },
                    };

        public static Dictionary<int, string> m_CallOfTheDeadWeapons = new Dictionary<int, string>
                    {
                        { 8, "Python" },
                        { 9, "Python" },
                        { 10, "CZ75" },
                        { 11, "CZ75" },
                        { 17, "G11" },
                        { 18, "G11" },
                        { 19, "FAMAS" },
                        { 20, "FAMAS" },
                        { 30, "Spectre" },
                        { 31, "Spectre" },
                        { 35, "CZ75x2" },
                        { 37, "CZ75x2" },
                        { 42, "SPAS-12" },
                        { 43, "SPAS-12" },
                        { 44, "HS-10" },
                        { 46, "HS-10" },
                        { 47, "AUG" },
                        { 48, "AUG" },
                        { 50, "Galil" },
                        { 51, "Galil" },
                        { 52, "Commando" },
                        { 53, "Commando" },
                        { 54, "FN FAL" },
                        { 55, "FN FAL" },
                        { 56, "Dragunov" },
                        { 57, "Dragunov" },
                        { 58, "L96A1" },
                        { 59, "L96A1" },
                        { 60, "RPK" },
                        { 61, "RPK" },
                        { 62, "HK21" },
                        { 63, "HK21" },
                        { 64, "M72 LAW" },
                        { 65, "M72 LAW" },
                        { 66, "China Lake" },
                        { 67, "China Lake" },
                        { 68, "Ray Gun" },
                        { 69, "Ray Gun" },
                        { 70, "Crossbow" },
                        { 71, "Crossbow" },
                        { 72, "VR-11" },
                        { 73, "VR-11" },
                        { 74, "Scavenger" },
                        { 75, "Scavenger" },
                        { 77, "Ballistic Knife" },
                        { 78, "Ballistic Knife" },
                        { 79, "Ballistic Knife" },
                        { 80, "Ballistic Knife" },
                    };

        public static Dictionary<int, string> m_DerRieseWeapons = new Dictionary<int, string>
                    {
                        { 6, "Python" },
                        { 31, "Python" },
                        { 7, "CZ75" },
                        { 32, "CZ75" },
                        { 8, "G11" },
                        { 33, "G11" },
                        { 9, "FAMAS" },
                        { 34, "FAMAS" },
                        { 10, "Spectre" },
                        { 35, "Spectre" },
                        { 12, "CZ75x2" },
                        { 37, "CZ75x2" },
                        { 13, "SPAS-12" },
                        { 38, "SPAS-12" },
                        { 14, "HS-10" },
                        { 40, "HS-10" },
                        { 15, "AUG" },
                        { 41, "AUG" },
                        { 16, "Galil" },
                        { 43, "Galil" },
                        { 17, "Commando" },
                        { 44, "Commando" },
                        { 18, "FN FAL" },
                        { 45, "FN FAL" },
                        { 19, "Dragunov" },
                        { 46, "Dragunov" },
                        { 20, "L96A1" },
                        { 47, "L96A1" },
                        { 21, "RPK" },
                        { 48, "RPK" },
                        { 22, "HK21" },
                        { 49, "HK21" },
                        { 23, "M72 LAW" },
                        { 50, "M72 LAW" },
                        { 24, "China Lake" },
                        { 51, "China Lake" },
                        { 26, "Crossbow" },
                        { 52, "Crossbow" },
                        { 27, "Ballistic Knife" },
                        { 53, "Ballistic Knife" },
                        { 28, "Ballistic Knife" },
                        { 54, "Ballistic Knife" },
                        { 76, "Ray Gun" },
                        { 77, "Ray Gun" },
                        { 78, "Wunderwaffe DG-2" },
                        { 79, "Wunderwaffe DG-2" },
                    };

        public static Dictionary<int, string> m_FiveWeapons = new Dictionary<int, string>
                    {
                        { 8, "Python" },
                        { 9, "Python" },
                        { 10, "CZ75" },
                        { 11, "CZ75" },
                        { 17, "G11" },
                        { 18, "G11" },
                        { 19, "FAMAS" },
                        { 20, "FAMAS" },
                        { 30, "Spectre" },
                        { 31, "Spectre" },
                        { 33, "CZ75x2" },
                        { 35, "CZ75x2" },
                        { 40, "SPAS-12" },
                        { 41, "SPAS-12" },
                        { 42, "HS-10" },
                        { 44, "HS-10" },
                        { 45, "AUG" },
                        { 46, "AUG" },
                        { 48, "Galil" },
                        { 49, "Galil" },
                        { 50, "Commando" },
                        { 51, "Commando" },
                        { 52, "FN FAL" },
                        { 53, "FN FAL" },
                        { 54, "Dragunov" },
                        { 55, "Dragunov" },
                        { 56, "L96A1" },
                        { 57, "L96A1" },
                        { 58, "RPK" },
                        { 59, "RPK" },
                        { 60, "HK21" },
                        { 61, "HK21" },
                        { 62, "M72 LAW" },
                        { 63, "M72 LAW" },
                        { 64, "China Lake" },
                        { 65, "China Lake" },
                        { 67, "Ray Gun" },
                        { 68, "Ray Gun" },
                        { 69, "Winter's howl" },
                        { 70, "Winter's howl" },
                        { 72, "Crossbow" },
                        { 73, "Crossbow" },
                        { 74, "Ballistic Knife" },
                        { 75, "Ballistic Knife" },
                        { 76, "Ballistic Knife" },
                        { 77, "Ballistic Knife" },
                    };

        public static Dictionary<int, string> m_KinoDerTotenWeapons = new Dictionary<int, string>
                    {
                        { 9, "Python" },
                        { 10, "Python" },
                        { 11, "CZ75" },
                        { 12, "CZ75" },
                        { 18, "G11" },
                        { 19, "G11" },
                        { 20, "FAMAS" },
                        { 21, "FAMAS" },
                        { 33, "Spectre" },
                        { 34, "Spectre" },
                        { 36, "CZ75x2" },
                        { 38, "CZ75x2" },
                        { 43, "SPAS-12" },
                        { 44, "SPAS-12" },
                        { 45, "HS-10" },
                        { 47, "HS-10" },
                        { 48, "AUG" },
                        { 49, "AUG" },
                        { 51, "Galil" },
                        { 52, "Galil" },
                        { 53, "Commando" },
                        { 54, "Commando" },
                        { 55, "FN FAL" },
                        { 56, "FN FAL" },
                        { 57, "Dragunov" },
                        { 58, "Dragunov" },
                        { 59, "L96A1" },
                        { 60, "L96A1" },
                        { 61, "RPK" },
                        { 62, "RPK" },
                        { 63, "HK21" },
                        { 64, "HK21" },
                        { 65, "M72 LAW" },
                        { 66, "M72 LAW" },
                        { 67, "China Lake" },
                        { 68, "China Lake" },
                        { 70, "Ray Gun" },
                        { 71, "Ray Gun" },
                        { 72, "Thundergun" },
                        { 73, "Thundergun" },
                        { 74, "Crossbow" },
                        { 75, "Crossbow" },
                        { 76, "Ballistic Knife" },
                        { 77, "Ballistic Knife" },
                        { 78, "Ballistic Knife" },
                        { 79, "Ballistic Knife" },
                    };

        public static Dictionary<int, string> m_MoonWeapons = new Dictionary<int, string>
                    {
                        { 9, "Python" },
                        { 10, "Python" },
                        { 11, "CZ75" },
                        { 12, "CZ75" },
                        { 18, "G11" },
                        { 19, "G11" },
                        { 20, "FAMAS" },
                        { 21, "FAMAS" },
                        { 33, "Spectre" },
                        { 34, "Spectre" },
                        { 36, "CZ75x2" },
                        { 38, "CZ75x2" },
                        { 43, "SPAS-12" },
                        { 44, "SPAS-12" },
                        { 45, "HS-10" },
                        { 47, "HS-10" },
                        { 48, "AUG" },
                        { 49, "AUG" },
                        { 51, "Galil" },
                        { 52, "Galil" },
                        { 53, "Commando" },
                        { 54, "Commando" },
                        { 55, "FN FAL" },
                        { 56, "FN FAL" },
                        { 57, "Dragunov" },
                        { 58, "Dragunov" },
                        { 59, "L96A1" },
                        { 60, "L96A1" },
                        { 61, "RPK" },
                        { 62, "RPK" },
                        { 63, "HK21" },
                        { 64, "HK21" },
                        { 65, "M72 LAW" },
                        { 66, "M72 LAW" },
                        { 67, "China Lake" },
                        { 68, "China Lake" },
                        { 69, "Ballistic Knife" },
                        { 70, "Ballistic Knife" },
                        { 71, "Ballistic Knife" },
                        { 72, "Ballistic Knife" },
                        { 73, "Gersh" },
                        { 74, "Ray Gun" },
                        { 75, "Ray Gun" },
                        { 76, "QED" },
                        { 78, "Wavegun" },
                        { 81, "Wavegun" },
                    };

        public static Dictionary<int, string> m_NachtDerUntotenWeapons = new Dictionary<int, string>
                    {
                        { 5, "Python" },
                        { 6, "CZ75" },
                        { 7, "G11" },
                        { 8, "FAMAS" },
                        { 9, "Spectre" },
                        { 11, "CZ75x2" },
                        { 12, "SPAS-12" },
                        { 13, "HS-10" },
                        { 14, "AUG" },
                        { 15, "Galil" },
                        { 16, "Commando" },
                        { 17, "FN FAL" },
                        { 18, "Dragunov" },
                        { 19, "L96A1" },
                        { 20, "RPK" },
                        { 21, "HK21" },
                        { 22, "M72 LAW" },
                        { 23, "China Lake" },
                        { 25, "Ray Gun" },
                        { 26, "Crossbow" },
                        { 27, "Ballistic Knife" },
                        { 37, "Thundergun" },
                    };

        public static Dictionary<int, string> m_ShangriLaWeapons = new Dictionary<int, string>
                    {
                        { 8, "Python" },
                        { 9, "Python" },
                        { 10, "CZ75" },
                        { 11, "CZ75" },
                        { 17, "G11" },
                        { 18, "G11" },
                        { 19, "FAMAS" },
                        { 20, "FAMAS" },
                        { 30, "Spectre" },
                        { 31, "Spectre" },
                        { 33, "CZ75x2" },
                        { 35, "CZ75x2" },
                        { 40, "SPAS-12" },
                        { 41, "SPAS-12" },
                        { 42, "HS-10" },
                        { 44, "HS-10" },
                        { 45, "AUG" },
                        { 46, "AUG" },
                        { 48, "Galil" },
                        { 49, "Galil" },
                        { 50, "Commando" },
                        { 51, "Commando" },
                        { 52, "FN FAL" },
                        { 53, "FN FAL" },
                        { 54, "Dragunov" },
                        { 55, "Dragunov" },
                        { 56, "L96A1" },
                        { 57, "L96A1" },
                        { 58, "RPK" },
                        { 59, "RPK" },
                        { 60, "HK21" },
                        { 61, "HK21" },
                        { 62, "M72 LAW" },
                        { 63, "M72 LAW" },
                        { 64, "China Lake" },
                        { 65, "China Lake" },
                        { 67, "Ray Gun" },
                        { 68, "Ray Gun" },
                        { 69, "31-79 Jgb 215" },
                        { 70, "31-79 Jgb 215" },
                        { 71, "Crossbow" },
                        { 72, "Crossbow" },
                        { 73, "Ballistic Knife" },
                        { 74, "Ballistic Knife" },
                        { 75, "Ballistic Knife" },
                        { 76, "Ballistic Knife" },
                    };

        public static Dictionary<int, string> m_ShiNoNumaWeapons = new Dictionary<int, string>
                    {
                        { 6, "Python" },
                        { 7, "CZ75" },
                        { 8, "G11" },
                        { 9, "FAMAS" },
                        { 10, "Spectre" },
                        { 12, "CZ75x2" },
                        { 13, "SPAS-12" },
                        { 14, "HS-10" },
                        { 15, "AUG" },
                        { 16, "Galil" },
                        { 17, "Commando" },
                        { 18, "FN FAL" },
                        { 19, "Dragunov" },
                        { 20, "L96A1" },
                        { 21, "RPK" },
                        { 22, "HK21" },
                        { 23, "M72 LAW" },
                        { 24, "China Lake" },
                        { 26, "Ray Gun" },
                        { 27, "Crossbow" },
                        { 28, "Ballistic Knife" },
                        { 39, "Wunderwaffe DG-2" },
                    };

        public static Dictionary<int, string> m_VerrucktWeapons = new Dictionary<int, string>
                    {
                        { 5, "Python" },
                        { 6, "CZ75" },
                        { 7, "G11" },
                        { 8, "FAMAS" },
                        { 9, "Spectre" },
                        { 11, "CZ75x2" },
                        { 12, "SPAS-12" },
                        { 13, "HS-10" },
                        { 14, "AUG" },
                        { 15, "Galil" },
                        { 16, "Commando" },
                        { 17, "FN FAL" },
                        { 18, "Dragunov" },
                        { 19, "L96A1" },
                        { 20, "RPK" },
                        { 21, "HK21" },
                        { 22, "M72 LAW" },
                        { 23, "China Lake" },
                        { 25, "Ray Gun" },
                        { 26, "Crossbow" },
                        { 27, "Ballistic Knife" },
                        { 39, "Winter's howl" },
                    };

        #endregion

        #region Getters / Setters
        public static Process BlackOpsProcess
        {
            get
            {
                if (m_BlackOpsProcess != null)
                {
                    try
                    {
                        DateTime exitTime = m_BlackOpsProcess.ExitTime;
                        m_BlackOpsProcess = null;
                    }
                    catch (Exception)
                    {
                    }
                }

                if (m_BlackOpsProcess == null)
                {
                    Process[] processes = Process.GetProcessesByName("BlackOps");
                    int count = processes.Length;
                    if (count <= 0)
                    {
                        processes = Process.GetProcessesByName("BGamerT5");
                    }

                    if (m_BlackOpsProcess != null && count <= 0)
                    {
                        m_BlackOpsProcess = null;
                    }

                    if (count > 0)
                    {
                        m_BlackOpsProcess = processes[0];
                    }
                }

                return m_BlackOpsProcess;
            }
        }

        public static IntPtr? BlackOpsHandle
        {
            get
            {
                if (BlackOpsProcess == null)
                {
                    m_ProcessHandle = IntPtr.Zero;
                }
                if ((m_ProcessHandle == IntPtr.Zero || m_ProcessHandle == null) && BlackOpsProcess != null)
                {
                    m_ProcessHandle = OpenProcess(Constants.C_PROCESS_ALL_ACCESS, false, BlackOpsProcess.Id);
                }

                return m_ProcessHandle;
            }
        }

        public static Dictionary<int, string> AscensionWeapons
        {
            get
            {
                return m_AscensionWeapons;
            }
        }

        public static Dictionary<int, string> CallOfTheDeadWeapons
        {
            get
            {
                return m_CallOfTheDeadWeapons;
            }
        }

        public static Dictionary<int, string> DerRieseWeapons
        {
            get
            {
                return m_DerRieseWeapons;
            }
        }

        public static Dictionary<int, string> FiveWeapons
        {
            get
            {
                return m_FiveWeapons;
            }
        }

        public static Dictionary<int, string> KinoDerTotenWeapons
        {
            get
            {
                return m_KinoDerTotenWeapons;
            }
        }

        public static Dictionary<int, string> MoonWeapons
        {
            get
            {
                return m_MoonWeapons;
            }
        }

        public static Dictionary<int, string> NachtDerUntotenWeapons
        {
            get
            {
                return m_NachtDerUntotenWeapons;
            }
        }

        public static Dictionary<int, string> ShangriLaWeapons
        {
            get
            {
                return m_ShangriLaWeapons;
            }
        }

        public static Dictionary<int, string> ShiNoNumaWeapons
        {
            get
            {
                return m_ShiNoNumaWeapons;
            }
        }

        public static Dictionary<int, string> VerrucktWeapons
        {
            get
            {
                return m_VerrucktWeapons;
            }
        }
        #endregion

        #region Methods
        public static int ReadInt(int _Address)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[24];
            ReadProcessMemory((int)m_ProcessHandle, _Address, buffer, buffer.Length, ref bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }
        public static bool WriteInt(int _Address, int _Value)
        {
            int lpNumberOfBytesWritten = 0;
            byte[] bytes = BitConverter.GetBytes(_Value);
            WriteProcessMemory((int)m_ProcessHandle, _Address, bytes, 4, ref lpNumberOfBytesWritten);
            return (lpNumberOfBytesWritten != 0);
        }

        public static bool IsGameClosed()
        {
            return BlackOpsProcess == null || BlackOpsProcess.HasExited || BlackOpsProcess.MainWindowHandle == IntPtr.Zero || BlackOpsHandle == null;
        }
        #endregion

    }
}
