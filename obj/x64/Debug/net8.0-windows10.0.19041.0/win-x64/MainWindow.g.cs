﻿#pragma checksum "C:\Users\Edge\Documents\GitHub\GT-AdminDB\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9F3DA04AB666E53BA0F8C196A90FA57A3441411A8C80E217B3FEEE9CAA8DDF24"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GT_AdminDB
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainWindow.xaml line 12
                {
                    this.splitView = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.SplitView>(target);
                }
                break;
            case 3: // MainWindow.xaml line 20
                {
                    this.PaneHeader = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 4: // MainWindow.xaml line 21
                {
                    this.btnOpenPane = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnOpenPane).Click += this.btnOpenPane_Click;
                }
                break;
            case 5: // MainWindow.xaml line 112
                {
                    this.infobarMain = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.InfoBar>(target);
                }
                break;
            case 6: // MainWindow.xaml line 113
                {
                    this.btnConfig = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnConfig).Click += this.btnConfig_Click;
                }
                break;
            case 7: // MainWindow.xaml line 27
                {
                    this.expMiembros = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Expander>(target);
                }
                break;
            case 8: // MainWindow.xaml line 50
                {
                    this.expRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Expander>(target);
                }
                break;
            case 9: // MainWindow.xaml line 74
                {
                    this.expFiltros = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Expander>(target);
                }
                break;
            case 10: // MainWindow.xaml line 93
                {
                    this.expOpcionAdicional = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Expander>(target);
                }
                break;
            case 11: // MainWindow.xaml line 109
                {
                    this.btnTest = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnTest).Click += this.btnTest_Click;
                }
                break;
            case 12: // MainWindow.xaml line 105
                {
                    this.chkSeleccionMultiple = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.chkSeleccionMultiple).Checked += this.chkSeleccionMultiple_Checked;
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.chkSeleccionMultiple).Unchecked += this.chkSeleccionMultiple_Unchecked;
                }
                break;
            case 13: // MainWindow.xaml line 106
                {
                    this.chkHistorialVentana = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                }
                break;
            case 14: // MainWindow.xaml line 87
                {
                    this.txtFiltroNombre = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.TextBox)this.txtFiltroNombre).TextChanged += this.OnFilterChanged;
                }
                break;
            case 15: // MainWindow.xaml line 88
                {
                    this.chkFiltroLogout = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.chkFiltroLogout).Checked += this.chkFiltroLogout_Checked;
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)this.chkFiltroLogout).Unchecked += this.chkFiltroLogout_Unchecked;
                }
                break;
            case 16: // MainWindow.xaml line 89
                {
                    this.chkFiltroRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                }
                break;
            case 17: // MainWindow.xaml line 65
                {
                    this.dpkFechaRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CalendarDatePicker>(target);
                }
                break;
            case 18: // MainWindow.xaml line 66
                {
                    this.txtCantidadRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                }
                break;
            case 19: // MainWindow.xaml line 67
                {
                    this.txtDanoTotal = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                }
                break;
            case 20: // MainWindow.xaml line 68
                {
                    this.btnAgregarRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnAgregarRaid).Click += this.btnAgregarRaid_Click;
                }
                break;
            case 21: // MainWindow.xaml line 69
                {
                    this.btnEliminarRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnEliminarRaid).Click += this.btnEliminarRaid_Click;
                }
                break;
            case 22: // MainWindow.xaml line 70
                {
                    this.btnStartRaid = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnStartRaid).Click += this.btnStartRaid_Click;
                }
                break;
            case 23: // MainWindow.xaml line 42
                {
                    this.txtNombre = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 24: // MainWindow.xaml line 43
                {
                    this.dpkFechaIngreso = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CalendarDatePicker>(target);
                }
                break;
            case 25: // MainWindow.xaml line 44
                {
                    this.dpkUltimoLogin = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CalendarDatePicker>(target);
                }
                break;
            case 26: // MainWindow.xaml line 45
                {
                    this.btnAgregarMiembro = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnAgregarMiembro).Click += this.btnAgregarMiembro_Click;
                }
                break;
            case 27: // MainWindow.xaml line 46
                {
                    this.btnCancelarMiembro = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.btnCancelarMiembro).Click += this.btnCancelarMiembro_Click;
                }
                break;
            case 28: // MainWindow.xaml line 122
                {
                    this.contentFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2408")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

