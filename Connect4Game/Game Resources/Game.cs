using Connect4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Connect4Game.Game_Resources.Graphics_Manager;
using System.Windows;

namespace Connect4Game.Game_Resources
{
    public abstract class Game
    {
        protected MainWindow gameWindow;

        protected Player Player;

        protected CellState[] CellStates;

        protected int gridSize;

        protected int GameRows;

        protected int GameColumns;

        protected GameState Status;

        protected List<Button> Winners;

        public MainWindow GameWindow { get => gameWindow; set => gameWindow = value; }

        public Game()
        {
            Player = new Player();
            Player.Name = "asd";
            Player.Turn = true;
        }

        public abstract void StartGame();

        public abstract void CheckNewPlay(int cellPosition);

        public abstract void EndGame();

        protected abstract void UpdateLabels();


        public int GetRows()
        {
            return GameRows;
        }

        public int GetColumns()
        {
            return GameColumns;
        }

        public void CreateGame(GridSize _gridSize)
        {
            gridSize = (int)_gridSize;
            GameRows = gridSize - 2;
            GameColumns = gridSize - 2;
            CellStates = new CellState[GameRows * GameColumns];
            GameManager.GenerateGameGrid(gameWindow, _gridSize);
        }

        public void SetPlayerName(string name)
        {
            Player.Name = name;
        }

        protected void UpdateCell(int cellNumber, out int updatedCellPosition)
        {
            Button button = GameWindow.Buttons[cellNumber];

            UpdateCellPositionAndColor(button, cellNumber, out int lastPosition);

            updatedCellPosition = lastPosition;

            //Se cambia el color del boton al del correspondiente jugador. 
            //UpdateCellColor(button);
            
            //Si es el turno del jugador uno, se pone el color rojo en esa posicion del arreglo, sino, el azul;
            UpdateCellState(lastPosition);

        }

        private void UpdateCellPositionAndColor(Button cell, int firstPosition, out int lastPosition)
        {
            int cellNumber = firstPosition;
            lastPosition = firstPosition;
            while (cellNumber <= (GameRows * GameColumns) - GameColumns - 1 && CellStates[cellNumber + GameColumns] == CellState.Free)
            {
                //GraphicsManager.Animations.FlashLabel(cell,Player.Turn);
                cellNumber += GameColumns;
                lastPosition = cellNumber;
                cell = GameWindow.Buttons[cellNumber];
            }
            UpdateCellColor(cell);
        }

        private void UpdateCellColor(Button cell)
        {
            if (Player.Turn)
                GraphicsManager.PaintCellRed(cell);
            else
                GraphicsManager.PaintCellBlue(cell);
        }

        private void UpdateCellState(int cellPosition)
        {
            CellStates[cellPosition] = Player.Turn ? CellState.Red : CellState.Blue;
        }

        protected void EnableDisableButtons()
        {
            foreach (var button in GameWindow.Buttons)
            {
                button.IsEnabled ^= true;
            }
        }

        public bool CellNotFree(int columnNumber)
        {
           return CellStates[columnNumber] != CellState.Free;  
        }

        public void HideEndGameMessage()
        {
            GameWindow.EndOfGameMessage.Visibility = Visibility.Hidden;
        }

        protected void CheckForWinner(int cellPosition)
        {
            int points;
            Button button = GameWindow.Buttons[cellPosition];
            Winners = new List<Button>(); //Lista que guarda las posibles celdas "ganadoras"

            //Control Horizontal.
            Winners.Add(button);
            points = 1 + CheckLeft(cellPosition - 1) + CheckRight(cellPosition + 1);
            if (points >= 4)
            {
                
                //Si el jugador gano, se cambia el color de las fichas para indicarlo.
                //GraphicsManager.PaintButtonGold(button);
                //WinLeft(cellPosition - 1, button);
                //WinRight(cellPosition + 1, button);
                PlayerWon();
                EndGame();
                return;
            }

            //Control Vertical.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckDown(cellPosition + GameColumns);
            if (points >= 4)
            {
                //GraphicsManager.PaintButtonGold(button);
                //WinDown(cellPosition + GameColumns, button);
                PlayerWon();
                EndGame();
                return;
            }

            //Control Diagonal \.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckUpLeft(cellPosition - (GameColumns + 1)) + CheckDownRight(cellPosition + (GameColumns + 1));
            if (points >= 4)
            {
                //GraphicsManager.PaintButtonGold(button);
                //WinUpLeft(cellPosition - (GameColumns + 1), button);
                //WinDownRight(cellPosition + (GameColumns + 1), button);
                PlayerWon();
                EndGame();
                return;
            }

            //Control Diagonal /.
            Winners.Clear();
            Winners.Add(button);
            points = 1 + CheckUpRight(cellPosition - (GameColumns - 1)) + CheckDownLeft(cellPosition + (GameColumns - 1));
            if (points >= 4)
            {
                //GraphicsManager.PaintButtonGold(button);
                //WinUpRight(cellPosition - (GameColumns - 1), button);
                //WinDownLeft(cellPosition + (GameColumns - 1), button);
                PlayerWon();
                EndGame();
                return;
            }

            //En caso de no haber ganador, se controla si el panel esta lleno.
            //Si lo esta, se termina el juego.
            if (FullPanel()) 
                EndGame();

        }

