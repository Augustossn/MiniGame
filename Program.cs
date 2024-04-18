using System;
using System.Threading.Tasks;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

int playerX = 0;
int playerY = 0;

int foodX = 0;
int foodY = 0;

string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

string player = states[0];

int food = 0;

int playerSpeed = 1;

InitializeGame();
while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.Write("Console was resized. Program exiting.");
        shouldExit = true;
    }
    else
    {
        if (PlayerIsFaster())
        {
            Move(playerSpeed, false);
        }
        else if (PlayerIsSick())
        {
            FreezePlayer();
        }
        else
        {
            Move(otherKeysExit: false);
        }
        if (GotFood())
        {
            ChangePlayer();
            ShowFood();
        }
    }
}

bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

void ShowFood()
{
    food = random.Next(0, foods.Length);

    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}
bool GotFood()
{
    return playerY == foodY && playerX == foodX;
}
bool PlayerIsSick()
{
    return player.Equals(states[2]);
}

bool PlayerIsFaster()
{
    return player.Equals(states[1]);
}

async void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    if (food == 1)
    {
        playerSpeed = 3;
        await Task.Delay(5000);
        playerSpeed = 1;
    }
}

void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

void Move(int speed = 1, bool otherKeysExit = false)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.W:
            playerY--;
            break;
        case ConsoleKey.S:
            playerY++;
            break;
        case ConsoleKey.A:
            playerX -= speed;
            break;
        case ConsoleKey.D:
            playerX += speed;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            shouldExit = otherKeysExit;
            break;
    }

    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
