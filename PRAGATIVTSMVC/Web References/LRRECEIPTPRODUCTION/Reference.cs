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

namespace PRAGATIVTSMVC.LRRECEIPTPRODUCTION {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="VTSLRReceipt_OutBinding", Namespace="http://bajajauto.co.in/VTS/IB/LRReceipt")]
    public partial class VTSLRReceipt_OutService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback VTSLRReceipt_OutOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public VTSLRReceipt_OutService() {
            this.Url = global::PRAGATIVTSMVC.Properties.Settings.Default.PRAGATIVTSMVC_LRRECEIPTDEVELOPMENT_VTSLRReceipt_OutService;
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
        public event VTSLRReceipt_OutCompletedEventHandler VTSLRReceipt_OutCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sap.com/xi/WebService/soap1.1", OneWay=true, Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public void VTSLRReceipt_Out([System.Xml.Serialization.XmlArrayAttribute(Namespace="http://bajajauto.co.in/VTS/IB/LRReceipt")] [System.Xml.Serialization.XmlArrayItemAttribute("Records", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)] VTSLRReceiptRecords[] VTSLRReceipt) {
            this.Invoke("VTSLRReceipt_Out", new object[] {
                        VTSLRReceipt});
        }
        
        /// <remarks/>
        public void VTSLRReceipt_OutAsync(VTSLRReceiptRecords[] VTSLRReceipt) {
            this.VTSLRReceipt_OutAsync(VTSLRReceipt, null);
        }
        
        /// <remarks/>
        public void VTSLRReceipt_OutAsync(VTSLRReceiptRecords[] VTSLRReceipt, object userState) {
            if ((this.VTSLRReceipt_OutOperationCompleted == null)) {
                this.VTSLRReceipt_OutOperationCompleted = new System.Threading.SendOrPostCallback(this.OnVTSLRReceipt_OutOperationCompleted);
            }
            this.InvokeAsync("VTSLRReceipt_Out", new object[] {
                        VTSLRReceipt}, this.VTSLRReceipt_OutOperationCompleted, userState);
        }
        
        private void OnVTSLRReceipt_OutOperationCompleted(object arg) {
            if ((this.VTSLRReceipt_OutCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VTSLRReceipt_OutCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://bajajauto.co.in/VTS/IB/LRReceipt")]
    public partial class VTSLRReceiptRecords {
        
        private string cOSIGNMENT_IDField;
        
        private string lR_NUMBERField;
        
        private string lR_DATEField;
        
        private string tRUCK_NOField;
        
        private string tRANSPORTER_IDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string COSIGNMENT_ID {
            get {
                return this.cOSIGNMENT_IDField;
            }
            set {
                this.cOSIGNMENT_IDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LR_NUMBER {
            get {
                return this.lR_NUMBERField;
            }
            set {
                this.lR_NUMBERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LR_DATE {
            get {
                return this.lR_DATEField;
            }
            set {
                this.lR_DATEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRUCK_NO {
            get {
                return this.tRUCK_NOField;
            }
            set {
                this.tRUCK_NOField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRANSPORTER_ID {
            get {
                return this.tRANSPORTER_IDField;
            }
            set {
                this.tRANSPORTER_IDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void VTSLRReceipt_OutCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591