        protected bool FullPanel()
        {
            if (!CellStates.Any(result => result == CellState.Free))
            {
                //Pinta los botones de naranja para indicar la perdida de ambos jugadores.
                foreach (var button in GameWindow.Buttons)
                {
                    GraphicsManager.PaintButtonOrange(button);
                }
                return true;
            }
            return false;
        }

        private int CheckLeft(int position)
        {
            
            //Control que no este en la columna mas a la izquierda | Que no sea una posicion negativa | Que el boton tenga un tipo distinto de free | Que tenga el mismo tipo que el boton anterior
            if (!(position % GameColumns != (GameColumns - 1) && position >= 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + 1]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckLeft(position - 1) + 1;
        }

        //Controla los elementos iguales a la derecha.
        private int CheckRight(int position)
        {
            if (!(position % GameColumns != 0 && position <= (GameColumns * GameRows - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - 1]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckRight(position + 1) + 1;
        }

        //Controla los elementos iguales hacia abajo.
        private int CheckDown(int position)
        {
            //
            if (!(position <= (GameColumns * GameRows - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - GameColumns]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDown(position + GameColumns) + 1;
        }

        //Controla los elementos iguales en diagonal arriba/izquierda.
        private int CheckUpLeft(int position)
        {
            if (!(position >= 0 && (position + (GameColumns + 1)) % GameColumns != 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + (GameColumns + 1)]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckUpLeft(position - (GameColumns + 1)) + 1;
        }

        //Controla los elementos iguales en diagonal abajo/derecha.
        private int CheckDownRight(int position)
        {
            if (!(position <= (GameColumns * GameRows - 1) && (position - (GameColumns + 1)) % GameColumns != (GameColumns - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - (GameColumns + 1)]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDownRight(position + (GameColumns + 1)) + 1;
        }

        //Controla los elementos iguales en diagonal arriba\derecha.
        private int CheckUpRight(int position)
        {
            if (!(position >= 0 && (position + (GameColumns - 1)) % GameColumns != (GameColumns - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + (GameColumns - 1)]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckUpRight(position - (GameColumns - 1)) + 1;
        }

        //Controla los elementos iguales en diagonal abajo\izquierda.
        private int CheckDownLeft(int position)
        {
            if (!(position <= (GameColumns * GameRows - 1) && (position - (GameColumns - 1)) % GameColumns != 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - (GameColumns - 1)]))
            {
                return 0;
            }
            Button button = GameWindow.Buttons[position];
            Winners.Add(button);
            return CheckDownLeft(position + (GameColumns - 1)) + 1;
        }

        //private int WinLeft(int position, Button button)
        //{
        //    //Control que no este en la columna mas a la izquierda | Que no sea una posicion negativa | Que el boton tenga un tipo distinto de free | Que tenga el mismo tipo que el boton anterior
        //    if (!(position % GameColumns != (GameColumns - 1) && position >= 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + 1]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinLeft(position - 1, button);
        //}

        //private int WinRight(int position, Button button)
        //{
        //    if (!(position % GameColumns != 0 && position <= (GameColumns * GameRows - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - 1]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinRight(position + 1, button);
        //}

        //private int WinDown(int position, Button button)
        //{
        //    //
        //    if (!(position <= (GameColumns * GameRows - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - GameColumns]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinDown(position + GameColumns, button);
        //}

        //private int WinUpLeft(int position, Button button)
        //{
        //    if (!(position >= 0 && (position + (GameColumns + 1)) % GameColumns != 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + (GameColumns + 1)]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinUpLeft(position - (GameColumns + 1), button);
        //}

        //private int WinDownRight(int position, Button button)
        //{
        //    if (!(position <= (GameColumns * GameRows - 1) && (position - (GameColumns + 1)) % GameColumns != (GameColumns - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - (GameColumns + 1)]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinDownRight(position + (GameColumns + 1), button);
        //}

        //private int WinUpRight(int position, Button button)
        //{
        //    if (!(position >= 0 && (position + (GameColumns - 1)) % GameColumns != (GameColumns - 1) && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position + (GameColumns - 1)]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinUpRight(position - (GameColumns - 1), button);
        //}

        //private int WinDownLeft(int position, Button button)
        //{
        //    if (!(position <= (GameColumns * GameRows - 1) && (position - (GameColumns - 1)) % GameColumns != 0 && CellStates[position] != CellState.Free && CellStates[position] == CellStates[position - (GameColumns - 1)]))
        //    {
        //        return 0;
        //    }
        //    button = GameWindow.Buttons[position];
        //    GraphicsManager.PaintButtonGold(button);
        //    return WinDownLeft(position + (GameColumns - 1), button);
        //}

        //Pinta las celdas ganadoras de color dorado
        private void PlayerWon()
        {
            Winners.ForEach(WinnerCell =>
            {
                GraphicsManager.PaintButtonGold(WinnerCell);
            });
        }

        
        protected enum GameState
        {
            Playing,
            NotStartedOrFinished
        }

    }
}
