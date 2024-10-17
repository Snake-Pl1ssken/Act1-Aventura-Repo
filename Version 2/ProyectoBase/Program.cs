using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Text;

namespace AdventureEngine
{
    internal enum CommandId
    {
        showDialog,
        setBackground,
        playMusic,
        stopMusic,
        wait
    }

    internal struct Command
    {
        public CommandId id;
        public string param1;
        public string param2;
        public string param3;
        public string param4;
        public string param5;
        public string param6;
    }

    internal static class Program
    {
        // ConstantsConfigs

        static int screenWidth = 1280;
        static int screenHeight = 720;
        
        static string storyTitle = "El secreto de la señora Dorotea";
       
        static int characterX = 720;
        static int characterY = 580;
        static int dialogBaseX = 30;
        static int dialogBaseY = 526;
        static int dialogNameX = 90;
        static int dialogNameY = 544;
        static int dialogContentX = 90;
        static int dialogContentY = 600;

 

        // Characters

        static Texture basilioTexture;
        static Texture carmeloTexture;
        static Texture doroteaTexture;
        static Texture nicolasTexture;

        static Sprite characterSprite;

        // Background

        static Texture armeriaTexture;
        static Texture castilloTexture;
        static Texture comedorTexture;
        static Texture dormitorioTexture;
        static Texture mazmorraTexture;
        static Texture vestibuloTexture;

        static Sprite backgroundSprite;

        // Music

        static Music animadaMusic;
        static Music normalMusic;
        static Music tensaMusic;


        // Dialog

        static Texture dialogBaseTexture;
        static Sprite dialogBaseSprite;

        static Font dialogFont;

        static Text dialogNameText;
        static Text dialogContentText;


        static bool dialogVisible;

        // Commands

        static bool waitingForCommand;
        static int commandIndex;

        static Clock clock;
        static float waitTime;

        static bool continuePressed;
        
        
        static Command[] commands =
        {
            new Command() { id = CommandId.setBackground, param1 = "castillo" },
            new Command() { id = CommandId.playMusic, param1 = "normal" },
            new Command() { id = CommandId.wait, param1 = "1.0" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Buenos días" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Soy Nicolás, el mayordomo de la señora Dorotea" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Permítame llevar su equipaje" },
            new Command() { id = CommandId.wait, param1 = "1" },
            new Command() { id = CommandId.setBackground, param1 = "vestibulo" },
            new Command() { id = CommandId.wait, param1 = "1" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Éste es el vestíbulo del castillo" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Espere aquí. La señora Dorotea vendrá en cualquier momento" },
            new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Si me disculpa, llevaré su equipaje a su dormitorio" },
            new Command() { id = CommandId.wait, param1 = "3" },
            new Command() { id = CommandId.playMusic, param1 = "tensa" },
            new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "¿Quién es usted?" },
            new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Bueno, en realidad me da igual" },
            new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Vaya a buscar a ese inútil de Nicolás porque tenemos un problema" },
            new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "LA SEÑORA DOROTEA HA DESAPARECIDO" }

        };

