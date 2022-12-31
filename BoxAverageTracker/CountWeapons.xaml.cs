using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BoxAverageTracker
{
    /// <summary>
    /// Interaction logic for CountWeapons.xaml
    /// </summary>
    public partial class CountWeapons : Window
    {
        private ICommand selectAllToCount;
        private ICommand selectNoneToCount;
        private ICommand selectAllToDisplay;
        private ICommand selectNoneToDisplay;

        public ICommand SelectAllToCount
        {
            get
            {
                if (selectAllToCount == null)
                {
                    selectAllToCount = new CommandHandler(() => SelectAllToCountMethod(), null);
                }

                return selectAllToCount;
            }

            set
            {
                selectAllToCount = value;
            }
        }

        public ICommand SelectNoneToCount
        {
            get
            {
                if (selectNoneToCount == null)
                {
                    selectNoneToCount = new CommandHandler(() => SelectNoneToCountMethod(), null);
                }

                return selectNoneToCount;
            }

            set
            {
                selectNoneToCount = value;
            }
        }

        public ICommand SelectAllToDisplay
        {
            get
            {
                if (selectAllToDisplay == null)
                {
                    selectAllToDisplay = new CommandHandler(() => SelectAllToDisplayMethod(), null);
                }

                return selectAllToDisplay;
            }

            set
            {
                selectAllToDisplay = value;
            }
        }

        public ICommand SelectNoneToDisplay
        {
            get
            {
                if (selectNoneToDisplay == null)
                {
                    selectNoneToDisplay = new CommandHandler(() => SelectNoneToDisplayMethod(), null);
                }

                return selectNoneToDisplay;
            }

            set
            {
                selectNoneToDisplay = value;
            }
        }

        public class WeaponToCount
        {
            public WeaponToCount()
            {
                IsCounting = true;
                IsDisplayed = false;
                WeaponName = string.Empty;
            }

            public WeaponToCount(bool isCounting, bool isDisplayed, string weaponName)
            {
                IsCounting = isCounting;
                IsDisplayed = isDisplayed;
                WeaponName = weaponName;
            }

            public bool IsCounting { get; set; }
            public bool IsDisplayed { get; set; }
            public string WeaponName { get; set; }
        }

        public static DependencyProperty WeaponsProperty = DependencyProperty.Register("Weapons", typeof(ObservableCollection<WeaponToCount>), typeof(CountWeapons), new PropertyMetadata(new ObservableCollection<WeaponToCount>()));
        public static DependencyProperty WeaponsToNotCountProperty = DependencyProperty.Register("WeaponsToNotCount", typeof(ObservableCollection<string>), typeof(CountWeapons), new PropertyMetadata(new ObservableCollection<string>()));
        public static DependencyProperty WeaponsToDisplayProperty = DependencyProperty.Register("WeaponsToDisplay", typeof(ObservableCollection<string>), typeof(CountWeapons), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<WeaponToCount> Weapons
        {
            get => (ObservableCollection<WeaponToCount>)GetValue(WeaponsProperty);
            set => SetValue(WeaponsProperty, value);
        }

        public ObservableCollection<string> WeaponsToNotCount
        {
            get => (ObservableCollection<string>)GetValue(WeaponsToNotCountProperty);
            set => SetValue(WeaponsToNotCountProperty, value);
        }

        public ObservableCollection<string> WeaponsToDisplay
        {
            get => (ObservableCollection<string>)GetValue(WeaponsToDisplayProperty);
            set => SetValue(WeaponsToDisplayProperty, value);
        }

        public CountWeapons(List<string> weapons, List<string> weaponsToNotCount, List<string> weaponsToDisplay)
        {
            InitializeComponent();

            DataContext = this;

            var weaponsToCount = new List<WeaponToCount>();
            weapons = weapons.Distinct().ToList();
            foreach (var weapon in weapons)
            {
                weaponsToCount.Add(new WeaponToCount(!weaponsToNotCount.Contains(weapon), weaponsToDisplay.Contains(weapon), weapon));
            }

            Weapons = new ObservableCollection<WeaponToCount>(weaponsToCount);
            WeaponsToNotCount = new ObservableCollection<string>(weaponsToNotCount);
            WeaponsToDisplay = new ObservableCollection<string>(weaponsToDisplay);
        }

        public void SelectAllToCountMethod()
        {
            var weapons = new ObservableCollection<WeaponToCount>(Weapons);

            for (int i = 0; i < weapons.Count; ++i)
            {
                weapons[i].IsCounting = true;
            }
            Weapons = weapons;
            WeaponsToNotCount = new ObservableCollection<string>();
        }

        public void SelectNoneToCountMethod()
        {
            var weapons = new ObservableCollection<WeaponToCount>(Weapons);
            var weaponsToNotCount = new ObservableCollection<string>();

            for (int i = 0; i < weapons.Count; ++i)
            {
                weapons[i].IsCounting = false;
                weaponsToNotCount.Add(weapons[i].WeaponName);
            }

            Weapons = weapons;
            WeaponsToNotCount = weaponsToNotCount;
        }

        public void SelectAllToDisplayMethod()
        {
            var weapons = new ObservableCollection<WeaponToCount>(Weapons);
            var weaponsToDisplay = new ObservableCollection<string>();

            for (int i = 0; i < weapons.Count; ++i)
            {
                weapons[i].IsDisplayed = true;
                weaponsToDisplay.Add(weapons[i].WeaponName);
            }

            Weapons = weapons;
            WeaponsToDisplay = weaponsToDisplay;
        }

        public void SelectNoneToDisplayMethod()
        {
            var weapons = new ObservableCollection<WeaponToCount>(Weapons);

            for (int i = 0; i < weapons.Count; ++i)
            {
                weapons[i].IsDisplayed = false;
            }

            Weapons = weapons;
            WeaponsToDisplay = new ObservableCollection<string>();
        }

        private void IsCounting_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).DataContext is WeaponToCount weaponToCount && weaponToCount.IsCounting == false && WeaponsToNotCount.Any(x => x == weaponToCount.WeaponName))
            {
                var weaponsToNotCount = new ObservableCollection<string>(WeaponsToNotCount);
                var weapons = new ObservableCollection<WeaponToCount>(Weapons);

                weapons.FirstOrDefault(x => x.WeaponName == weaponToCount.WeaponName).IsCounting = true;
                weaponsToNotCount.Remove(weaponToCount.WeaponName);

                Weapons = weapons;
                WeaponsToNotCount = weaponsToNotCount;
            }
        }

        private void IsCounting_UnChecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).DataContext is WeaponToCount weaponToCount && weaponToCount.IsCounting == true && !WeaponsToNotCount.Any(x => x == weaponToCount.WeaponName))
            {
                var weaponsToNotCount = new ObservableCollection<string>(WeaponsToNotCount);
                var weapons = new ObservableCollection<WeaponToCount>(Weapons);

                weapons.FirstOrDefault(x => x.WeaponName == weaponToCount.WeaponName).IsCounting = false;
                weaponsToNotCount.Add(weaponToCount.WeaponName);

                Weapons = weapons;
                WeaponsToNotCount = weaponsToNotCount;
            }
        }

        private void IsDisplayed_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).DataContext is WeaponToCount weaponToCount && weaponToCount.IsDisplayed == false && !WeaponsToDisplay.Any(x => x == weaponToCount.WeaponName))
            {
                var weaponsToDisplay = new ObservableCollection<string>(WeaponsToDisplay);
                var weapons = new ObservableCollection<WeaponToCount>(Weapons);

                weapons.FirstOrDefault(x => x.WeaponName == weaponToCount.WeaponName).IsDisplayed = true;
                weaponsToDisplay.Add(weaponToCount.WeaponName);

                Weapons = weapons;
                WeaponsToDisplay = weaponsToDisplay;
            }
        }

        private void IsDisplayed_UnChecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).DataContext is WeaponToCount weaponToCount && weaponToCount.IsDisplayed == true && WeaponsToDisplay.Any(x => x == weaponToCount.WeaponName))
            {
                var weaponsToDisplay = new ObservableCollection<string>(WeaponsToDisplay);
                var weapons = new ObservableCollection<WeaponToCount>(Weapons);

                weapons.FirstOrDefault(x => x.WeaponName == weaponToCount.WeaponName).IsDisplayed = false;
                weaponsToDisplay.Remove(weaponToCount.WeaponName);

                Weapons = weapons;
                WeaponsToDisplay = weaponsToDisplay;
            }
        }
    }
}
