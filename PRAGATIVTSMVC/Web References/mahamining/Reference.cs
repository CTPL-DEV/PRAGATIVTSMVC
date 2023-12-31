﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace PRAGATIVTSMVC.mahamining {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://gps.mahamining.com/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback InsertGPRSdataOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::PRAGATIVTSMVC.Properties.Settings.Default.PRAGATIVTSMVC_mahamining_Service;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event InsertGPRSdataCompletedEventHandler InsertGPRSdataCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.mahamining.com/InsertGPRSdata", RequestNamespace="http://gps.mahamining.com/", ResponseNamespace="http://gps.mahamining.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void InsertGPRSdata(
                    string Unit, 
                    string Reason, 
                    string CommandKey, 
                    string Commandkeyvalue, 
                    int Ignition, 
                    int PowerCut, 
                    int BoxOpen, 
                    string MSGKey, 
                    long Odometer, 
                    int Speed, 
                    int SatVisible, 
                    string GPSfixed, 
                    string Latitude, 
                    string Longitude, 
                    int Altitude, 
                    int Direction, 
                    string Time, 
                    string DATE, 
                    int GSMStrength, 
                    int DeviceCompanyId) {
            this.Invoke("InsertGPRSdata", new object[] {
                        Unit,
                        Reason,
                        CommandKey,
                        Commandkeyvalue,
                        Ignition,
                        PowerCut,
                        BoxOpen,
                        MSGKey,
                        Odometer,
                        Speed,
                        SatVisible,
                        GPSfixed,
                        Latitude,
                        Longitude,
                        Altitude,
                        Direction,
                        Time,
                        DATE,
                        GSMStrength,
                        DeviceCompanyId});
        }
        
        /// <remarks/>
        public void InsertGPRSdataAsync(
                    string Unit, 
                    string Reason, 
                    string CommandKey, 
                    string Commandkeyvalue, 
                    int Ignition, 
                    int PowerCut, 
                    int BoxOpen, 
                    string MSGKey, 
                    long Odometer, 
                    int Speed, 
                    int SatVisible, 
                    string GPSfixed, 
                    string Latitude, 
                    string Longitude, 
                    int Altitude, 
                    int Direction, 
                    string Time, 
                    string DATE, 
                    int GSMStrength, 
                    int DeviceCompanyId) {
            this.InsertGPRSdataAsync(Unit, Reason, CommandKey, Commandkeyvalue, Ignition, PowerCut, BoxOpen, MSGKey, Odometer, Speed, SatVisible, GPSfixed, Latitude, Longitude, Altitude, Direction, Time, DATE, GSMStrength, DeviceCompanyId, null);
        }
        
        /// <remarks/>
        public void InsertGPRSdataAsync(
                    string Unit, 
                    string Reason, 
                    string CommandKey, 
                    string Commandkeyvalue, 
                    int Ignition, 
                    int PowerCut, 
                    int BoxOpen, 
                    string MSGKey, 
                    long Odometer, 
                    int Speed, 
                    int SatVisible, 
                    string GPSfixed, 
                    string Latitude, 
                    string Longitude, 
                    int Altitude, 
                    int Direction, 
                    string Time, 
                    string DATE, 
                    int GSMStrength, 
                    int DeviceCompanyId, 
                    object userState) {
            if ((this.InsertGPRSdataOperationCompleted == null)) {
                this.InsertGPRSdataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsertGPRSdataOperationCompleted);
            }
            this.InvokeAsync("InsertGPRSdata", new object[] {
                        Unit,
                        Reason,
                        CommandKey,
                        Commandkeyvalue,
                        Ignition,
                        PowerCut,
                        BoxOpen,
                        MSGKey,
                        Odometer,
                        Speed,
                        SatVisible,
                        GPSfixed,
                        Latitude,
                        Longitude,
                        Altitude,
                        Direction,
                        Time,
                        DATE,
                        GSMStrength,
                        DeviceCompanyId}, this.InsertGPRSdataOperationCompleted, userState);
        }
        
        private void OnInsertGPRSdataOperationCompleted(object arg) {
            if ((this.InsertGPRSdataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InsertGPRSdataCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void InsertGPRSdataCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591