﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrintNCI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=localhost;Initial Catalog=RSKUPANG;Persist Security Info=True;User ID" +
            "=jamkesmas;Password=jamkesmas")]
        public string RSKUPANGConnectionString {
            get {
                return ((string)(this["RSKUPANGConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Jaminan_Askes_IGD_FromBill.exe")]
        public string ExecutablePrintAskes {
            get {
                return ((string)(this["ExecutablePrintAskes"]));
            }
            set {
                this["ExecutablePrintAskes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Jaminan_Jamkesmas_IGD_FromBill.exe")]
        public string ExecutablePrintJamkesmasRWJ {
            get {
                return ((string)(this["ExecutablePrintJamkesmasRWJ"]));
            }
            set {
                this["ExecutablePrintJamkesmasRWJ"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public string UntukUGD {
            get {
                return ((string)(this["UntukUGD"]));
            }
            set {
                this["UntukUGD"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Jaminan_Jamkesmas_RWI.exe")]
        public string ExecutablePrintJamkesmasRWI {
            get {
                return ((string)(this["ExecutablePrintJamkesmasRWI"]));
            }
            set {
                this["ExecutablePrintJamkesmasRWI"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PrintNCI.log")]
        public string LogFileName {
            get {
                return ((string)(this["LogFileName"]));
            }
            set {
                this["LogFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public string Logging {
            get {
                return ((string)(this["Logging"]));
            }
            set {
                this["Logging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NCICetakStatusRI.tmp")]
        public string RITempFileName {
            get {
                return ((string)(this["RITempFileName"]));
            }
            set {
                this["RITempFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Jaminan_Jamkesda_IGD_FromBill.exe")]
        public string ExecutablePrintJamkesdaRWJ {
            get {
                return ((string)(this["ExecutablePrintJamkesdaRWJ"]));
            }
            set {
                this["ExecutablePrintJamkesdaRWJ"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NCICetakStatus.tmp")]
        public string RJTempFileName {
            get {
                return ((string)(this["RJTempFileName"]));
            }
            set {
                this["RJTempFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Status_Pasien.exe")]
        public string ExecutablePrintStatus {
            get {
                return ((string)(this["ExecutablePrintStatus"]));
            }
            set {
                this["ExecutablePrintStatus"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Jaminan_In_Health_IGD_FromBill.exe")]
        public string ExecutablePrintInHealth {
            get {
                return ((string)(this["ExecutablePrintInHealth"]));
            }
            set {
                this["ExecutablePrintInHealth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
                    <ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                        <string>ASKES</string>
                        <string>JAMKESMAS</string>
                        <string>IN HEALTH</string>
                    </ArrayOfString>
                ")]
        public string Kelompok {
            get {
                return ((string)(this["Kelompok"]));
            }
            set {
                this["Kelompok"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public string UpdateTracer {
            get {
                return ((string)(this["UpdateTracer"]));
            }
            set {
                this["UpdateTracer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PrintNCI")]
        public string ProgramFolder {
            get {
                return ((string)(this["ProgramFolder"]));
            }
            set {
                this["ProgramFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Kartu_Berobat.exe")]
        public string ExecutablePrintKartuBerobat {
            get {
                return ((string)(this["ExecutablePrintKartuBerobat"]));
            }
            set {
                this["ExecutablePrintKartuBerobat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Print_Tracer.exe")]
        public string ExecutablePrintTracer {
            get {
                return ((string)(this["ExecutablePrintTracer"]));
            }
            set {
                this["ExecutablePrintTracer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sa")]
        public string DatabaseUser {
            get {
                return ((string)(this["DatabaseUser"]));
            }
            set {
                this["DatabaseUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("pocopoco")]
        public string DatabasePassword {
            get {
                return ((string)(this["DatabasePassword"]));
            }
            set {
                this["DatabasePassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("APO")]
        public string Modul {
            get {
                return ((string)(this["Modul"]));
            }
            set {
                this["Modul"] = value;
            }
        }
    }
}
