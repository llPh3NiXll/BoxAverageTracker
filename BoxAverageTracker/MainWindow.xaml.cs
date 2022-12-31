using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace BoxAverageTracker
{
    public partial class MainWindow : Window
    {
        public static DependencyProperty BoxHitsCountProperty = DependencyProperty.Register("BoxHitsCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static DependencyProperty WeaponAveragesProperty = DependencyProperty.Register("WeaponAverages", typeof(ObservableCollection<KeyValuePair<string, float>>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<KeyValuePair<string, float>>()));
        public static DependencyProperty SelectedMapProperty = DependencyProperty.Register("SelectedMap", typeof(string), typeof(MainWindow), new PropertyMetadata(""));
        public static DependencyProperty TotalHeightProperty = DependencyProperty.Register("TotalHeight", typeof(int), typeof(MainWindow), new PropertyMetadata(100, SaveApplicationConfig));
        public static DependencyProperty TotalWidthProperty = DependencyProperty.Register("TotalWidth", typeof(int), typeof(MainWindow), new PropertyMetadata(275, SaveApplicationConfig));
        public static DependencyProperty AutoDisplayProperty = DependencyProperty.Register("AutoDisplay", typeof(bool), typeof(MainWindow), new PropertyMetadata(true, SaveApplicationConfig));
        public static DependencyProperty GameConnectedStringProperty = DependencyProperty.Register("GameConnectedString", typeof(string), typeof(MainWindow), new PropertyMetadata("Game not connected"));

        public static DependencyProperty TextFontFamilyProperty = DependencyProperty.Register("TextFontFamily", typeof(System.Windows.Media.FontFamily), typeof(MainWindow), new PropertyMetadata(new System.Windows.Media.FontFamily(), SaveApplicationConfig));
        public static DependencyProperty TextForegroundProperty = DependencyProperty.Register("TextForeground", typeof(System.Windows.Media.Brush), typeof(MainWindow), new PropertyMetadata(System.Windows.Media.Brushes.White, SaveApplicationConfig));
        public static DependencyProperty TextSizeProperty = DependencyProperty.Register("TextSize", typeof(float), typeof(MainWindow), new PropertyMetadata(30f, SaveApplicationConfig));
        public static DependencyProperty TextFontWeightProperty = DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(MainWindow), new PropertyMetadata(FontWeights.Normal, SaveApplicationConfig));
        public static DependencyProperty TextFontStyleProperty = DependencyProperty.Register("TextFontStyle", typeof(FontStyle), typeof(MainWindow), new PropertyMetadata(FontStyles.Normal, SaveApplicationConfig));

        public static DependencyProperty AverageTextFontFamilyProperty = DependencyProperty.Register("AverageTextFontFamily", typeof(System.Windows.Media.FontFamily), typeof(MainWindow), new PropertyMetadata(new System.Windows.Media.FontFamily(), SaveApplicationConfig));
        public static DependencyProperty AverageTextForegroundProperty = DependencyProperty.Register("AverageTextForeground", typeof(System.Windows.Media.Brush), typeof(MainWindow), new PropertyMetadata(System.Windows.Media.Brushes.White, SaveApplicationConfig));
        public static DependencyProperty AverageTextSizeProperty = DependencyProperty.Register("AverageTextSize", typeof(float), typeof(MainWindow), new PropertyMetadata(12.5f, SaveApplicationConfig));
        public static DependencyProperty AverageTextFontWeightProperty = DependencyProperty.Register("AverageTextFontWeight", typeof(FontWeight), typeof(MainWindow), new PropertyMetadata(FontWeights.Normal, SaveApplicationConfig));
        public static DependencyProperty AverageTextFontStyleProperty = DependencyProperty.Register("AverageTextFontStyle", typeof(FontStyle), typeof(MainWindow), new PropertyMetadata(FontStyles.Normal, SaveApplicationConfig));

        private CountWeapons countWeaponsWindow = null;

        public int BoxHitsCount
        {
            get => (int)GetValue(BoxHitsCountProperty);
            set => SetValue(BoxHitsCountProperty, value);
        }

        public ObservableCollection<KeyValuePair<string, float>> WeaponAverages
        {
            get => Dispatcher.Invoke(() => (ObservableCollection<KeyValuePair<string, float>>)GetValue(WeaponAveragesProperty));
            set => Dispatcher.Invoke(() => SetValue(WeaponAveragesProperty, value));
        }

        public string SelectedMap
        {
            get => Dispatcher.Invoke(() => (string)GetValue(SelectedMapProperty));
            set => Dispatcher.Invoke(() => SetValue(SelectedMapProperty, value));
        }

        public System.Windows.Media.FontFamily TextFontFamily
        {
            get => (System.Windows.Media.FontFamily)GetValue(TextFontFamilyProperty);
            set => SetValue(TextFontFamilyProperty, value);
        }

        public System.Windows.Media.Brush TextForeground
        {
            get => (System.Windows.Media.Brush)GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }

        public FontWeight TextFontWeight
        {
            get => (FontWeight)GetValue(TextFontWeightProperty);
            set => SetValue(TextFontWeightProperty, value);
        }

        public FontStyle TextFontStyle
        {
            get => (FontStyle)GetValue(TextFontStyleProperty);
            set => SetValue(TextFontStyleProperty, value);
        }

        public float TextSize
        {
            get => (float)GetValue(TextSizeProperty);
            set => SetValue(TextSizeProperty, value);
        }

        public bool AutoDisplay
        {
            get => Dispatcher.Invoke(() => (bool)GetValue(AutoDisplayProperty));
            set => Dispatcher.Invoke(() => SetValue(AutoDisplayProperty, value));
        }

        public System.Windows.Media.FontFamily AverageTextFontFamily
        {
            get => (System.Windows.Media.FontFamily)GetValue(AverageTextFontFamilyProperty);
            set => SetValue(AverageTextFontFamilyProperty, value);
        }

        public System.Windows.Media.Brush AverageTextForeground
        {
            get => (System.Windows.Media.Brush)GetValue(AverageTextForegroundProperty);
            set => SetValue(AverageTextForegroundProperty, value);
        }

        public float AverageTextSize
        {
            get => (float)GetValue(AverageTextSizeProperty);
            set => SetValue(AverageTextSizeProperty, value);
        }

        public FontWeight AverageTextFontWeight
        {
            get => (FontWeight)GetValue(AverageTextFontWeightProperty);
            set => SetValue(AverageTextFontWeightProperty, value);
        }

        public FontStyle AverageTextFontStyle
        {
            get => (FontStyle)GetValue(AverageTextFontStyleProperty);
            set => SetValue(AverageTextFontStyleProperty, value);
        }

        public int TotalHeight
        {
            get => (int)GetValue(TotalHeightProperty);
            set => SetValue(TotalHeightProperty, value);
        }

        public int TotalWidth
        {
            get => (int)GetValue(TotalWidthProperty);
            set => SetValue(TotalWidthProperty, value);
        }

        public string GameConnectedString
        {
            get => Dispatcher.Invoke(() => (string)GetValue(GameConnectedStringProperty));
            set => Dispatcher.Invoke(() => SetValue(GameConnectedStringProperty, value));
        }

        private Dictionary<string, List<int>> m_WeaponsGotten = new Dictionary<string, List<int>>();
        private Dictionary<string, int> m_WeaponsBoxHits = new Dictionary<string, int>();
        private List<string> m_WeaponsToDisplay = new List<string>();
        private List<string> m_WeaponsToNotCount = new List<string>();

        private List<int> m_WhiteInventory = new List<int>();
        private List<int> m_BlueInventory = new List<int>();
        private List<int> m_YellowInventory = new List<int>();
        private List<int> m_GreenInventory = new List<int>();

        private List<int> m_WhiteAmmos = new List<int>();
        private List<int> m_BlueAmmos = new List<int>();
        private List<int> m_YellowAmmos = new List<int>();
        private List<int> m_GreenAmmos = new List<int>();

        private List<int> m_WhiteTmpInventory = new List<int>();
        private List<int> m_BlueTmpInventory = new List<int>();
        private List<int> m_YellowTmpInventory = new List<int>();
        private List<int> m_GreenTmpInventory = new List<int>();

        private bool m_SetupDone = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            m_WeaponsBoxHits = new Dictionary<string, int>()
            {
                { "Python", 0 },
                { "CZ75", 0 },
                { "G11", 0 },
                { "FAMAS", 0 },
                { "Spectre", 0 },
                { "CZ75x2", 0 },
                { "SPAS-12", 0 },
                { "HS-10", 0 },
                { "AUG", 0 },
                { "Galil", 0 },
                { "Commando", 0 },
                { "FN FAL", 0 },
                { "Dragunov", 0 },
                { "L96A1", 0 },
                { "RPK", 0 },
                { "HK21", 0 },
                { "M72 LAW", 0 },
                { "China Lake", 0 },
                { "Gersh", 0 },
                { "Matrioshka", 0 },
                { "Ray Gun", 0 },
                { "Thundergun", 0 },
                { "Crossbow", 0 },
                { "Ballistic Knife", 0 },
                { "Scavenger", 0 },
                { "VR-11", 0 },
                { "Wunderwaffe DG-2", 0 },
                { "Winter's howl", 0 },
                { "QED", 0 },
                { "Wavegun", 0 },
                { "31-79 Jgb 215", 0 },
                { "Teddy bear", 0 },
            };

            m_WeaponsGotten = new Dictionary<string, List<int>>()
            {
                { "Python", new List<int>() },
                { "CZ75", new List<int>() },
                { "G11", new List<int>() },
                { "FAMAS", new List<int>() },
                { "Spectre", new List<int>() },
                { "CZ75x2", new List<int>() },
                { "SPAS-12", new List<int>() },
                { "HS-10", new List<int>() },
                { "AUG", new List<int>() },
                { "Galil", new List<int>() },
                { "Commando", new List<int>() },
                { "FN FAL", new List<int>() },
                { "Dragunov", new List<int>() },
                { "L96A1", new List<int>() },
                { "RPK", new List<int>() },
                { "HK21", new List<int>() },
                { "M72 LAW", new List<int>() },
                { "China Lake", new List<int>() },
                { "Gersh", new List<int>() },
                { "Matrioshka", new List<int>() },
                { "Ray Gun", new List<int>() },
                { "Thundergun", new List<int>() },
                { "Crossbow", new List<int>() },
                { "Ballistic Knife", new List<int>() },
                { "Scavenger", new List<int>() },
                { "VR-11", new List<int>() },
                { "Wunderwaffe DG-2", new List<int>() },
                { "Winter's howl", new List<int>() },
                { "QED", new List<int>() },
                { "Wavegun", new List<int>() },
                { "31-79 Jgb 215", new List<int>() },
                { "Teddy bear", new List<int>() },
            };

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
                hwndSource.CompositionTarget.RenderMode = RenderMode.SoftwareOnly;

            base.OnSourceInitialized(e);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (BoxHitsCount > 0 && m_SetupDone == true)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("You're closing the box hits tracker but it has registered box hits.\nDo you want to save the stats and the game before closing it ?", "Save & Exit", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveGame();
                    ExportGame();
                }
            }

            BlackOpsLibrary.m_ProcessHandle = null;
            BlackOpsLibrary.m_BlackOpsProcess = null;
            GameConnectedString = "Game not connected";

            if (Application.Current != null)
            {
                Application.Current.Shutdown();
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ListMaps.ContextMenu = ContextMenu = (ContextMenu)Resources["MainMenu"];
            LoadApplicationConfig();

            await UpdateTracking();
        }

        private async Task UpdateTracking()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (!BlackOpsLibrary.IsGameClosed())
                    {
                        bool isBoxRandomizing = false;

                        int timeSinceLastRandom = 0;
                        int previousTime = BlackOpsLibrary.ReadInt(Constants.C_TIMEADDRESS);
                        int previousMoonTeddy = BlackOpsLibrary.ReadInt(Constants.C_MOONTEDDYBEAR);

                        int whitePreviousPoints = BlackOpsLibrary.ReadInt(Constants.C_WHITEPOINTSADDRESS);
                        int whitePreviousKills = BlackOpsLibrary.ReadInt(Constants.C_WHITEKILLSADDRESS);
                        int whitePreviousDowns = BlackOpsLibrary.ReadInt(Constants.C_WHITEDOWNSADDRESS);
                        int whitePreviousRevives = BlackOpsLibrary.ReadInt(Constants.C_WHITEREVIVESADDRESS);

                        int bluePreviousPoints = BlackOpsLibrary.ReadInt(Constants.C_BLUEPOINTSADDRESS);
                        int bluePreviousKills = BlackOpsLibrary.ReadInt(Constants.C_BLUEKILLSADDRESS);
                        int bluePreviousDowns = BlackOpsLibrary.ReadInt(Constants.C_BLUEDOWNSADDRESS);
                        int bluePreviousRevives = BlackOpsLibrary.ReadInt(Constants.C_BLUEREVIVESADDRESS);

                        int yellowPreviousPoints = BlackOpsLibrary.ReadInt(Constants.C_YELLOWPOINTSADDRESS);
                        int yellowPreviousKills = BlackOpsLibrary.ReadInt(Constants.C_YELLOWKILLSADDRESS);
                        int yellowPreviousDowns = BlackOpsLibrary.ReadInt(Constants.C_YELLOWDOWNSADDRESS);
                        int yellowPreviousRevives = BlackOpsLibrary.ReadInt(Constants.C_YELLOWREVIVESADDRESS);

                        int greenPreviousPoints = BlackOpsLibrary.ReadInt(Constants.C_GREENPOINTSADDRESS);
                        int greenPreviousKills = BlackOpsLibrary.ReadInt(Constants.C_GREENKILLSADDRESS);
                        int greenPreviousDowns = BlackOpsLibrary.ReadInt(Constants.C_GREENDOWNSADDRESS);
                        int greenPreviousRevives = BlackOpsLibrary.ReadInt(Constants.C_GREENREVIVESADDRESS);

                        InitializeInventories();

                        while (BlackOpsLibrary.m_BlackOpsProcess != null && !BlackOpsLibrary.m_BlackOpsProcess.HasExited)
                        {
                            try
                            {
                                GameConnectedString = string.Empty;

                                int timeCount = BlackOpsLibrary.ReadInt(Constants.C_TIMEADDRESS);
                                int moonTeddy = BlackOpsLibrary.ReadInt(Constants.C_MOONTEDDYBEAR);
                                int mapId = BlackOpsLibrary.ReadInt(Constants.C_MAPADDRESS);

                                bool mustReset = timeCount < 50 || string.IsNullOrEmpty(SelectedMap);

                                if (mustReset)
                                {
                                    UpdateSelectedMap(mapId);

                                    var weaponAverages = new ObservableCollection<KeyValuePair<string, float>>();
                                    foreach (var wtd in m_WeaponsToDisplay)
                                    {
                                        float divider = (float)m_WeaponsGotten[wtd].Count > 0 ? (float)m_WeaponsGotten[wtd].Count : 1;
                                        weaponAverages.Add(new KeyValuePair<string, float>(wtd, ((float)m_WeaponsBoxHits[wtd]) / (float)divider));
                                    }
                                    WeaponAverages = weaponAverages;
                                    ResetTracking();
                                }

                                else if (timeCount != previousTime && BlackOpsLibrary.ReadInt(Constants.C_MAPADDRESS) != 5)
                                {
                                    int whitePointsCount = BlackOpsLibrary.ReadInt(Constants.C_WHITEPOINTSADDRESS);
                                    int whiteKillsCount = BlackOpsLibrary.ReadInt(Constants.C_WHITEKILLSADDRESS);
                                    int whiteDownsCount = BlackOpsLibrary.ReadInt(Constants.C_WHITEDOWNSADDRESS);
                                    int whiteRevivesCount = BlackOpsLibrary.ReadInt(Constants.C_WHITEREVIVESADDRESS);

                                    int bluePointsCount = BlackOpsLibrary.ReadInt(Constants.C_BLUEPOINTSADDRESS);
                                    int blueKillsCount = BlackOpsLibrary.ReadInt(Constants.C_BLUEKILLSADDRESS);
                                    int blueDownsCount = BlackOpsLibrary.ReadInt(Constants.C_BLUEDOWNSADDRESS);
                                    int blueRevivesCount = BlackOpsLibrary.ReadInt(Constants.C_BLUEREVIVESADDRESS);

                                    int yellowPointsCount = BlackOpsLibrary.ReadInt(Constants.C_YELLOWPOINTSADDRESS);
                                    int yellowKillsCount = BlackOpsLibrary.ReadInt(Constants.C_YELLOWKILLSADDRESS);
                                    int yellowDownsCount = BlackOpsLibrary.ReadInt(Constants.C_YELLOWDOWNSADDRESS);
                                    int yellowRevivesCount = BlackOpsLibrary.ReadInt(Constants.C_YELLOWREVIVESADDRESS);

                                    int greenPointsCount = BlackOpsLibrary.ReadInt(Constants.C_GREENPOINTSADDRESS);
                                    int greenKillsCount = BlackOpsLibrary.ReadInt(Constants.C_GREENKILLSADDRESS);
                                    int greenDownsCount = BlackOpsLibrary.ReadInt(Constants.C_GREENDOWNSADDRESS);
                                    int greenRevivesCount = BlackOpsLibrary.ReadInt(Constants.C_GREENREVIVESADDRESS);

                                    bool mustUpdate = false;

                                    bool whiteHasHit = HasHitBox(whitePreviousPoints, whitePointsCount, whitePreviousKills, whiteKillsCount, whitePreviousDowns, whiteDownsCount);
                                    bool blueHasHit = HasHitBox(bluePreviousPoints, bluePointsCount, bluePreviousKills, blueKillsCount, bluePreviousDowns, blueDownsCount);
                                    bool yellowHasHit = HasHitBox(yellowPreviousPoints, yellowPointsCount, yellowPreviousKills, yellowKillsCount, yellowPreviousDowns, yellowDownsCount);
                                    bool greenHasHit = HasHitBox(greenPreviousPoints, greenPointsCount, greenPreviousKills, greenKillsCount, greenPreviousDowns, greenDownsCount);
                                    // bool hasMoonBoxMoved = false;
                                    bool inventoryHasChanged = HasWhiteInventoryChanged() || HasBlueInventoryChanged() || HasYellowInventoryChanged() || HasGreenInventoryChanged();
                                    bool ammoBought = HasWhiteBoughtAmmo() || HasBlueBoughtAmmo() || HasYellowBoughtAmmo() || HasGreenBoughtAmmo();

                                    if (ammoBought)
                                    {
                                        UpdateAmmos();
                                    }

                                    if (whiteHasHit || blueHasHit || yellowHasHit || greenHasHit)
                                    {
                                        if (!inventoryHasChanged && !ammoBought)
                                        {
                                            SetBoxHits(GetBoxHits() + 1);
                                            isBoxRandomizing = true;
                                            timeSinceLastRandom = 0;

                                            m_WhiteTmpInventory = new List<int>(m_WhiteInventory);
                                            m_BlueTmpInventory = new List<int>(m_BlueInventory);
                                            m_YellowTmpInventory = new List<int>(m_YellowInventory);
                                            m_GreenTmpInventory = new List<int>(m_GreenInventory);
                                            if (m_SetupDone)
                                            {
                                                AddBoxHitForEachWeapon(whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                                            }
                                        }
                                        mustUpdate = true;
                                    }

                                    if (HasGottenTeddyBear(whitePreviousPoints, whitePointsCount, whitePreviousKills, whiteKillsCount, whitePreviousRevives, whiteRevivesCount) ||
                                        HasGottenTeddyBear(bluePreviousPoints, bluePointsCount, bluePreviousKills, blueKillsCount, bluePreviousRevives, blueRevivesCount) ||
                                        HasGottenTeddyBear(yellowPreviousPoints, yellowPointsCount, yellowPreviousKills, yellowKillsCount, yellowPreviousRevives, yellowRevivesCount) ||
                                        HasGottenTeddyBear(greenPreviousPoints, greenPointsCount, greenPreviousKills, greenKillsCount, greenPreviousRevives, greenRevivesCount) &&
                                        m_SetupDone)
                                    {
                                        if (!m_WeaponsToNotCount.Any(x => x == "Teddy bear"))
                                        {
                                            m_WeaponsGotten["Teddy bear"].Add(GetBoxHits());
                                            mustUpdate = true;
                                        }
                                    }

                                    //if (hasMoonBoxMoved)
                                    //{
                                    //    if (!m_WeaponsToNotCount.Any(x => x == "Teddy bear"))
                                    //    {
                                    //        m_WeaponsGotten["Teddy bear"].Add(GetBoxHits());
                                    //        mustUpdate = true;
                                    //    }
                                    //}

                                    if (inventoryHasChanged)
                                    {
                                        if (m_SetupDone)
                                        {
                                            UpdateChangedWeapon();
                                            mustUpdate = true;
                                        }
                                        UpdateInventories();
                                    }

                                    if (isBoxRandomizing && timeSinceLastRandom >= 70)
                                    {
                                        isBoxRandomizing = false;
                                        if (m_SetupDone)
                                        {
                                            var diffWhiteInventory = m_WhiteTmpInventory.Where(p => !m_WhiteInventory.Any(p2 => p2 == p));
                                            var diffBlueInventory = m_BlueTmpInventory.Where(p => !m_BlueInventory.Any(p2 => p2 == p));
                                            var diffYellowInventory = m_YellowTmpInventory.Where(p => !m_YellowInventory.Any(p2 => p2 == p));
                                            var diffGreenInventory = m_GreenTmpInventory.Where(p => !m_GreenInventory.Any(p2 => p2 == p));

                                            foreach (var diff in diffWhiteInventory)
                                            {
                                                AddBoxHitForSpecificWeapon(diff);
                                                mustUpdate = true;
                                            }

                                            foreach (var diff in diffBlueInventory)
                                            {
                                                AddBoxHitForSpecificWeapon(diff);
                                                mustUpdate = true;
                                            }

                                            foreach (var diff in diffYellowInventory)
                                            {
                                                AddBoxHitForSpecificWeapon(diff);
                                                mustUpdate = true;
                                            }

                                            foreach (var diff in diffGreenInventory)
                                            {
                                                AddBoxHitForSpecificWeapon(diff);
                                                mustUpdate = true;
                                            }
                                        }
                                        var weaponAverages = WeaponAverages.ToList();
                                        WeaponAverages = new ObservableCollection<KeyValuePair<string, float>>(weaponAverages);
                                    }

                                    if (mustUpdate)
                                    {
                                        var weaponAverages = WeaponAverages.ToList();

                                        for (int i = 0; i < weaponAverages.Count; ++i)
                                        {
                                            var weapon = weaponAverages[i];
                                            float divider = (float)m_WeaponsGotten[weapon.Key].Count > 0 ? (float)m_WeaponsGotten[weapon.Key].Count : 1;
                                            var copyWeapon = new KeyValuePair<string, float>(weapon.Key, ((float)m_WeaponsBoxHits[weapon.Key]) / (float)divider);
                                            weaponAverages[i] = copyWeapon;
                                        }

                                        WeaponAverages = new ObservableCollection<KeyValuePair<string, float>>(weaponAverages);
                                    }

                                    previousTime = timeCount;
                                    whitePreviousPoints = whitePointsCount;
                                    whitePreviousKills = whiteKillsCount;
                                    whitePreviousDowns = whiteDownsCount;
                                    whitePreviousRevives = whiteRevivesCount;

                                    bluePreviousPoints = bluePointsCount;
                                    bluePreviousKills = blueKillsCount;
                                    bluePreviousDowns = blueDownsCount;
                                    bluePreviousRevives = blueRevivesCount;

                                    yellowPreviousPoints = yellowPointsCount;
                                    yellowPreviousKills = yellowKillsCount;
                                    yellowPreviousDowns = yellowDownsCount;
                                    yellowPreviousRevives = yellowRevivesCount;

                                    greenPreviousPoints = greenPointsCount;
                                    greenPreviousKills = greenKillsCount;
                                    greenPreviousDowns = greenDownsCount;
                                    greenPreviousRevives = greenRevivesCount;

                                    previousMoonTeddy = moonTeddy;
                                    timeSinceLastRandom++;

                                    UpdateAmmos();
                                }

                                Thread.Sleep(50);
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        BlackOpsLibrary.m_ProcessHandle = null;
                        BlackOpsLibrary.m_BlackOpsProcess = null;
                        GameConnectedString = "Game not connected";
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        private void ResetTracking()
        {
            SetBoxHits(0);
            m_SetupDone = false;
            var tmpWeaponsGotten = new Dictionary<string, List<int>>(m_WeaponsGotten);
            foreach (var x in m_WeaponsGotten)
            {
                tmpWeaponsGotten[x.Key] = new List<int>();
            }
            m_WeaponsGotten = tmpWeaponsGotten;

            var tmpWeaponsHits = new Dictionary<string, int>(m_WeaponsBoxHits);
            foreach (var x in m_WeaponsBoxHits)
            {
                tmpWeaponsHits[x.Key] = 0;
            }
            m_WeaponsBoxHits = tmpWeaponsHits;

            var weaponsAverages = WeaponAverages.ToList();
            for (int i = 0; i < weaponsAverages.Count; ++i)
            {
                weaponsAverages[i] = new KeyValuePair<string, float>(weaponsAverages[i].Key, 0);
            }
            WeaponAverages = new ObservableCollection<KeyValuePair<string, float>>(weaponsAverages);
        }

        private void UpdateSelectedMap(int mapId)
        {
            switch (mapId)
            {
                case 3:
                    if (SelectedMap != "Nacht Der Untoten")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Nacht Der Untoten");
                        if (AutoDisplay)
                            m_WeaponsToDisplay.Add("Thundergun");
                    }
                    break;
                case 5:
                    m_WeaponsToDisplay = new List<string>();
                    SetSelectedMap("MainMenu");
                    break;
                case 37:
                    if (SelectedMap != "Kino der Toten")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Kino der Toten");
                        if (AutoDisplay)
                            m_WeaponsToDisplay.Add("Thundergun");
                    }
                    break;
                case 50:
                    if (SelectedMap != "Verruckt")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Verruckt");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Ray Gun");
                            m_WeaponsToDisplay.Add("Winter's howl");
                            m_WeaponsToDisplay.Add("M72 LAW");
                        }
                    }
                    break;
                case 77:
                    if (SelectedMap != "Five")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Five");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Ray Gun");
                            m_WeaponsToDisplay.Add("Winter's howl");
                            m_WeaponsToDisplay.Add("M72 LAW");
                        }
                    }
                    break;
                case 85:
                    if (SelectedMap != "Call of the Dead")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Call of the Dead");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("VR-11");
                            m_WeaponsToDisplay.Add("Scavenger");
                            m_WeaponsToDisplay.Add("Ray Gun");
                            m_WeaponsToDisplay.Add("M72 LAW");
                            m_WeaponsToDisplay.Add("Crossbow");
                        }
                    }
                    break;
                case 90:
                    if (SelectedMap != "Der Riese")
                    {
                        SetSelectedMap("Der Riese");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Wunderwaffe DG-2");
                        }
                    }
                    break;
                case 99:
                    if (SelectedMap != "Shi No Numa")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Shi No Numa");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Wunderwaffe DG-2");
                        }
                    }
                    break;
                case 129:
                    if (SelectedMap != "Shangri-la")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Shangri-la");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("31-79 Jgb 215");
                        }
                    }
                    break;
                case 163:
                    if (SelectedMap != "Moon")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Moon");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Wavegun");
                            m_WeaponsToDisplay.Add("Gersh");
                            m_WeaponsToDisplay.Add("QED");
                        }
                    }
                    break;
                case 189:
                    if (SelectedMap != "Ascension")
                    {
                        m_WeaponsToDisplay = new List<string>();
                        SetSelectedMap("Ascension");
                        if (AutoDisplay)
                        {
                            m_WeaponsToDisplay.Add("Thundergun");
                            m_WeaponsToDisplay.Add("Gersh");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void InitializeInventories()
        {
            m_WhiteInventory = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT4),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT5),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT6),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT7),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT8),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT9),
                    };
            m_BlueInventory = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT4),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT5),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT6),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT7),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT8),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT9),
                    };
            m_YellowInventory = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT4),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT5),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT6),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT7),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT8),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT9),
                    };
            m_GreenInventory = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT4),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT5),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT6),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT7),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT8),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT9),
                    };


            m_WhiteAmmos = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1_2),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2_2),
                        BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3_2),
                    };
            m_BlueAmmos = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1_2),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2_2),
                        BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3_2),
                    };
            m_YellowAmmos = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1_2),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2_2),
                        BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3_2),
                    };
            m_GreenAmmos = new List<int>
                    {
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1_2),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2_2),
                        BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3_2),
                    };
        }

        private void AddBoxHitForSpecificWeapon(int weaponKey)
        {
            string weapon = string.Empty;

            switch (SelectedMap)
            {
                case "Ascension":
                    weapon = BlackOpsLibrary.AscensionWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Call of the Dead":
                    weapon = BlackOpsLibrary.CallOfTheDeadWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Der Riese":
                    weapon = BlackOpsLibrary.DerRieseWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Five":
                    weapon = BlackOpsLibrary.FiveWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Kino der Toten":
                    weapon = BlackOpsLibrary.KinoDerTotenWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Moon":
                    weapon = BlackOpsLibrary.MoonWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Nacht Der Untoten":
                    weapon = BlackOpsLibrary.NachtDerUntotenWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Shangri-la":
                    weapon = BlackOpsLibrary.ShangriLaWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Shi No Numa":
                    weapon = BlackOpsLibrary.ShiNoNumaWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                case "Verruckt":
                    weapon = BlackOpsLibrary.VerrucktWeapons[weaponKey];
                    if (!m_WeaponsToNotCount.Any(x => x == weapon))
                    {
                        m_WeaponsBoxHits[weapon]++;
                    }
                    break;
                default:
                    break;
            }
        }

        private static void SaveApplicationConfig(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)d;
            mainWindow.SaveApplicationConfig();
        }

        private bool HasHitBox(int previousPoints, int pointsCount, int previousKills, int killsCount, int previousDowns, int downsCount)
        {
            return previousDowns == downsCount && ((previousPoints - pointsCount >= 920 && previousPoints - pointsCount <= 950) || (previousPoints - pointsCount >= 880 && previousPoints - pointsCount <= 900 && killsCount - previousKills == 1) ||
                   (previousPoints - pointsCount == 10) ||
                   (SelectedMap == "Moon" && (previousPoints - pointsCount == 600 || previousPoints - pointsCount == 1200)));
        }

        private bool HasGottenTeddyBear(int previousPoints, int pointsCount, int previousKills, int killsCount, int previousRevives, int revivesCount)
        {
            return (previousPoints - pointsCount == -950 && killsCount - previousKills == 0 && SelectedMap != "Moon" && previousRevives == revivesCount);
        }

        private void AddBoxHitForEachWeapon(bool whiteHasHit, bool blueHasHit, bool yellowHasHit, bool greenHasHit)
        {
            switch (SelectedMap)
            {
                case "Ascension":
                    ManageMapAverages(BlackOpsLibrary.AscensionWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Call of the Dead":
                    ManageMapAverages(BlackOpsLibrary.CallOfTheDeadWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Der Riese":
                    ManageMapAverages(BlackOpsLibrary.DerRieseWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Five":
                    ManageMapAverages(BlackOpsLibrary.FiveWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Kino der Toten":
                    ManageMapAverages(BlackOpsLibrary.KinoDerTotenWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Moon":
                    ManageMapAverages(BlackOpsLibrary.MoonWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Nacht Der Untoten":
                    ManageMapAverages(BlackOpsLibrary.NachtDerUntotenWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Shangri-la":
                    ManageMapAverages(BlackOpsLibrary.ShangriLaWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Shi No Numa":
                    ManageMapAverages(BlackOpsLibrary.ShiNoNumaWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                case "Verruckt":
                    ManageMapAverages(BlackOpsLibrary.VerrucktWeapons, whiteHasHit, blueHasHit, yellowHasHit, greenHasHit);
                    break;
                default:
                    break;
            }
        }

        private void ManageMapAverages(Dictionary<int, string> weaponsDictionary, bool whiteHasHit, bool blueHasHit, bool yellowHasHit, bool greenHasHit)
        {
            List<string> alreadyCheckedWeapons = new List<string>();

            foreach (var weapon in weaponsDictionary)
            {
                string weaponName = weaponsDictionary[weapon.Key];

                if (!alreadyCheckedWeapons.Any(x => x == weapon.Value))
                {
                    alreadyCheckedWeapons.Add(weapon.Value);

                    if ((!m_WhiteInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                                              weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                        !m_BlueInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                                              weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                        !m_YellowInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                                              weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                        !m_GreenInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                                              weaponsDictionary[x] == weaponsDictionary[weapon.Key])) &&
                        (weaponsDictionary[weapon.Key] == "Thundergun" || weaponsDictionary[weapon.Key] == "Matrioshka" ||
                         weaponsDictionary[weapon.Key] == "Crossbow" || weaponsDictionary[weapon.Key] == "Ballistic Knife" ||
                         weaponsDictionary[weapon.Key] == "Scavenger" || weaponsDictionary[weapon.Key] == "VR-11" ||
                         weaponsDictionary[weapon.Key] == "Wunderwaffe DG-2" || weaponsDictionary[weapon.Key] == "Winter's howl" ||
                         weaponsDictionary[weapon.Key] == "Wavegun" || weaponsDictionary[weapon.Key] == "31-79 Jgb 215"))
                    {
                        if (!m_WeaponsToNotCount.Any(x => x == weaponName))
                        {
                            m_WeaponsBoxHits[weaponName]++;
                        }
                    }

                    else if (!m_WhiteInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                               weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                               whiteHasHit &&
                              (weaponsDictionary[weapon.Key] != "Thundergun" && weaponsDictionary[weapon.Key] != "Matrioshka" &&
                               weaponsDictionary[weapon.Key] != "Crossbow" && weaponsDictionary[weapon.Key] != "Ballistic Knife" &&
                               weaponsDictionary[weapon.Key] != "Scavenger" && weaponsDictionary[weapon.Key] != "VR-11" &&
                               weaponsDictionary[weapon.Key] != "Wunderwaffe DG-2" && weaponsDictionary[weapon.Key] != "Winter's howl" &&
                               weaponsDictionary[weapon.Key] != "Wavegun" && weaponsDictionary[weapon.Key] != "31-79 Jgb 215"))
                    {
                        if (!m_WeaponsToNotCount.Any(x => x == weaponName))
                        {
                            m_WeaponsBoxHits[weaponName]++;
                        }
                    }

                    else if (!m_BlueInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                               weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                               blueHasHit &&
                              (weaponsDictionary[weapon.Key] != "Thundergun" && weaponsDictionary[weapon.Key] != "Matrioshka" &&
                               weaponsDictionary[weapon.Key] != "Crossbow" && weaponsDictionary[weapon.Key] != "Ballistic Knife" &&
                               weaponsDictionary[weapon.Key] != "Scavenger" && weaponsDictionary[weapon.Key] != "VR-11" &&
                               weaponsDictionary[weapon.Key] != "Wunderwaffe DG-2" && weaponsDictionary[weapon.Key] != "Winter's howl" &&
                               weaponsDictionary[weapon.Key] != "Wavegun" && weaponsDictionary[weapon.Key] != "31-79 Jgb 215"))
                    {
                        if (!m_WeaponsToNotCount.Any(x => x == weaponName))
                        {
                            m_WeaponsBoxHits[weaponName]++;
                        }
                    }

                    else if (!m_YellowInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                               weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                               yellowHasHit &&
                              (weaponsDictionary[weapon.Key] != "Thundergun" && weaponsDictionary[weapon.Key] != "Matrioshka" &&
                               weaponsDictionary[weapon.Key] != "Crossbow" && weaponsDictionary[weapon.Key] != "Ballistic Knife" &&
                               weaponsDictionary[weapon.Key] != "Scavenger" && weaponsDictionary[weapon.Key] != "VR-11" &&
                               weaponsDictionary[weapon.Key] != "Wunderwaffe DG-2" && weaponsDictionary[weapon.Key] != "Winter's howl" &&
                               weaponsDictionary[weapon.Key] != "Wavegun" && weaponsDictionary[weapon.Key] != "31-79 Jgb 215"))
                    {
                        if (!m_WeaponsToNotCount.Any(x => x == weaponName))
                        {
                            m_WeaponsBoxHits[weaponName]++;
                        }
                    }

                    else if (!m_GreenInventory.Any(x => weaponsDictionary.ContainsKey(x) &&
                               weaponsDictionary[x] == weaponsDictionary[weapon.Key]) &&
                               greenHasHit &&
                              (weaponsDictionary[weapon.Key] != "Thundergun" && weaponsDictionary[weapon.Key] != "Matrioshka" &&
                               weaponsDictionary[weapon.Key] != "Crossbow" && weaponsDictionary[weapon.Key] != "Ballistic Knife" &&
                               weaponsDictionary[weapon.Key] != "Scavenger" && weaponsDictionary[weapon.Key] != "VR-11" &&
                               weaponsDictionary[weapon.Key] != "Wunderwaffe DG-2" && weaponsDictionary[weapon.Key] != "Winter's howl" &&
                               weaponsDictionary[weapon.Key] != "Wavegun" && weaponsDictionary[weapon.Key] != "31-79 Jgb 215"))
                    {
                        if (!m_WeaponsToNotCount.Any(x => x == weaponName))
                        {
                            m_WeaponsBoxHits[weaponName]++;
                        }
                    }
                }
            }

            if (SelectedMap != "Nacht Der Untoten" && SelectedMap != "Moon" && SelectedMap != "MainMenu")
            {
                if (!m_WeaponsToNotCount.Any(x => x == "Teddy bear"))
                {
                    m_WeaponsBoxHits["Teddy bear"]++;
                }
            }
        }

        private bool HasWhiteInventoryChanged()
        {
            return m_WhiteInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT1) ||
                   m_WhiteInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT2) ||
                   m_WhiteInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT3) ||
                   m_WhiteInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT4) ||
                   m_WhiteInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT5) ||
                   m_WhiteInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT6) ||
                   m_WhiteInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT7) ||
                   m_WhiteInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT8) ||
                   m_WhiteInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT9);
        }

        private bool HasBlueInventoryChanged()
        {
            return m_BlueInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT1) ||
                   m_BlueInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT2) ||
                   m_BlueInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT3) ||
                   m_BlueInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT4) ||
                   m_BlueInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT5) ||
                   m_BlueInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT6) ||
                   m_BlueInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT7) ||
                   m_BlueInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT8) ||
                   m_BlueInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT9);
        }

        private bool HasYellowInventoryChanged()
        {
            return m_YellowInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT1) ||
                   m_YellowInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT2) ||
                   m_YellowInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT3) ||
                   m_YellowInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT4) ||
                   m_YellowInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT5) ||
                   m_YellowInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT6) ||
                   m_YellowInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT7) ||
                   m_YellowInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT8) ||
                   m_YellowInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT9);
        }

        private bool HasGreenInventoryChanged()
        {
            return m_GreenInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT1) ||
                   m_GreenInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT2) ||
                   m_GreenInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT3) ||
                   m_GreenInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT4) ||
                   m_GreenInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT5) ||
                   m_GreenInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT6) ||
                   m_GreenInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT7) ||
                   m_GreenInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT8) ||
                   m_GreenInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT9);
        }

        private bool HasWhiteBoughtAmmo()
        {
            return m_WhiteAmmos[0] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1) ||
                   m_WhiteAmmos[1] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2) ||
                   m_WhiteAmmos[2] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3) ||
                   m_WhiteAmmos[3] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1_2) ||
                   m_WhiteAmmos[4] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2_2) ||
                   m_WhiteAmmos[5] < BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3_2);
        }

        private bool HasBlueBoughtAmmo()
        {
            return m_BlueAmmos[0] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1) ||
                   m_BlueAmmos[1] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2) ||
                   m_BlueAmmos[2] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3) ||
                   m_BlueAmmos[3] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1_2) ||
                   m_BlueAmmos[4] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2_2) ||
                   m_BlueAmmos[5] < BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3_2);
        }

        private bool HasYellowBoughtAmmo()
        {
            return m_YellowAmmos[0] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1) ||
                   m_YellowAmmos[1] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2) ||
                   m_YellowAmmos[2] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3) ||
                   m_YellowAmmos[3] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1_2) ||
                   m_YellowAmmos[4] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2_2) ||
                   m_YellowAmmos[5] < BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3_2);
        }

        private bool HasGreenBoughtAmmo()
        {
            return m_GreenAmmos[0] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1) ||
                   m_GreenAmmos[1] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2) ||
                   m_GreenAmmos[2] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3) ||
                   m_GreenAmmos[3] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1_2) ||
                   m_GreenAmmos[4] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2_2) ||
                   m_GreenAmmos[5] < BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3_2);
        }

        private void UpdateAmmos()
        {
            m_WhiteAmmos[0] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1);
            m_WhiteAmmos[1] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2);
            m_WhiteAmmos[2] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3);
            m_WhiteAmmos[3] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT1_2);
            m_WhiteAmmos[4] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT2_2);
            m_WhiteAmmos[5] = BlackOpsLibrary.ReadInt(Constants.C_WHITEAMMOSSLOT3_2);

            m_BlueAmmos[0] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1);
            m_BlueAmmos[1] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2);
            m_BlueAmmos[2] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3);
            m_BlueAmmos[3] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT1_2);
            m_BlueAmmos[4] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT2_2);
            m_BlueAmmos[5] = BlackOpsLibrary.ReadInt(Constants.C_BLUEAMMOSSLOT3_2);

            m_YellowAmmos[0] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1);
            m_YellowAmmos[1] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2);
            m_YellowAmmos[2] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3);
            m_YellowAmmos[3] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT1_2);
            m_YellowAmmos[4] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT2_2);
            m_YellowAmmos[5] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWAMMOSSLOT3_2);

            m_GreenAmmos[0] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1);
            m_GreenAmmos[1] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2);
            m_GreenAmmos[2] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3);
            m_GreenAmmos[3] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT1_2);
            m_GreenAmmos[4] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT2_2);
            m_GreenAmmos[5] = BlackOpsLibrary.ReadInt(Constants.C_GREENAMMOSSLOT3_2);
        }

        private void UpdateChangedWeapon()
        {
            int changedWeaponId = -1;

            if (m_WhiteInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT1))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT1);
            }
            else if (m_WhiteInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT2))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT2);
            }
            else if (m_WhiteInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT3))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT3);
            }
            else if (m_WhiteInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT4))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT4);
            }
            else if (m_WhiteInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT5))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT5);
            }
            else if (m_WhiteInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT6))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT6);
            }
            else if (m_WhiteInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT7))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT7);
            }
            else if (m_WhiteInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT8))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT8);
            }
            else if (m_WhiteInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT9))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT9);
            }


            else if (m_BlueInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT1))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT1);
            }
            else if (m_BlueInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT2))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT2);
            }
            else if (m_BlueInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT3))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT3);
            }
            else if (m_BlueInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT4))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT4);
            }
            else if (m_BlueInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT5))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT5);
            }
            else if (m_BlueInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT6))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT6);
            }
            else if (m_BlueInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT7))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT7);
            }
            else if (m_BlueInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT8))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT8);
            }
            else if (m_BlueInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT9))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT9);
            }


            else if (m_YellowInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT1))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT1);
            }
            else if (m_YellowInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT2))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT2);
            }
            else if (m_YellowInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT3))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT3);
            }
            else if (m_YellowInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT4))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT4);
            }
            else if (m_YellowInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT5))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT5);
            }
            else if (m_YellowInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT6))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT6);
            }
            else if (m_YellowInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT7))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT7);
            }
            else if (m_YellowInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT8))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT8);
            }
            else if (m_YellowInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT9))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT9);
            }


            else if (m_GreenInventory[0] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT1))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT1);
            }
            else if (m_GreenInventory[1] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT2))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT2);
            }
            else if (m_GreenInventory[2] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT3))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT3);
            }
            else if (m_GreenInventory[3] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT4))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT4);
            }
            else if (m_GreenInventory[4] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT5))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT5);
            }
            else if (m_GreenInventory[5] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT6))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT6);
            }
            else if (m_GreenInventory[6] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT7))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT7);
            }
            else if (m_GreenInventory[7] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT8))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT8);
            }
            else if (m_GreenInventory[8] != BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT9))
            {
                changedWeaponId = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT9);
            }

            if (SelectedMap == "Ascension")
            {
                if (BlackOpsLibrary.AscensionWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 8 ||
                        changedWeaponId == 10 ||
                        changedWeaponId == 17 ||
                        changedWeaponId == 19 ||
                        changedWeaponId == 30 ||
                        changedWeaponId == 34 ||
                        changedWeaponId == 40 ||
                        changedWeaponId == 43 ||
                        changedWeaponId == 45 ||
                        changedWeaponId == 48 ||
                        changedWeaponId == 50 ||
                        changedWeaponId == 52 ||
                        changedWeaponId == 54 ||
                        changedWeaponId == 56 ||
                        changedWeaponId == 58 ||
                        changedWeaponId == 60 ||
                        changedWeaponId == 62 ||
                        changedWeaponId == 64 ||
                        changedWeaponId == 68 ||
                        changedWeaponId == 70 ||
                        changedWeaponId == 72 ||
                        changedWeaponId == 74 ||
                        changedWeaponId == 76;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.AscensionWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Call of the Dead")
            {
                if (BlackOpsLibrary.CallOfTheDeadWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 9 ||
                        changedWeaponId == 11 ||
                        changedWeaponId == 18 ||
                        changedWeaponId == 20 ||
                        changedWeaponId == 31 ||
                        changedWeaponId == 36 ||
                        changedWeaponId == 43 ||
                        changedWeaponId == 46 ||
                        changedWeaponId == 48 ||
                        changedWeaponId == 51 ||
                        changedWeaponId == 53 ||
                        changedWeaponId == 55 ||
                        changedWeaponId == 57 ||
                        changedWeaponId == 59 ||
                        changedWeaponId == 61 ||
                        changedWeaponId == 63 ||
                        changedWeaponId == 65 ||
                        changedWeaponId == 67 ||
                        changedWeaponId == 69 ||
                        changedWeaponId == 71 ||
                        changedWeaponId == 73 ||
                        changedWeaponId == 75 ||
                        changedWeaponId == 78 ||
                        changedWeaponId == 80;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.CallOfTheDeadWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Der Riese")
            {
                if (BlackOpsLibrary.DerRieseWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 31 ||
                        changedWeaponId == 32 ||
                        changedWeaponId == 33 ||
                        changedWeaponId == 34 ||
                        changedWeaponId == 35 ||
                        changedWeaponId == 37 ||
                        changedWeaponId == 38 ||
                        changedWeaponId == 40 ||
                        changedWeaponId == 41 ||
                        changedWeaponId == 43 ||
                        changedWeaponId == 44 ||
                        changedWeaponId == 45 ||
                        changedWeaponId == 46 ||
                        changedWeaponId == 47 ||
                        changedWeaponId == 48 ||
                        changedWeaponId == 49 ||
                        changedWeaponId == 50 ||
                        changedWeaponId == 51 ||
                        changedWeaponId == 52 ||
                        changedWeaponId == 53 ||
                        changedWeaponId == 54 ||
                        changedWeaponId == 77 ||
                        changedWeaponId == 79;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.DerRieseWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Five")
            {
                if (BlackOpsLibrary.FiveWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 9 ||
                        changedWeaponId == 11 ||
                        changedWeaponId == 18 ||
                        changedWeaponId == 20 ||
                        changedWeaponId == 31 ||
                        changedWeaponId == 35 ||
                        changedWeaponId == 41 ||
                        changedWeaponId == 44 ||
                        changedWeaponId == 46 ||
                        changedWeaponId == 49 ||
                        changedWeaponId == 51 ||
                        changedWeaponId == 53 ||
                        changedWeaponId == 55 ||
                        changedWeaponId == 57 ||
                        changedWeaponId == 59 ||
                        changedWeaponId == 61 ||
                        changedWeaponId == 63 ||
                        changedWeaponId == 65 ||
                        changedWeaponId == 68 ||
                        changedWeaponId == 70 ||
                        changedWeaponId == 73 ||
                        changedWeaponId == 75 ||
                        changedWeaponId == 77;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.FiveWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Kino der Toten")
            {
                if (BlackOpsLibrary.KinoDerTotenWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 10 ||
                        changedWeaponId == 12 ||
                        changedWeaponId == 19 ||
                        changedWeaponId == 21 ||
                        changedWeaponId == 34 ||
                        changedWeaponId == 38 ||
                        changedWeaponId == 44 ||
                        changedWeaponId == 47 ||
                        changedWeaponId == 49 ||
                        changedWeaponId == 52 ||
                        changedWeaponId == 54 ||
                        changedWeaponId == 56 ||
                        changedWeaponId == 58 ||
                        changedWeaponId == 60 ||
                        changedWeaponId == 62 ||
                        changedWeaponId == 64 ||
                        changedWeaponId == 66 ||
                        changedWeaponId == 68 ||
                        changedWeaponId == 71 ||
                        changedWeaponId == 73 ||
                        changedWeaponId == 75 ||
                        changedWeaponId == 77 ||
                        changedWeaponId == 79;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.KinoDerTotenWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Moon")
            {
                if (BlackOpsLibrary.MoonWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 10 ||
                        changedWeaponId == 12 ||
                        changedWeaponId == 19 ||
                        changedWeaponId == 21 ||
                        changedWeaponId == 34 ||
                        changedWeaponId == 38 ||
                        changedWeaponId == 44 ||
                        changedWeaponId == 47 ||
                        changedWeaponId == 49 ||
                        changedWeaponId == 52 ||
                        changedWeaponId == 54 ||
                        changedWeaponId == 56 ||
                        changedWeaponId == 58 ||
                        changedWeaponId == 60 ||
                        changedWeaponId == 62 ||
                        changedWeaponId == 64 ||
                        changedWeaponId == 66 ||
                        changedWeaponId == 68 ||
                        changedWeaponId == 70 ||
                        changedWeaponId == 72 ||
                        changedWeaponId == 75 ||
                        changedWeaponId == 81;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.MoonWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Nacht Der Untoten")
            {
                if (BlackOpsLibrary.NachtDerUntotenWeapons.ContainsKey(changedWeaponId))
                {
                    string gottenWeapon = BlackOpsLibrary.NachtDerUntotenWeapons[changedWeaponId];
                    if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                    {
                        m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                    }
                }
            }
            if (SelectedMap == "Shangri-la")
            {
                if (BlackOpsLibrary.ShangriLaWeapons.ContainsKey(changedWeaponId))
                {
                    bool isWeaponPap =
                        changedWeaponId == 9 ||
                        changedWeaponId == 11 ||
                        changedWeaponId == 18 ||
                        changedWeaponId == 20 ||
                        changedWeaponId == 31 ||
                        changedWeaponId == 35 ||
                        changedWeaponId == 41 ||
                        changedWeaponId == 44 ||
                        changedWeaponId == 46 ||
                        changedWeaponId == 49 ||
                        changedWeaponId == 51 ||
                        changedWeaponId == 53 ||
                        changedWeaponId == 55 ||
                        changedWeaponId == 57 ||
                        changedWeaponId == 59 ||
                        changedWeaponId == 61 ||
                        changedWeaponId == 63 ||
                        changedWeaponId == 65 ||
                        changedWeaponId == 68 ||
                        changedWeaponId == 70 ||
                        changedWeaponId == 72 ||
                        changedWeaponId == 74 ||
                        changedWeaponId == 76;

                    if (!isWeaponPap)
                    {
                        string gottenWeapon = BlackOpsLibrary.ShangriLaWeapons[changedWeaponId];
                        if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                        {
                            m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                        }
                    }
                }
            }
            if (SelectedMap == "Shi No Numa")
            {
                if (BlackOpsLibrary.ShiNoNumaWeapons.ContainsKey(changedWeaponId))
                {
                    string gottenWeapon = BlackOpsLibrary.ShiNoNumaWeapons[changedWeaponId];
                    if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                    {
                        m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                    }
                }
            }
            if (SelectedMap == "Verruckt")
            {
                if (BlackOpsLibrary.VerrucktWeapons.ContainsKey(changedWeaponId))
                {
                    string gottenWeapon = BlackOpsLibrary.VerrucktWeapons[changedWeaponId];
                    if (!m_WeaponsToNotCount.Any(x => x == gottenWeapon))
                    {
                        m_WeaponsGotten[gottenWeapon].Add(GetBoxHits());
                    }
                }
            }

            UpdateInventories();
        }

        private void UpdateInventories()
        {
            m_WhiteInventory[0] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT1);
            m_WhiteInventory[1] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT2);
            m_WhiteInventory[2] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT3);
            m_WhiteInventory[3] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT4);
            m_WhiteInventory[4] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT5);
            m_WhiteInventory[5] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT6);
            m_WhiteInventory[6] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT7);
            m_WhiteInventory[7] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT8);
            m_WhiteInventory[8] = BlackOpsLibrary.ReadInt(Constants.C_WHITEWEAPONSLOT9);

            m_BlueInventory[0] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT1);
            m_BlueInventory[1] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT2);
            m_BlueInventory[2] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT3);
            m_BlueInventory[3] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT4);
            m_BlueInventory[4] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT5);
            m_BlueInventory[5] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT6);
            m_BlueInventory[6] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT7);
            m_BlueInventory[7] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT8);
            m_BlueInventory[8] = BlackOpsLibrary.ReadInt(Constants.C_BLUEWEAPONSLOT9);

            m_YellowInventory[0] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT1);
            m_YellowInventory[1] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT2);
            m_YellowInventory[2] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT3);
            m_YellowInventory[3] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT4);
            m_YellowInventory[4] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT5);
            m_YellowInventory[5] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT6);
            m_YellowInventory[6] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT7);
            m_YellowInventory[7] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT8);
            m_YellowInventory[8] = BlackOpsLibrary.ReadInt(Constants.C_YELLOWWEAPONSLOT9);

            m_GreenInventory[0] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT1);
            m_GreenInventory[1] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT2);
            m_GreenInventory[2] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT3);
            m_GreenInventory[3] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT4);
            m_GreenInventory[4] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT5);
            m_GreenInventory[5] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT6);
            m_GreenInventory[6] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT7);
            m_GreenInventory[7] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT8);
            m_GreenInventory[8] = BlackOpsLibrary.ReadInt(Constants.C_GREENWEAPONSLOT9);
        }

        private int GetBoxHits()
        {
            return Application.Current.Dispatcher.Invoke(() => BoxHitsCount);
        }

        private void SetBoxHits(int value)
        {
            Application.Current.Dispatcher.Invoke(() => (BoxHitsCount = value));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void RightClick_Ascension(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Ascension");
        }
        private void RightClick_CallOfTheDead(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Call of the Dead");
        }
        private void RightClick_DerRiese(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Der Riese");
        }
        private void RightClick_Five(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Five");
        }
        private void RightClick_KinoDerToten(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Kino der Toten");
        }
        private void RightClick_Moon(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Moon");
        }
        private void RightClick_NachtDerUntoten(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Nacht Der Untoten");
        }
        private void RightClick_ShangriLa(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Shangri-la");
        }
        private void RightClick_ShiNoNuma(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Shi No Numa");
        }
        private void RightClick_Verruckt(object sender, RoutedEventArgs e)
        {
            SetSelectedMap("Verruckt");
        }

        private void SetSelectedMap(string map)
        {
            Dispatcher.Invoke(() =>
            {
                if (SelectedMap != map)
                {
                    SelectedMap = map;
                }
            });
        }

        private void RightClick_EndSetup(object sender, RoutedEventArgs e)
        {
            m_SetupDone = true;
        }

        private void RightClick_Export(object sender, RoutedEventArgs e)
        {
            ExportGame();
        }

        private void RightClick_Exit(object sender, RoutedEventArgs e)
        {
            BoxAverageTrackerWindow.Close();
        }

        private void ExportGame()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filename = SelectedMap + "_" + (DateTime.Now.ToString().Replace("/", "_").Replace(":", "-")) + ".txt";
            string createdFilepath = Path.Combine(currentDir, filename);

            string toWrite = string.Empty;

            toWrite += "Total hits : ";
            toWrite += BoxHitsCount.ToString();
            toWrite += "\n";

            foreach (var weapon in m_WeaponsGotten)
            {
                if (weapon.Value.Count > 0)
                {
                    toWrite += weapon.Key;
                    toWrite += "= ";
                    toWrite += weapon.Value.Count;
                    toWrite += " ";
                    toWrite += "(Average = ";
                    toWrite += (float)((float)m_WeaponsBoxHits[weapon.Key] / (float)weapon.Value.Count);
                    toWrite += ")";
                    toWrite += "\n";
                }
            }

            File.WriteAllText(createdFilepath, toWrite);
            Process.Start(createdFilepath);
        }

        private void LoadGame(object sender, RoutedEventArgs e)
        {
            LoadGame();
        }

        private void LoadGame()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                string[] fileText = File.ReadAllLines(filePath);

                string map = fileText.FirstOrDefault(x => x.StartsWith("Map=")).Substring("Map=".Length);
                string setup = fileText.FirstOrDefault(x => x.StartsWith("Setup=")).Substring("Setup=".Length);
                string totalHits = fileText.FirstOrDefault(x => x.StartsWith("Total hits=")).Substring("Total hits=".Length);
                Dictionary<string, int> weaponsHits = new Dictionary<string, int>(m_WeaponsBoxHits);
                Dictionary<string, List<int>> weaponsGotten = new Dictionary<string, List<int>>(m_WeaponsGotten);

                foreach (string line in fileText)
                {
                    if (line.StartsWith("WG-"))
                    {
                        int equalLoc = line.IndexOf("=", StringComparison.Ordinal);
                        string weaponName = line.Substring("WG-".Length, equalLoc - "WG-".Length);
                        List<int> gotten = line.Substring(line.LastIndexOf('=') + 1).Split(',').Select(int.Parse).ToList();

                        weaponsGotten[weaponName] = gotten;
                    }

                    if (line.StartsWith("WH-"))
                    {
                        int equalLoc = line.IndexOf("=", StringComparison.Ordinal);
                        string weaponName = line.Substring("WH-".Length, equalLoc - "WH-".Length);
                        int hits = int.Parse(line.Substring(line.LastIndexOf('=') + 1));

                        weaponsHits[weaponName] = hits;
                    }
                }

                SelectedMap = map;
                SetBoxHits(int.Parse(totalHits));
                m_WeaponsBoxHits = weaponsHits;
                m_WeaponsGotten = weaponsGotten;
                m_SetupDone = bool.Parse(setup);
                
                var weaponAverages = new ObservableCollection<KeyValuePair<string, float>>();
                foreach (var wtd in m_WeaponsToDisplay)
                {
                    float divider = (float)m_WeaponsGotten[wtd].Count > 0 ? (float)m_WeaponsGotten[wtd].Count : 1;
                    weaponAverages.Add(new KeyValuePair<string, float>(wtd, ((float)m_WeaponsBoxHits[wtd]) / (float)divider));
                }

                WeaponAverages = weaponAverages;
            }
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        private void SaveGame()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filename = "Saved_" + SelectedMap + "_" + (DateTime.Now.ToString().Replace("/", "_").Replace(":", "-")) + ".txt";
            string createdFilepath = Path.Combine(currentDir, filename);
            string toWrite = string.Empty;

            toWrite += "Map=";
            toWrite += SelectedMap;
            toWrite += "\n\n";

            toWrite += "Setup=";
            toWrite += m_SetupDone.ToString();
            toWrite += "\n\n";

            toWrite += "Total hits=";
            toWrite += BoxHitsCount.ToString();
            toWrite += "\n\n";

            foreach (var weapon in m_WeaponsGotten)
            {
                if (weapon.Value.Count > 0)
                {
                    toWrite += "WG-";
                    toWrite += weapon.Key;
                    toWrite += "=";
                    toWrite += string.Join(",", weapon.Value);
                    toWrite += "\n";
                }
            }
            toWrite += "\n";

            foreach (var weapon in m_WeaponsBoxHits)
            {
                toWrite += "WH-";
                toWrite += weapon.Key;
                toWrite += "=";
                toWrite += weapon.Value;
                toWrite += "\n";
            }

            File.WriteAllText(createdFilepath, toWrite);
            Process.Start(createdFilepath);
        }

        private void SetTextFont(object sender, RoutedEventArgs e)
        {
            var color = (TextForeground as System.Windows.Media.SolidColorBrush).Color;
            System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;

            if (TextFontStyle == FontStyles.Italic)
            {
                fontStyle = System.Drawing.FontStyle.Italic;
            }

            if (TextFontWeight == FontWeights.Bold)
            {
                fontStyle |= System.Drawing.FontStyle.Bold;
            }

            System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog()
            {
                AllowVerticalFonts = false,
                MinSize = 0,
                MaxSize = 0,
                Font = new System.Drawing.Font(TextFontFamily.Source, TextSize, fontStyle),
                Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B),
                ShowColor = true,
            };

            if (fontDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                TextFontFamily = new System.Windows.Media.FontFamily(fontDialog.Font.FontFamily.Name);
                TextForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(fontDialog.Color.R, fontDialog.Color.G, fontDialog.Color.B));
                TextSize = fontDialog.Font.Size;
                TextFontStyle = fontDialog.Font.Style.HasFlag(System.Drawing.FontStyle.Italic) ? FontStyles.Italic : FontStyles.Normal;
                TextFontWeight = fontDialog.Font.Style.HasFlag(System.Drawing.FontStyle.Bold) ? FontWeights.Bold : FontWeights.Regular;
            }
        }

        private void AutoDisplayClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                AutoDisplay = menuItem.IsChecked;
            }
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Version 1.6", "About", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        private void SetAverageTextFont(object sender, RoutedEventArgs e)
        {
            var color = (AverageTextForeground as System.Windows.Media.SolidColorBrush).Color;
            System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;

            if (AverageTextFontStyle == FontStyles.Italic)
            {
                fontStyle = System.Drawing.FontStyle.Italic;
            }

            if (AverageTextFontWeight == FontWeights.Bold)
            {
                fontStyle = System.Drawing.FontStyle.Bold;
            }
            System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog()
            {
                AllowVerticalFonts = false,
                MinSize = 0,
                MaxSize = 0,
                Font = new System.Drawing.Font(AverageTextFontFamily.Source, AverageTextSize, fontStyle),
                Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B),
                ShowColor = true,
            };

            if (fontDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                AverageTextFontFamily = new System.Windows.Media.FontFamily(fontDialog.Font.FontFamily.Name);
                AverageTextForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(fontDialog.Color.R, fontDialog.Color.G, fontDialog.Color.B));
                AverageTextSize = fontDialog.Font.Size;
                AverageTextFontStyle = fontDialog.Font.Style.HasFlag(System.Drawing.FontStyle.Italic) ? FontStyles.Italic : FontStyles.Normal;
                AverageTextFontWeight = fontDialog.Font.Style.HasFlag(System.Drawing.FontStyle.Bold) ? FontWeights.Bold : FontWeights.Regular;
            }
        }

        private void LoadApplicationConfig()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ApplicationConfig.txt");
            if (File.Exists(filePath))
            {
                string[] fileText = File.ReadAllLines(filePath);
                string boxHitsFont = fileText.FirstOrDefault(x => x.StartsWith("BoxHitsFont=")).Substring("BoxHitsFont=".Length);
                string boxHitsColor = fileText.FirstOrDefault(x => x.StartsWith("BoxHitsColor=")).Substring("BoxHitsColor=".Length);
                string boxHitsSize = fileText.FirstOrDefault(x => x.StartsWith("BoxHitsSize=")).Substring("BoxHitsSize=".Length);
                string boxHitsStyle = fileText.FirstOrDefault(x => x.StartsWith("BoxHitsStyle=")).Substring("BoxHitsStyle=".Length);
                string boxHitsWeight = fileText.FirstOrDefault(x => x.StartsWith("BoxHitsWeight=")).Substring("BoxHitsWeight=".Length);
                string averagesFont = fileText.FirstOrDefault(x => x.StartsWith("AveragesFont=")).Substring("AveragesFont=".Length);
                string averagesColor = fileText.FirstOrDefault(x => x.StartsWith("AveragesColor=")).Substring("AveragesColor=".Length);
                string averagesSize = fileText.FirstOrDefault(x => x.StartsWith("AveragesSize=")).Substring("AveragesSize=".Length);
                string averagesStyle = fileText.FirstOrDefault(x => x.StartsWith("AveragesStyle=")).Substring("AveragesStyle=".Length);
                string averagesWeight = fileText.FirstOrDefault(x => x.StartsWith("AveragesWeight=")).Substring("AveragesWeight=".Length);
                string totalHeight = fileText.FirstOrDefault(x => x.StartsWith("TotalHeight=")).Substring("TotalHeight=".Length);
                string totalWidth = fileText.FirstOrDefault(x => x.StartsWith("TotalWidth=")).Substring("TotalWidth=".Length);
                string autoDisplay = fileText.FirstOrDefault(x => x.StartsWith("AutoDisplay=")).Substring("AutoDisplay=".Length);

                TextFontFamily = new System.Windows.Media.FontFamily(boxHitsFont);
                TextForeground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(boxHitsColor));
                TextSize = float.Parse(boxHitsSize);
                TextFontStyle = (FontStyle)(new FontStyleConverter()).ConvertFromString(boxHitsStyle);
                TextFontWeight = (FontWeight)(new FontWeightConverter()).ConvertFromString(boxHitsWeight);
                AverageTextFontFamily = new System.Windows.Media.FontFamily(averagesFont);
                AverageTextForeground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(averagesColor));
                AverageTextSize = float.Parse(averagesSize);
                AverageTextFontStyle = (FontStyle)(new FontStyleConverter()).ConvertFromString(averagesStyle);
                AverageTextFontWeight = (FontWeight)(new FontWeightConverter()).ConvertFromString(averagesWeight);
                TotalHeight = int.Parse(totalHeight);
                TotalWidth = int.Parse(totalWidth);
                AutoDisplay = bool.Parse(autoDisplay);
            }
        }

        private void SaveApplicationConfig()
        {
            string createdFilepath = Path.Combine(Directory.GetCurrentDirectory(), "ApplicationConfig.txt");
            string toWrite = string.Empty;

            toWrite += "BoxHitsFont=";
            toWrite += TextFontFamily.Source;
            toWrite += "\n";

            toWrite += "BoxHitsColor=";
            toWrite += (TextForeground as System.Windows.Media.SolidColorBrush).Color.ToString();
            toWrite += "\n";

            toWrite += "BoxHitsSize=";
            toWrite += TextSize.ToString();
            toWrite += "\n";

            toWrite += "BoxHitsStyle=";
            toWrite += TextFontStyle.ToString();
            toWrite += "\n";

            toWrite += "BoxHitsWeight=";
            toWrite += TextFontWeight.ToString();
            toWrite += "\n";

            toWrite += "AveragesFont=";
            toWrite += AverageTextFontFamily.Source;
            toWrite += "\n";

            toWrite += "AveragesColor=";
            toWrite += (AverageTextForeground as System.Windows.Media.SolidColorBrush).Color.ToString();
            toWrite += "\n";

            toWrite += "AveragesSize=";
            toWrite += AverageTextSize.ToString();
            toWrite += "\n";

            toWrite += "AveragesStyle=";
            toWrite += AverageTextFontStyle.ToString();
            toWrite += "\n";

            toWrite += "AveragesWeight=";
            toWrite += AverageTextFontWeight.ToString();
            toWrite += "\n";

            toWrite += "TotalHeight=";
            toWrite += TotalHeight.ToString();
            toWrite += "\n";

            toWrite += "TotalWidth=";
            toWrite += TotalWidth.ToString();
            toWrite += "\n";

            toWrite += "AutoDisplay=";
            toWrite += AutoDisplay.ToString();
            toWrite += "\n";

            File.WriteAllText(createdFilepath, toWrite);
        }

        private void ShowCounting(object sender, RoutedEventArgs e)
        {
            if (countWeaponsWindow == null)
            {
                Dictionary<int, string> mapWeapons = new Dictionary<int, string>();
                switch (SelectedMap)
                {
                    case "Ascension":
                        mapWeapons = BlackOpsLibrary.AscensionWeapons;
                        break;
                    case "Call of the Dead":
                        mapWeapons = BlackOpsLibrary.CallOfTheDeadWeapons;
                        break;
                    case "Der Riese":
                        mapWeapons = BlackOpsLibrary.DerRieseWeapons;
                        break;
                    case "Five":
                        mapWeapons = BlackOpsLibrary.FiveWeapons;
                        break;
                    case "Kino der Toten":
                        mapWeapons = BlackOpsLibrary.KinoDerTotenWeapons;
                        break;
                    case "Moon":
                        mapWeapons = BlackOpsLibrary.MoonWeapons;
                        break;
                    case "Nacht Der Untoten":
                        mapWeapons = BlackOpsLibrary.NachtDerUntotenWeapons;
                        break;
                    case "Shangri-la":
                        mapWeapons = BlackOpsLibrary.ShangriLaWeapons;
                        break;
                    case "Shi No Numa":
                        mapWeapons = BlackOpsLibrary.ShiNoNumaWeapons;
                        break;
                    case "Verruckt":
                        mapWeapons = BlackOpsLibrary.VerrucktWeapons;
                        break;
                    default:
                        break;
                }

                List<string> weapons = mapWeapons.Values.ToList();
                if (SelectedMap != "Nacht Der Untoten" && SelectedMap != "Moon" && SelectedMap != "MainMenu")
                {
                    weapons.Add("Teddy bear");
                }
                countWeaponsWindow = new CountWeapons(weapons, m_WeaponsToNotCount, m_WeaponsToDisplay);
                countWeaponsWindow.ShowDialog();
                m_WeaponsToNotCount = countWeaponsWindow.WeaponsToNotCount.ToList();
                m_WeaponsToDisplay = countWeaponsWindow.WeaponsToDisplay.ToList();

                var weaponAverages = new ObservableCollection<KeyValuePair<string, float>>();
                foreach (var wtd in m_WeaponsToDisplay)
                {
                    float divider = (float)m_WeaponsGotten[wtd].Count > 0 ? (float)m_WeaponsGotten[wtd].Count : 1;
                    weaponAverages.Add(new KeyValuePair<string, float>(wtd, ((float)m_WeaponsBoxHits[wtd]) / (float)divider));
                }

                WeaponAverages = weaponAverages;
                countWeaponsWindow = null;
            }
        }
    }
}
