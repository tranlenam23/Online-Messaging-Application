﻿#pragma checksum "..\..\..\SignUp_Password.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F341A02EB7FDECC49B11669F43E55849CC5F00D5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ChatOnline;
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


namespace ChatOnline {
    
    
    /// <summary>
    /// SignUp_Password
    /// </summary>
    public partial class SignUp_Password : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox placeholderText;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Confirm_PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Confirm_placeholderText;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Login_page;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Signup_button;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.GradientStop leftStopLogin;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\SignUp_Password.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.GradientStop rightStopLogin;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ChatOnline;component/signup_password.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SignUp_Password.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 17 "..\..\..\SignUp_Password.xaml"
            this.PasswordBox.GotFocus += new System.Windows.RoutedEventHandler(this.PasswordBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\SignUp_Password.xaml"
            this.PasswordBox.LostFocus += new System.Windows.RoutedEventHandler(this.PasswordBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.placeholderText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Confirm_PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 21 "..\..\..\SignUp_Password.xaml"
            this.Confirm_PasswordBox.GotFocus += new System.Windows.RoutedEventHandler(this.Confirm_PasswordBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\SignUp_Password.xaml"
            this.Confirm_PasswordBox.LostFocus += new System.Windows.RoutedEventHandler(this.ConfirmPasswordBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Confirm_placeholderText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Login_page = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\SignUp_Password.xaml"
            this.Login_page.Click += new System.Windows.RoutedEventHandler(this.LoginButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Signup_button = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\SignUp_Password.xaml"
            this.Signup_button.Click += new System.Windows.RoutedEventHandler(this.Check_SignUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.leftStopLogin = ((System.Windows.Media.GradientStop)(target));
            return;
            case 8:
            this.rightStopLogin = ((System.Windows.Media.GradientStop)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