        static void Main()
        {
            //Config initialization

            StreamReader Config;

            Config = new StreamReader("config.ini", Encoding.UTF8);

            while (!Config.EndOfStream)
            {
                string line = Config.ReadLine();
                string[] parts = line.Split('=');

                line = line.ToUpper();

                Console.WriteLine(line);

                if (parts[0] == "screenWidth")
                {
                    screenWidth = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "screenHeight")
                {
                    screenHeight = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "storyTitle")
                {
                    storyTitle = parts[1];
                }
                else if (parts[0] == "characterX")
                {
                    characterX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "characterY")
                {
                    characterY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogBaseX")
                {
                    dialogBaseX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogBaseY")
                {
                    dialogBaseY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogNameX")
                {
                    dialogNameX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogNameY")
                {
                    dialogNameY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogContentX")
                {
                    dialogContentX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogContentY")
                {
                    dialogContentY = Int32.Parse(parts[1]);
                }
            }



            //*********************************************************************************************************************************************************************
            //DICCIONARIO INICIO

            Dictionary<string, Texture> CharacterTextures = new Dictionary<string, Texture>();

            Dictionary<string, Texture> BackgroundTextures = new Dictionary<string, Texture>();

            Dictionary<string, Music> MusicAudio = new Dictionary<string, Music>();

            //DICCIONARIO FIN
            //*********************************************************************************************************************************************************************


            //*********************************************************************************************************************************************************************
            //ENTRADA RUTAS

            string rutaOriginal, Characters, Backgrounds, Musics;

            rutaOriginal = Directory.GetCurrentDirectory();

            Characters = Directory.GetCurrentDirectory() + "\\characters";

            Backgrounds = Directory.GetCurrentDirectory() + "\\backgrounds";

            Musics = Directory.GetCurrentDirectory() + "\\musics";

            string[] ficheros;
            string[] ficherosBG;
            string[] ficherosMS;

            ficheros = Directory.GetFiles(Characters);
            ficherosBG = Directory.GetFiles(Backgrounds);
            ficherosMS = Directory.GetFiles(Musics);

            for (int i = 0; i < ficheros.Length; i++)
            {
                int posicion;
                string nombre;

                posicion = ficheros[i].LastIndexOf('\\');

                nombre = ficheros[i].Substring(posicion + 1);

                string[] Character = nombre.Split('.');

                CharacterTextures[Character[0]] = new Texture(ficheros[i]);

            }
            for (int i = 0; i < ficherosBG.Length; i++)
            {
                int posicion;
                string nombre;

                posicion = ficherosBG[i].LastIndexOf('\\');

                nombre = ficherosBG[i].Substring(posicion + 1);

                string[] Background = nombre.Split('.');

                BackgroundTextures[Background[0]] = new Texture(ficherosBG[i]);
            }

            for (int i = 0; i < ficherosMS.Length; i++)
            {
                int posicion;
                string nombre;

                posicion = ficherosMS[i].LastIndexOf('\\');

                nombre = ficherosMS[i].Substring(posicion + 1);

                string[] Music = nombre.Split('.');

                MusicAudio[Music[0]] = new Music(ficherosMS[i]);
            }

            //SALLIDA RUTAS
            //*********************************************************************************************************************************************************************


            // Window initialization

            var mode = new VideoMode((uint)screenWidth, (uint)screenHeight);
            var window = new RenderWindow(mode, storyTitle);
            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMousePressed;

            Texture CHt = CharacterTextures.First().Value;
            characterSprite = new Sprite();
            characterSprite.Texture = CHt;
            characterSprite.Origin = new Vector2f(CHt.Size.X / 2, CHt.Size.Y);
            characterSprite.Position = new Vector2f(characterX, characterY);

            Texture BckG = BackgroundTextures.First().Value;

            backgroundSprite = new Sprite();
            backgroundSprite.Texture = BckG;
            backgroundSprite.Position = new Vector2f(0, 0);

            // Dialog initialization

            dialogBaseTexture = new Texture("dialog\\base.png");

            dialogBaseSprite = new Sprite();
            dialogBaseSprite.Texture = dialogBaseTexture;
            dialogBaseSprite.Position = new Vector2f(dialogBaseX, dialogBaseY);

            dialogFont = new Font("fonts\\default.ttf");

            dialogNameText = new Text("", dialogFont);
            dialogNameText.Position = new Vector2f(dialogNameX, dialogNameY);

            dialogContentText = new Text("", dialogFont);
            dialogContentText.Position = new Vector2f(dialogContentX, dialogContentY);

            // Commands initialization

            commandIndex = 0;

            clock = new Clock();

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();

                // Game logic goes here

                if (waitingForCommand)
                {
                    Command command = commands[commandIndex];

                    if(command.id == CommandId.wait)
                    {
                        if(clock.ElapsedTime.AsSeconds() >= waitTime || continuePressed)
                        {
                            waitingForCommand = false;
                            commandIndex ++;
                        }
                    }
                    else if(command.id == CommandId.showDialog) 
                    {
                        if(continuePressed)
                        {
                            dialogVisible = false;
                            waitingForCommand = false;
                            commandIndex ++;
                        }
                    }
                }
                else if(commandIndex < commands.Length) //quedan comandos?
                {
                    Command command = commands[commandIndex]; //leer comando

                    if(command.id == CommandId.setBackground)
                    {

                        backgroundSprite.Texture = BackgroundTextures[command.param1];
                        commandIndex++;

                    }
                    else if(command.id == CommandId.playMusic)
                    {

                        foreach(KeyValuePair<string, Music> ControlMusic in MusicAudio)
                        {
                            ControlMusic.Value.Stop();
                        }

                        MusicAudio[command.param1].Loop = true;
                        MusicAudio[command.param1].Play();
                        commandIndex++;
                    }
                    else if(command.id == CommandId.stopMusic)
                    {
                        normalMusic.Stop();
                        animadaMusic.Stop();
                        tensaMusic.Stop();

                        commandIndex++;
                    }
                    else if (command.id == CommandId.showDialog)
                    {
                        characterSprite.Texture = CharacterTextures[command.param1]; 
                        
                        dialogNameText = new Text(command.param1, dialogFont);
                        dialogNameText.Position = new Vector2f(100, 540);

                        dialogContentText = new Text(command.param2, dialogFont);
                        dialogContentText.Position = new Vector2f(100, 600);

                        dialogVisible = true;

                        waitingForCommand = true;
                    }
                    else if(command.id == CommandId.wait)
                    {
                        waitTime = Single.Parse(command.param1);
                        clock.Restart();
                        waitingForCommand = true;
                    }

                }

                window.Draw(backgroundSprite);

                if(dialogVisible)
                {
                    window.Draw(characterSprite);
                    window.Draw(dialogBaseSprite);
                    window.Draw(dialogNameText);
                    window.Draw(dialogContentText);
                }

                window.Display();

                continuePressed = false;

            }
        }

        static void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                continuePressed = true;
            }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if(e.Code == Keyboard.Key.Space)
            {
                continuePressed = true;
            }
        }
    }


}
