﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GIR.Integracao.ServicoComunicacao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.example.org/NewWSDLFile/", ConfigurationName="ServicoComunicacao.ServicoEmailPort")]
    public interface ServicoEmailPort {
        
        // CODEGEN: Generating message contract since the operation EnviarEmail is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.example.org/NewWSDLFile/EnviarEmail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        GIR.Integracao.ServicoComunicacao.EnviarEmailResponse1 EnviarEmail(GIR.Integracao.ServicoComunicacao.EnviarEmailRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/NewWSDLFile/")]
    public partial class EnviarEmailRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string[] paraField;
        
        private string assuntoField;
        
        private string conteudoField;
        
        private string deField;
        
        private string comCopiaParaField;
        
        private string copiaOcultaParaField;
        
        private string responderParaField;
        
        private string enderecoRespostaField;
        
        private string identificadorSistemaField;
        
        private string tipoConteudoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("para", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string[] para {
            get {
                return this.paraField;
            }
            set {
                this.paraField = value;
                this.RaisePropertyChanged("para");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string assunto {
            get {
                return this.assuntoField;
            }
            set {
                this.assuntoField = value;
                this.RaisePropertyChanged("assunto");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string conteudo {
            get {
                return this.conteudoField;
            }
            set {
                this.conteudoField = value;
                this.RaisePropertyChanged("conteudo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string de {
            get {
                return this.deField;
            }
            set {
                this.deField = value;
                this.RaisePropertyChanged("de");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string comCopiaPara {
            get {
                return this.comCopiaParaField;
            }
            set {
                this.comCopiaParaField = value;
                this.RaisePropertyChanged("comCopiaPara");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string copiaOcultaPara {
            get {
                return this.copiaOcultaParaField;
            }
            set {
                this.copiaOcultaParaField = value;
                this.RaisePropertyChanged("copiaOcultaPara");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string responderPara {
            get {
                return this.responderParaField;
            }
            set {
                this.responderParaField = value;
                this.RaisePropertyChanged("responderPara");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string enderecoResposta {
            get {
                return this.enderecoRespostaField;
            }
            set {
                this.enderecoRespostaField = value;
                this.RaisePropertyChanged("enderecoResposta");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string identificadorSistema {
            get {
                return this.identificadorSistemaField;
            }
            set {
                this.identificadorSistemaField = value;
                this.RaisePropertyChanged("identificadorSistema");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string tipoConteudo {
            get {
                return this.tipoConteudoField;
            }
            set {
                this.tipoConteudoField = value;
                this.RaisePropertyChanged("tipoConteudo");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/NewWSDLFile/")]
    public partial class EnviarEmailResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int codigoRetornoField;
        
        private string mensagemRetornoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int codigoRetorno {
            get {
                return this.codigoRetornoField;
            }
            set {
                this.codigoRetornoField = value;
                this.RaisePropertyChanged("codigoRetorno");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string mensagemRetorno {
            get {
                return this.mensagemRetornoField;
            }
            set {
                this.mensagemRetornoField = value;
                this.RaisePropertyChanged("mensagemRetorno");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarEmailRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.example.org/NewWSDLFile/", Order=0)]
        public GIR.Integracao.ServicoComunicacao.EnviarEmailRequest EnviarEmailRequest;
        
        public EnviarEmailRequest1() {
        }
        
        public EnviarEmailRequest1(GIR.Integracao.ServicoComunicacao.EnviarEmailRequest EnviarEmailRequest) {
            this.EnviarEmailRequest = EnviarEmailRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarEmailResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.example.org/NewWSDLFile/", Order=0)]
        public GIR.Integracao.ServicoComunicacao.EnviarEmailResponse EnviarEmailResponse;
        
        public EnviarEmailResponse1() {
        }
        
        public EnviarEmailResponse1(GIR.Integracao.ServicoComunicacao.EnviarEmailResponse EnviarEmailResponse) {
            this.EnviarEmailResponse = EnviarEmailResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicoEmailPortChannel : GIR.Integracao.ServicoComunicacao.ServicoEmailPort, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicoEmailPortClient : System.ServiceModel.ClientBase<GIR.Integracao.ServicoComunicacao.ServicoEmailPort>, GIR.Integracao.ServicoComunicacao.ServicoEmailPort {
        
        public ServicoEmailPortClient() {
        }
        
        public ServicoEmailPortClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicoEmailPortClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoEmailPortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoEmailPortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GIR.Integracao.ServicoComunicacao.EnviarEmailResponse1 GIR.Integracao.ServicoComunicacao.ServicoEmailPort.EnviarEmail(GIR.Integracao.ServicoComunicacao.EnviarEmailRequest1 request) {
            return base.Channel.EnviarEmail(request);
        }
        
        public GIR.Integracao.ServicoComunicacao.EnviarEmailResponse EnviarEmail(GIR.Integracao.ServicoComunicacao.EnviarEmailRequest EnviarEmailRequest) {
            GIR.Integracao.ServicoComunicacao.EnviarEmailRequest1 inValue = new GIR.Integracao.ServicoComunicacao.EnviarEmailRequest1();
            inValue.EnviarEmailRequest = EnviarEmailRequest;
            GIR.Integracao.ServicoComunicacao.EnviarEmailResponse1 retVal = ((GIR.Integracao.ServicoComunicacao.ServicoEmailPort)(this)).EnviarEmail(inValue);
            return retVal.EnviarEmailResponse;
        }
    }
}
