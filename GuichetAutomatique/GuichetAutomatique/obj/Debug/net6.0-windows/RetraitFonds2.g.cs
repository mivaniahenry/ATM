﻿#pragma checksum "..\..\..\RetraitFonds2.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E3BCA794B29087CE677D363F3477E33D286709AD"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using GuichetAutomatique;
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


namespace GuichetAutomatique {
    
    
    /// <summary>
    /// RetraitFonds2
    /// </summary>
    public partial class RetraitFonds2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\RetraitFonds2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LabelnoCPTRetrait;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\RetraitFonds2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMontantRetrait;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\RetraitFonds2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OKRetrait;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\RetraitFonds2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AnnulerRetrait;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\RetraitFonds2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Quitter;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GuichetAutomatique;component/retraitfonds2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\RetraitFonds2.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LabelnoCPTRetrait = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.txtMontantRetrait = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.OKRetrait = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\RetraitFonds2.xaml"
            this.OKRetrait.Click += new System.Windows.RoutedEventHandler(this.OKRetrait_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AnnulerRetrait = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\RetraitFonds2.xaml"
            this.AnnulerRetrait.Click += new System.Windows.RoutedEventHandler(this.AnnulerRetrait_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Quitter = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\RetraitFonds2.xaml"
            this.Quitter.Click += new System.Windows.RoutedEventHandler(this.Quitter_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
