﻿#pragma checksum "..\..\..\Dialogs\KeyboardInputDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DCF538FB5C8ADCB865E8EEB97EB26F83B37062ADE52AEF5E68AD0906005B8A81"
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


namespace WpfStaticIPApp.Dialogs {
    
    
    /// <summary>
    /// KeyboardInputDialog
    /// </summary>
    public partial class KeyboardInputDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IpAddressTextBox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SubnetMaskTextBox;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox GatewayTextBox;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PreferredDNSTextBox;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AlternateDNSTextBox;
        
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
            System.Uri resourceLocater = new System.Uri("/Static IP Application;component/dialogs/keyboardinputdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
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
            this.IpAddressTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.SubnetMaskTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.GatewayTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.PreferredDNSTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.AlternateDNSTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            
            #line 31 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ConfirmButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 32 "..\..\..\Dialogs\KeyboardInputDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

