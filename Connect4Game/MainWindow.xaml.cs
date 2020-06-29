using Connect4Game.Game_Resources;
using Connect4Game.Game_Resources.Graphics_Manager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        /// <summary>
        /// Variables que se usan en la clase.
        /// </summary>
        #region PRIVATE VARIABLES

        //Instancia del juego
        private Game Game;

        //Tamaño del grid del juego seleccionado
        private GridSize SelectedGridSize;

        //Label que indica de quien es el turno.
        public Label TurnLabel { get; set; } = new Label();

        //Label de Juego.
        public Label GameLabel { get; set; } = new Label();

        //Boton que aparece al final del juego.
        public Button EndOfGameMessage { get; set; } = new Button();

        //Lista de botones que se generan con el grid de juego.
        public List<Button> Buttons { get; set; }
        #endregion


        //La Ventana del juego.
        public MainWindow()
        {
            InitializeComponent();

            SelectedGridSize = GridSize.Medium; //por defecto el tamaño del grid es de 7x7

            GraphicsManager.LoadImages(this);

            MainMenu();
        }

        public void MainMenu()
        {
            //TODO erase this, fix second win bug;
            if (MenuGrid.Visibility == Visibility.Hidden)
            {
                MenuGrid.Visibility = Visibility.Visible;
                GameGrid.Visibility = Visibility.Hidden;
            }
            GameWindow.Width = 400;

            //Se asigna una imagen de boton.
            GraphicsManager.InitButtons(this);
        }


        /// <summary>
        /// Eventos que ocurren al presionar botones
        /// </summary>
        /// <param name="sender"> El boton presionado </param>
        /// <param name="e"> El evento </param>
        #region BUTTON EVENTS

        //Boton de Inicio en el Menu de juego.
        private void SinglePlayer_OnClick(object sender, RoutedEventArgs e)
        {
            StartNewGame(GameType.Singleplayer, SelectedGridSize);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            int position = Grid.GetColumn((Button)sender) - 1;
            Game.CheckNewPlay(position);
          
        }

        //Boton de fin de juego.
        public void EndGame_Click(object sender, RoutedEventArgs e)
        {
            //Muestra el menu y esconde los botones de juego.
            GraphicsManager.SwitchVisibility(this);
            //Esconde el boton de fin de juego.
            Game.HideEndGameMessage();
            //Vuelve a activar los botones para la siguiente ronda.
            MainMenu();
        }


        private void SmallGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Small; //5x5
            SmallGrid.Opacity = 1;
            MediumGrid.Opacity = 0.75;
            LargeGrid.Opacity = 0.75;
        }

        private void MediumGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Medium; //7x7
            SmallGrid.Opacity = 0.75;
            MediumGrid.Opacity = 1;
            LargeGrid.Opacity = 0.75;
        }

        private void BigGrid_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedGridSize = GridSize.Big; //9x9
            SmallGrid.Opacity = 0.75;
            MediumGrid.Opacity = 0.75;
            LargeGrid.Opacity = 1;
        }

        //Boton que elige modo un jugador.
        private void MultiPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedGridSize != GridSize.Medium) //Solo se puede jugar de a 2 con el tamaño de grid mediano
            {
                MessageBox.Show("Debes seleccionar el tamaño 'Mediano' para acceder a una partida de 2 jugadores", 
                                "Tamaño no disponible", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                return;
            }
            StartNewGame(GameType.Multiplayer, SelectedGridSize);
        }
        
        private void StartNewGame(GameType gameType, GridSize selectedSize)
        {
            Game = GameManager.NewGame(gameType);
            Game.GameWindow = this;
            Game.CreateGame(selectedSize);
            Game.SetPlayerName(PlayerNameTextBox.Text);
            GraphicsManager.SwitchVisibility(this);
            GameWindow.Width = 800;
            Game.StartGame();
        }


        //Cierra la ventana
        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
        
    }
}