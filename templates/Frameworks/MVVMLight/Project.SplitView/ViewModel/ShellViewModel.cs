﻿using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using uct.ItemName.Services;
using uct.ItemName.Model;

namespace uct.ItemName.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        private NavigationService navigationService
        {
            get { return ServiceLocator.Current.GetInstance<NavigationService>(); }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ShellNavigationItem _selectedItem;
        public ShellNavigationItem SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); }
        }

        private ObservableCollection<ShellNavigationItem> _navigationItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> NavigationItems
        {
            get { return _navigationItems; }
            set { Set(ref _navigationItems, value); }
        }

        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                if(_openPaneCommand == null)
                {
                    _openPaneCommand = new RelayCommand(() => IsPaneOpen = !_isPaneOpen);
                }
                
                return _openPaneCommand;
            }
        }

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand
        {
            get
            {
                if(_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
                }
                
                return _itemClickCommand;
            }
        }

        public ShellViewModel() 
        {
            //More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
            SelectedItem = NavigationItems.FirstOrDefault();
        }
        
        public void Initialize(NavigationEventArgs args)
        {
            string viewModelName = args?.Parameter?.ToString();
            if (String.IsNullOrEmpty(viewModelName))
            {
                viewModelName = typeof(MainViewModel).FullName;
            }
            navigationService.Navigate(viewModelName);
        }

        public void SetNavigationItem(NavigationEventArgs args)
        {
            var item = NavigationItems?.FirstOrDefault(i => i.ViewModelName == navigationService.GetViewModel(args?.SourcePageType));
            if (item != null)
            {
                SelectedItem = item;
            }
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            var navigationItem = args?.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                navigationService.Navigate(navigationItem.ViewModelName);
            }
        }
    }
}