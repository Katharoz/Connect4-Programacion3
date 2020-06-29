using Connect4Game.Game_Resources.Multiplayer_Game_Engine;
using Connect4Game.Game_Resources.Singleplayer_Game_Engine;
using Connect4Game.Game_Resources.Graphics_Manager;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Connect4;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Connect4Game.Game_Resources
{
    public enum GameType
    {
        Multiplayer,
        Singleplayer
    }

    public enum GridSize
    {
        Small=7,
        Medium=9,
        Big=11
    }

    public static class GameManager
    {
        private static int GridSize;

        public static Game NewGame(GameType type)
        {
            if (type == GameType.Multiplayer)
            {
                return new MultiplayerGame();
            }
            return new SingleplayerGame();
        }

        public static void GenerateGameGrid(MainWindow gameWindow, GridSize gridSize)
        {
            GridSize = (int)gridSize;

            RemovePreviousGrid(gameWindow);

            CreateGrid(gameWindow);

            CreateColums(gameWindow);

            CreateRows(gameWindow);

            CreateButtons(gameWindow);

            GenerateLabels(gameWindow);

            GenerateEndGameMessage(gameWindow);

            gameWindow.MainGrid.Children.Add(gameWindow.GameGrid);

        }

            private static void RemovePreviousGrid(MainWindow gameWindow)
            {
                gameWindow.MainGrid.Children.Remove(gameWindow.GameGrid);

                gameWindow.GameGrid.Children.Remove(gameWindow.TurnLabel);
                gameWindow.GameGrid.Children.Remove(gameWindow.GameLabel);
                gameWindow.GameGrid.Children.Remove(gameWindow.EndOfGameMessage);
            }

            private static void CreateGrid(MainWindow gameWindow)
            {
                gameWindow.GameGrid = new Grid();
                gameWindow.GameGrid.Visibility = Visibility.Hidden;
                gameWindow.GameGrid.Margin = new Thickness(50);
            }

            private static void CreateColums(MainWindow gameWindow)
            {
                for (int i = 0; i < GridSize; i++)
                {
                    ColumnDefinition coldef = new ColumnDefinition();
                    gameWindow.GameGrid.ColumnDefinitions.Add(coldef);
                }
            }

            private static void CreateRows(MainWindow gameWindow)
            {
                for (int i = 0; i < GridSize; i++)
                {
                    RowDefinition rowdef = new RowDefinition();
                    gameWindow.GameGrid.RowDefinitions.Add(rowdef);
                }
            }

            private static void CreateButtons(MainWindow gameWindow)
            {
                //Se reinicia la lista de botones, volviendose a crear con la cantidad indicada.
                gameWindow.Buttons = new List<Button>();
                for (int i = 1; i < GridSize - 1; i++)
                {
                    for (int j = 1; j < GridSize - 1; j++)
                    {
                        //Por cada celda en el grid, se crea y se le asigna un boton. 
                        Button gridButton = new Button();
                        gridButton.Cursor = Cursors.Hand;
                        gridButton.Click += gameWindow.Button_Click;
                        //gridButton.Content = (i - 1) * (GridSize - 2) + (j - 1);
                        Grid.SetRow(gridButton, i);
                        Grid.SetColumn(gridButton, j);
                        gameWindow.GameGrid.Children.Add(gridButton);
                        GraphicsManager.Paint(gridButton);
                        gameWindow.Buttons.Add(gridButton);
                    }
                }
            }

            private static void GenerateLabels(MainWindow gameWindow)
            {
                TopLabel(gameWindow);
                DownLabel(gameWindow);
                LeftLabel(gameWindow);
                RightLabel(gameWindow);
            }

                private static void TopLabel(MainWindow gameWindow)
                {
                    gameWindow.GameLabel.Content = "!4 En Linea¡";
                    gameWindow.GameLabel.FontSize = 25;
                    gameWindow.GameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                    gameWindow.GameLabel.Foreground = Brushes.Azure;
                    Grid.SetRow(gameWindow.GameLabel, 0);
                    Grid.SetColumnSpan(gameWindow.GameLabel, GridSize);
                    gameWindow.GameGrid.Children.Add(gameWindow.GameLabel);

                }

                private static void DownLabel(MainWindow gameWindow)
                {
                    //Se agregan valores a la label del turno, y se asigna a la ultima fila.
                    //gameWindow.TurnLabel.Content = "Turno Jugador 1";
                    gameWindow.TurnLabel.FontSize = 15;
                    gameWindow.TurnLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                    gameWindow.TurnLabel.Foreground = Brushes.Azure;
                    Grid.SetRow(gameWindow.TurnLabel, GridSize);
                    Grid.SetColumnSpan(gameWindow.TurnLabel, GridSize);
                    gameWindow.GameGrid.Children.Add(gameWindow.TurnLabel);
                }

                private static void LeftLabel(MainWindow gameWindow)
                {
                    Label myIcon = new Label();
                    myIcon.Margin = new Thickness(5);
                    myIcon.Width = 40;
                    myIcon.Background = GraphicsManager.myBrushes[1];
                    Grid.SetColumn(myIcon, 0);
                    Grid.SetRow(myIcon, 1);
                    gameWindow.GameGrid.Children.Add(myIcon);
                    
                }

                private static void RightLabel(MainWindow gameWindow)
                {
                    Label adversaryIcon = new Label();
                    adversaryIcon.Margin = new Thickness(5);
                    adversaryIcon.Width = 40;
                    adversaryIcon.Background = GraphicsManager.myBrushes[2];
                    Grid.SetColumn(adversaryIcon, GridSize);
                    Grid.SetRow(adversaryIcon, 1);
                    gameWindow.GameGrid.Children.Add(adversaryIcon);
                }


            private static void GenerateEndGameMessage(MainWindow gameWindow)
            {
                //Se agregan valores al mensaje de fin y se asigna en el medio-ish del grid.
                //gameWindow.EndOfGameMessage.FontSize = 20;
                gameWindow.EndOfGameMessage.Foreground = Brushes.Azure;
                gameWindow.EndOfGameMessage.Visibility = Visibility.Hidden;
                gameWindow.EndOfGameMessage.Click += gameWindow.EndGame_Click;
                gameWindow.EndOfGameMessage.Cursor = Cursors.Hand;
                Grid.SetColumn(gameWindow.EndOfGameMessage, (int)Math.Floor((double)(GridSize / 3)));
                Grid.SetColumnSpan(gameWindow.EndOfGameMessage, 3);
                Grid.SetRow(gameWindow.EndOfGameMessage, (int)Math.Floor((double)(GridSize / 3)));
                Grid.SetRowSpan(gameWindow.EndOfGameMessage, 2);
                Grid.SetZIndex(gameWindow.EndOfGameMessage, 1);
                gameWindow.GameGrid.Children.Add(gameWindow.EndOfGameMessage);
            }

        public static void UpdateTurnLabel(Label turnLabel, bool playerTurn)
        {
            turnLabel.Content = playerTurn ? "Turno Jugador 2" : "Turno Jugador 1";
        }
    }
}
