﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PRAGATIVTSMVC.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://agni.bajajauto.co.in:9095/XISOAPAdapter/MessageServlet?senderParty=&sender" +
            "Service=BC_VTS&receiverParty=&receiverService=&interface=VTSAcknowledgementSent_" +
            "Out&interfaceNamespace=http%3A%2F%2Fbajajauto.co.in%2FVTS%2FIB%2FAcknowledgement" +
            "")]
        public string PRAGATIVTSMVC_WebReference_VTSAcknowledgementSent_OutService {
            get {
                return ((string)(this["PRAGATIVTSMVC_WebReference_VTSAcknowledgementSent_OutService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://agni.bajajauto.co.in:9095/XISOAPAdapter/MessageServlet?senderParty=&sender" +
            "Service=BC_VTS&receiverParty=&receiverService=&interface=VTSLRReceipt_Out&interf" +
            "aceNamespace=http%3A%2F%2Fbajajauto.co.in%2FVTS%2FIB%2FLRReceipt")]
        public string PRAGATIVTSMVC_WebReference1_VTSLRReceipt_OutService {
            get {
                return ((string)(this["PRAGATIVTSMVC_WebReference1_VTSLRReceipt_OutService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://agni.bajajauto.co.in:9099/XISOAPAdapter/MessageServlet?senderParty=&sender" +
            "Service=BC_VTS&receiverParty=&receiverService=&interface=VTSLRReceipt_Out&interf" +
            "aceNamespace=http%3A%2F%2Fbajajauto.co.in%2FVTS%2FIB%2FLRReceipt")]
        public string PRAGATIVTSMVC_LRRECEIPTDEVELOPMENT_VTSLRReceipt_OutService {
            get {
                return ((string)(this["PRAGATIVTSMVC_LRRECEIPTDEVELOPMENT_VTSLRReceipt_OutService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://gps.mahamining.com/Service.asmx")]
        public string PRAGATIVTSMVC_mahamining_Service {
            get {
                return ((string)(this["PRAGATIVTSMVC_mahamining_Service"]));
            }
        }
    }
}
