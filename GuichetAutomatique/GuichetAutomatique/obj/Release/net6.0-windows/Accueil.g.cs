﻿#pragma checksum "..\..\..\Accueil.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39E1CFE8F53B972670ACC36D4497CD2CF025696D"
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
    /// Accueil
    /// </summary>
    public partial class Accueil : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Retrait;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Depot;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PaiementFacture;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InfoCompte;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Virement;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Quitter;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Accueil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReleveTrans;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GuichetAutomatique;component/accueil.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Accueil.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Retrait = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\Accueil.xaml"
            this.Retrait.Click += new System.Windows.RoutedEventHandler(this.Retrait_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Depot = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\Accueil.xaml"
            this.Depot.Click += new System.Windows.RoutedEventHandler(this.Depot_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PaiementFacture = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\Accueil.xaml"
            this.PaiementFacture.Click += new System.Windows.RoutedEventHandler(this.PaiementFacture_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.InfoCompte = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\Accueil.xaml"
            this.InfoCompte.Click += new System.Windows.RoutedEventHandler(this.InfoCompte_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Virement = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\Accueil.xaml"
            this.Virement.Click += new System.Windows.RoutedEventHandler(this.Virement_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Quitter = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Accueil.xaml"
            this.Quitter.Click += new System.Windows.RoutedEventHandler(this.Quitter_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ReleveTrans = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\Accueil.xaml"
            this.ReleveTrans.Click += new System.Windows.RoutedEventHandler(this.ReleveTrans_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
