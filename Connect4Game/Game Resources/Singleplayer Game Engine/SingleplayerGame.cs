using Connect4Game.Game_Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Connect4Game.Game_Resources.Graphics_Manager;
using System.Threading.Tasks;

namespace Connect4Game.Game_Resources.Singleplayer_Game_Engine
{
    public enum PlayerType
    {
        Human,
        CPU
    }
    public class SingleplayerGame : Game
    {
        private CpuPlayer CpuPlayer;
        private PlayerType CurrentPlayer;


        public SingleplayerGame()
        {
            CpuPlayer = new CpuPlayer();
            CpuPlayer.Turn = false;
            CurrentPlayer = PlayerType.Human;
        }

        public override void CheckNewPlay(int cellPosition)
        {
           
            if (CellNotFree(cellPosition))
            {
                MessageBox.Show("Columna llena, elegir otra");
                return;
            }

            EnableDisableButtons();

            ProcessPlay(PlayerType.Human, cellPosition);

            if (!Player.Turn && Status != GameState.NotStartedOrFinished)
            {
                CpuPlayer.Play();
            }

            EnableDisableButtons();
        }

        public override void EndGame()
        {
            EnableDisableButtons();
            //Thread.Sleep(1000);
            GameWindow.EndOfGameMessage.Visibility = Visibility.Visible;

            //Se muestra un mensaje que depende de si hubo ganador, y quien fue.
            if (!FullPanel())
            {
                if (Player.Turn)
                {
                    GraphicsManager.BackgroundRed(GameWindow.EndOfGameMessage);
                    GameWindow.EndOfGameMessage.Content = $"Ganador: {Player.Name}";
                }
                else
                {
                    GraphicsManager.BackgroundBlue(GameWindow.EndOfGameMessage);
                    GameWindow.EndOfGameMessage.Content = $"Ganador: {CpuPlayer.Name}";
                }
            }
            else
            {
                GraphicsManager.PaintButtonGold(GameWindow.EndOfGameMessage);
                GameWindow.EndOfGameMessage.Content = "Empate";
            }
            //se cambia el estado del juego a "terminado"
            Status = GameState.NotStartedOrFinished;
        }

        public override void StartGame()
        {
            CpuPlayer.GameContext_ = this;
            UpdateLabels();
            Status = GameState.Playing;

        }

        protected void UpdateTurns()
        {
            Player.Turn = !Player.Turn;
            CpuPlayer.Turn = !CpuPlayer.Turn;
            CurrentPlayer = Player.Turn ? PlayerType.Human : PlayerType.CPU;
        }


        protected override void UpdateLabels()
        {
            GameWindow.TurnLabel.Content = Player.Turn ? $"Turno de {Player.Name}" : "Turno de la CPU";
        }

        public void ProcessPlay(PlayerType player, int cellPosition)
        {
            //controla que el jugador humano no juegue cuando no es su turno
            if (player == PlayerType.Human && CurrentPlayer == PlayerType.CPU)
            {
                MessageBox.Show("No es tu turno");
                return;
            }

            UpdateCell(cellPosition, out int lastCellPosition);

            CheckForWinner(lastCellPosition);

            UpdateTurns();

            UpdateLabels();
        }
    }
}
