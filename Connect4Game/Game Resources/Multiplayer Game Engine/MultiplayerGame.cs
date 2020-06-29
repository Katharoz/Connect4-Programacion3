using Connect4;
using Connect4Game.Game_Resources;
using Connect4Game.GameServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using Connect4Game.Game_Resources.Graphics_Manager;

namespace Connect4Game.Game_Resources.Multiplayer_Game_Engine
{
    public class MultiplayerGame : Game
    {
        private Player Opponent;
        private Client GameClient;
        private bool OponentLeftTheGame;
        
        public MultiplayerGame()
        {
            Status = GameState.NotStartedOrFinished;
            Player = new Player();

        }

        public override void CheckNewPlay(int cellPosition)
        {
            if (OponentLeftTheGame)
            {
                MessageBox.Show("El juego ha finalizado porque tu oponente abandono el juego",
                                "Juego Finalizado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }
            if (Status == GameState.NotStartedOrFinished)
            {
                MessageBox.Show("Esperando oponente para comenzar el juego",
                                "El juego no ha comenzado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (!Player.Turn)
            {
                MessageBox.Show($"Es el turno de {Opponent.Name}",
                                "Espera tu Turno",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            if (CellNotFree(cellPosition))
            {
                MessageBox.Show("Columna llena, elegir otra",
                                "Jugada no Disponible",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            EnableDisableButtons();

            GameClient.SendCellData(cellPosition);

            EnableDisableButtons();

        }

        public override void EndGame()
        {

            bool playing = Status == GameState.Playing;

            EnableDisableButtons();
            //mensaje en caso de que un jugador abandone el juego
            if (OponentLeftTheGame)
            {
                GraphicsManager.BackgroundRed(GameWindow.EndOfGameMessage);
                GameWindow.EndOfGameMessage.Content = $"Tu oponenente {Opponent.Name} abandono el juego, ganaste!";
            }
            //Se muestra un mensaje que depende de si hubo ganador, y quien fue.
            else if (!FullPanel() && playing)
            {
                if (Player.Turn)
                {
                    GraphicsManager.BackgroundRed(GameWindow.EndOfGameMessage);
                    GameWindow.EndOfGameMessage.Content = $"Felicidades {Player.Name}, ganaste!";
                }
                else
                {
                    GraphicsManager.BackgroundBlue(GameWindow.EndOfGameMessage);
                    GameWindow.EndOfGameMessage.Content = $"Tu Oponente {Opponent.Name} gano!";
                }
            }
            else
            {
                GraphicsManager.PaintButtonGold(GameWindow.EndOfGameMessage);
                GameWindow.EndOfGameMessage.Content = "Empate";
            }
            GameWindow.EndOfGameMessage.Visibility = Visibility.Visible;
            //se cambia el estado del juego a "terminado"
            Status = GameState.NotStartedOrFinished;
            GameClient.Exit();
        }

        public override void StartGame()
        {
            StartGameSession();
        }

        private void StartGameSession()
        {
            try
            {
                GameClient = new Client(this);
                GameClient.JoinGame();
                UpdateLabels();
            }
            catch (Exception)
            {
                MessageBox.Show("Debes iniciar el servidor antes de iniciar una partida multijugador", 
                                "Error de Conexion", 
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                GameWindow.MainMenu();
            }
        }

        protected override void UpdateLabels()
        {
            if (Status == GameState.NotStartedOrFinished)
            {
                GameWindow.TurnLabel.Content = "Esperando oponente para comenzar...";
                return;
            }
            GameWindow.TurnLabel.Content = Player.Turn ? $"Turno de {Player.Name}" : $"Turno de {Opponent.Name}";
        }

        private void CreateOpponent(string name)
        {
            Opponent = new Player();
            Opponent.Name = name;         
        }

        class ClientCallBack : IGameServiceCallback
        {      
            private MultiplayerGame GameContext;
            public ClientCallBack(MultiplayerGame GameContext)
            {
                this.GameContext = GameContext;
            }

            public void UpdateGameStatus(bool status)
            { 
                if (status)
                {
                    //MessageBox.Show("2 jugadores");
                    GameContext.Status = GameState.Playing;
                }
                if (!status && GameContext.Status == GameState.Playing)
                {
                    GameContext.Status = GameState.NotStartedOrFinished;
                    GameContext.OponentLeftTheGame = true;
                     
                    var option = MessageBox.Show($"Tu oponente {GameContext.Opponent.Name} abandono el juego :(",
                                "Juego Finalizado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                    if (option == MessageBoxResult.OK)
                    {
                        GameContext.EndGame();    
                    }
                }
            }

            public void UpdateClient(int cell) //deberia ser numero de celda
            {
                GameContext.UpdateCell(cell, out int lastCellPosition);
                GameContext.CheckForWinner(lastCellPosition);
                
            }

            public void UpdateTurn(bool turn)
            {
                GameContext.Player.Turn = turn;
                GameContext.UpdateLabels();
            }

            public void GetOpponentName(string oppName)
            {
                GameContext.CreateOpponent(oppName);
                GameContext.UpdateLabels();
            }
        }
        class Client
        {
            private ClientCallBack callBack;
            private InstanceContext context;
            private GameServiceClient client;
            private MultiplayerGame GameContext;
            public Client(MultiplayerGame game)
            {
                GameContext = game;
                callBack = new ClientCallBack(game);
                context = new InstanceContext(callBack);
                client = new GameServiceClient(context);
                client.Open();
            }

            public void JoinGame()
            {
                client.JoinGame(GameContext.Player.Name);
            }

            public void SendCellData(int cellNumber)
            {
                client.SendSelectedCell(cellNumber);
            }

            public void Exit()
            {
                client.LeaveGame(GameContext.Player.Name);
                client.Close();
            }
        }
    }
}
