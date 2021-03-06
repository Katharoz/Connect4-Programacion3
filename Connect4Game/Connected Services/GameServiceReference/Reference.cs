﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connect4Game.GameServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameServiceReference.IGameService", CallbackContract=typeof(Connect4Game.GameServiceReference.IGameServiceCallback))]
    public interface IGameService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/JoinGame")]
        void JoinGame(string playerName);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/JoinGame")]
        System.Threading.Tasks.Task JoinGameAsync(string playerName);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/LeaveGame")]
        void LeaveGame(string playerName);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/LeaveGame")]
        System.Threading.Tasks.Task LeaveGameAsync(string playerName);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendSelectedCell")]
        void SendSelectedCell(int cellNumber);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendSelectedCell")]
        System.Threading.Tasks.Task SendSelectedCellAsync(int cellNumber);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/UpdateClient")]
        void UpdateClient(int cellNumber);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/UpdateTurn")]
        void UpdateTurn(bool turn);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/UpdateGameStatus")]
        void UpdateGameStatus(bool status);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/GetOpponentName")]
        void GetOpponentName(string oppName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceChannel : Connect4Game.GameServiceReference.IGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GameServiceClient : System.ServiceModel.DuplexClientBase<Connect4Game.GameServiceReference.IGameService>, Connect4Game.GameServiceReference.IGameService {
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void JoinGame(string playerName) {
            base.Channel.JoinGame(playerName);
        }
        
        public System.Threading.Tasks.Task JoinGameAsync(string playerName) {
            return base.Channel.JoinGameAsync(playerName);
        }
        
        public void LeaveGame(string playerName) {
            base.Channel.LeaveGame(playerName);
        }
        
        public System.Threading.Tasks.Task LeaveGameAsync(string playerName) {
            return base.Channel.LeaveGameAsync(playerName);
        }
        
        public void SendSelectedCell(int cellNumber) {
            base.Channel.SendSelectedCell(cellNumber);
        }
        
        public System.Threading.Tasks.Task SendSelectedCellAsync(int cellNumber) {
            return base.Channel.SendSelectedCellAsync(cellNumber);
        }
    }
}
