﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6746C318989C025393329E1FA7CA812A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Maps.MapControl.WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WpfApplication3 {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtExpanderHeader;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUpload;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilterPoints;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDBSCAN;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMedian;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMedian;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveNoisies;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MedianBySpeed;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbFiles;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbCluster;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.Map MyMap;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.MapLayer mapTracksLayer;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.MapLayer mapShapeLayer;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.MapLayer mapPushPinLayer;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliPath;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbSpeedGroups;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtClusterCount;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCluster;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbClusterGroups;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbWaitingClusters;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedClusterCount;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClusterBySpeed;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbClusterSpeedGroups;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedCluster1;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedCluster2;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedCluster3;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedCluster4;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSpeedCluster5;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbStatus;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApplication3;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 24 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Expander)(target)).Collapsed += new System.Windows.RoutedEventHandler(this.Expander_Collapsed_1);
            
            #line default
            #line hidden
            
            #line 24 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Expander)(target)).Expanded += new System.Windows.RoutedEventHandler(this.Expander_Expanded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtExpanderHeader = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.btnUpload = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\MainWindow.xaml"
            this.btnUpload.Click += new System.Windows.RoutedEventHandler(this.btnUpload_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnFilterPoints = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\MainWindow.xaml"
            this.btnFilterPoints.Click += new System.Windows.RoutedEventHandler(this.btnFilterPoints_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnDBSCAN = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\MainWindow.xaml"
            this.btnDBSCAN.Click += new System.Windows.RoutedEventHandler(this.btnDBSCAN_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnMedian = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\MainWindow.xaml"
            this.btnMedian.Click += new System.Windows.RoutedEventHandler(this.btnMedian_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtMedian = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.btnRemoveNoisies = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\MainWindow.xaml"
            this.btnRemoveNoisies.Click += new System.Windows.RoutedEventHandler(this.btnRemoveNoisies_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MedianBySpeed = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\MainWindow.xaml"
            this.MedianBySpeed.Click += new System.Windows.RoutedEventHandler(this.MedianBySpeed_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.lbFiles = ((System.Windows.Controls.ListBox)(target));
            
            #line 42 "..\..\MainWindow.xaml"
            this.lbFiles.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbFiles_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.lbCluster = ((System.Windows.Controls.ListBox)(target));
            return;
            case 12:
            this.MyMap = ((Microsoft.Maps.MapControl.WPF.Map)(target));
            
            #line 46 "..\..\MainWindow.xaml"
            this.MyMap.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.MyMap_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 13:
            this.mapTracksLayer = ((Microsoft.Maps.MapControl.WPF.MapLayer)(target));
            return;
            case 14:
            this.mapShapeLayer = ((Microsoft.Maps.MapControl.WPF.MapLayer)(target));
            return;
            case 15:
            this.mapPushPinLayer = ((Microsoft.Maps.MapControl.WPF.MapLayer)(target));
            return;
            case 16:
            this.sliPath = ((System.Windows.Controls.Slider)(target));
            
            #line 65 "..\..\MainWindow.xaml"
            this.sliPath.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliPath_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 17:
            this.lbSpeedGroups = ((System.Windows.Controls.ListBox)(target));
            
            #line 69 "..\..\MainWindow.xaml"
            this.lbSpeedGroups.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbSpeedGroups_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.txtClusterCount = ((System.Windows.Controls.TextBox)(target));
            return;
            case 19:
            this.btnCluster = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\MainWindow.xaml"
            this.btnCluster.Click += new System.Windows.RoutedEventHandler(this.btnCluster_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            this.lbClusterGroups = ((System.Windows.Controls.ListBox)(target));
            
            #line 93 "..\..\MainWindow.xaml"
            this.lbClusterGroups.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbClusterGroups_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 21:
            this.lbWaitingClusters = ((System.Windows.Controls.ListBox)(target));
            return;
            case 22:
            this.txtSpeedClusterCount = ((System.Windows.Controls.TextBox)(target));
            return;
            case 23:
            this.btnClusterBySpeed = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\MainWindow.xaml"
            this.btnClusterBySpeed.Click += new System.Windows.RoutedEventHandler(this.btnClusterBySpeed_Click);
            
            #line default
            #line hidden
            return;
            case 24:
            this.lbClusterSpeedGroups = ((System.Windows.Controls.ListBox)(target));
            
            #line 105 "..\..\MainWindow.xaml"
            this.lbClusterSpeedGroups.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbClusterSpeedGroups_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 25:
            this.txtSpeedCluster1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 26:
            this.txtSpeedCluster2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 27:
            this.txtSpeedCluster3 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 28:
            this.txtSpeedCluster4 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 29:
            this.txtSpeedCluster5 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 30:
            this.tbStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

