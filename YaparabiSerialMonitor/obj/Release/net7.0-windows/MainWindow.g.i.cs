﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "240664DA22F92FC6E625C13D43EC2B65F8E5F3EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using YaparabiSerialMonitor;


namespace YaparabiSerialMonitor {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 119 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox portComboBox;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock portDescriptionText;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox baudComboBox;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button connectButton;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button resetButton;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button clearButton;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox autoScrollCheckBox;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox timestampCheckBox;
        
        #line default
        #line hidden
        
        
        #line 180 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox dataTextBox;
        
        #line default
        #line hidden
        
        
        #line 197 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Paragraph terminalParagraph;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox sendTextBox;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sendButton;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse connectionIndicator;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock statusText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/YaparabiSerialMonitor;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.portComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 121 "..\..\..\MainWindow.xaml"
            this.portComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PortComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.portDescriptionText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.baudComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.connectButton = ((System.Windows.Controls.Button)(target));
            
            #line 149 "..\..\..\MainWindow.xaml"
            this.connectButton.Click += new System.Windows.RoutedEventHandler(this.ConnectButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.resetButton = ((System.Windows.Controls.Button)(target));
            
            #line 155 "..\..\..\MainWindow.xaml"
            this.resetButton.Click += new System.Windows.RoutedEventHandler(this.ResetButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.clearButton = ((System.Windows.Controls.Button)(target));
            
            #line 161 "..\..\..\MainWindow.xaml"
            this.clearButton.Click += new System.Windows.RoutedEventHandler(this.ClearButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.autoScrollCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 170 "..\..\..\MainWindow.xaml"
            this.autoScrollCheckBox.Checked += new System.Windows.RoutedEventHandler(this.AutoScrollCheckBox_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.timestampCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.dataTextBox = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 10:
            this.terminalParagraph = ((System.Windows.Documents.Paragraph)(target));
            return;
            case 11:
            this.sendTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 216 "..\..\..\MainWindow.xaml"
            this.sendTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.SendTextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 12:
            this.sendButton = ((System.Windows.Controls.Button)(target));
            
            #line 222 "..\..\..\MainWindow.xaml"
            this.sendButton.Click += new System.Windows.RoutedEventHandler(this.SendButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.connectionIndicator = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 14:
            this.statusText = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

