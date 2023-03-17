// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Runtime.InteropServices;
using WinRT;
using CommunityToolkit;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using Windows.UI;
using System.Drawing;
using Windows.Media.Capture;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Minimalistic_Code_Editor
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window
    {
        private Microsoft.UI.Windowing.AppWindow m_AppWindow;

        public MainWindow()
        {
            this.InitializeComponent();

            //ExtendsContentIntoTitleBar = true;
            //SetTitleBar(AppTitleBar);

            DrawCustomTitleBar();
        }

        private Microsoft.UI.Windowing.AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return Microsoft.UI.Windowing.AppWindow.GetFromWindowId(wndId);
        }

        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Check to see if customization is supported.
            // Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported()
                && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Update drag region if the size of the title bar changes.
                SetDragRegionForCustomTitleBar(m_AppWindow);
            }
        }

        private void DrawCustomTitleBar()
        {
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                m_AppWindow = GetAppWindowForCurrentWindow();
                m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

                m_AppWindow.TitleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(0, 33, 33, 33);

                SetDragRegionForCustomTitleBar(m_AppWindow);
            }
        }

        private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
        {
            int menuBarWidth = m_AppWindow.TitleBar.RightInset;
            MainMenuBar.Margin = new Thickness(0, 0, menuBarWidth + 5, 0);

            int windowMenuBarWidthAndPadding = (int)MainMenuBar.ActualWidth + (int)MainMenuBar.Margin.Right;
            int dragRegionWidth = m_AppWindow.Size.Width - menuBarWidth;

            Windows.Graphics.RectInt32[] dragRects = Array.Empty<Windows.Graphics.RectInt32>();
            Windows.Graphics.RectInt32 dragRect;

            dragRect.X = windowMenuBarWidthAndPadding;
            dragRect.Y = 0;
            dragRect.Height = (int)AppTitleBar.Height;
            dragRect.Width = dragRegionWidth;

            Windows.Graphics.RectInt32[] dragRectsArray = dragRects.Append(dragRect).ToArray();
            m_AppWindow.TitleBar.SetDragRectangles(dragRectsArray);
        }

        private void MenuFlyoutItem_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var selectedFlyoutItem = sender as MenuFlyoutItem;
            switch (selectedFlyoutItem.Text)
            {
                case "Exit":
                    CoreApplication.Exit();
                    Status.Text = "Exit";
                    break;
                case "Open...":
                    Status.Text = "Open";
                    break;
                    
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //zastapic opacity czyms innym, chociazby booleanem
            if (FindReplaceGrid.Opacity == 0)
            {
                StoryboardFindReplaceGrid.Children[0].SetValue(DoubleAnimation.FromProperty, TranslationFindReplace.Y > 0 ? 0 : -200);
                StoryboardFindReplaceGrid.Children[0].SetValue(DoubleAnimation.ToProperty, TranslationFindReplace.Y);
                FindReplaceGrid.Opacity = 1;
            }
            else
            {
                FindReplaceGrid.Opacity = 0;
                StoryboardFindReplaceGrid.Children[0].SetValue(DoubleAnimation.FromProperty, TranslationFindReplace.Y);
                StoryboardFindReplaceGrid.Children[0].SetValue(DoubleAnimation.ToProperty, TranslationFindReplace.Y > 0 ? 0 : 200);
            }
            StoryboardFindReplaceGrid.Begin();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}