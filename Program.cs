using System;
using System.Collections.Generic;
using RetroSnakeGame;
using System.IO;

Coord gridDimensions = new Coord(50, 20);
Coord snakePos = new Coord(10, 2);
Random rand = new Random();
Coord foodPos = new Coord(rand.Next(1, gridDimensions.X - 1), rand.Next(1, gridDimensions.Y - 1));
int verticalFrameRate = 150;
int horizontalFrameRate = 100;
Direction moveDirection = Direction.Down;
int score = 0;



List<Coord> snakePosHistory = new List<Coord>();
List<int> scoreHistory = new List<int>();
string scoreHistoryFilePath = "scoreHistory.txt";
int snakeLength = 1;




void SaveScores()
{
    File.WriteAllLines(scoreHistoryFilePath, scoreHistory.ConvertAll(score => score.ToString()));
}

void LoadScores()
{
    if (File.Exists(scoreHistoryFilePath))
    {
        scoreHistory = new List<int>(Array.ConvertAll(File.ReadAllLines(scoreHistoryFilePath), int.Parse));
    }
}
void ResetGame()
{
    score = 0;
    snakePos = new Coord(10, 2);
    snakePosHistory.Clear();
    snakeLength = 1;
    moveDirection = Direction.Down;
    foodPos = new Coord(rand.Next(1, gridDimensions.X - 1), rand.Next(1, gridDimensions.Y - 1));
}

LoadScores();
while (true)
{
    Console.Clear();
    Console.WriteLine("Score: " + score);
    snakePos.ApplyDirection(moveDirection);

    for (int y = 0; y < gridDimensions.Y; y++)
    {
        for (int x = 0; x < gridDimensions.X; x++)
        {
            Coord currentCoord = new Coord(x, y);
            if (snakePos.Equals(currentCoord) || snakePosHistory.Contains(currentCoord))
            {
                Console.Write("■");
            }
            else if (foodPos.Equals(currentCoord))
            {
                Console.Write("$");
            }
            else if (x == 0 || y == 0 || x == gridDimensions.X - 1 || y == gridDimensions.Y - 1)
            {
                Console.Write("\u2588");
            }
            else
            {
                Console.Write(" ");
            }
        }
        Console.WriteLine();
    }

    if (snakePos.Equals(foodPos))
    {
        snakeLength++;
        score += 15;
        foodPos = new Coord(rand.Next(1, gridDimensions.X - 1), rand.Next(1, gridDimensions.Y - 1));
        
    }
    else if (snakePos.X == 0 || snakePos.X == gridDimensions.X - 1 || snakePos.Y == 0 || snakePos.Y == gridDimensions.Y - 1 || snakePosHistory.Contains(snakePos))
    {
        Console.WriteLine("Game Over!");
        Console.WriteLine("Your score is: " + score);
        scoreHistory.Add(score);
        scoreHistory.Sort((a, b) => b.CompareTo(a));
        Console.WriteLine("Score History: " + string.Join(", ", scoreHistory));
        SaveScores();
        Console.WriteLine("Press any key to play again");
        Console.ReadKey();
        ResetGame();
        continue;
    }

    snakePosHistory.Add(new Coord(snakePos.X, snakePos.Y));
    if (snakePosHistory.Count > snakeLength)
    {
        snakePosHistory.RemoveAt(0);
    }
    DateTime start = DateTime.Now;
    int currentFrameRate = (moveDirection == Direction.Up || moveDirection == Direction.Down) ? verticalFrameRate : horizontalFrameRate;
    while ((DateTime.Now - start).Milliseconds < currentFrameRate)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (moveDirection != Direction.Down) moveDirection = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (moveDirection != Direction.Up) moveDirection = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if (moveDirection != Direction.Right) moveDirection = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (moveDirection != Direction.Left) moveDirection = Direction.Right;
                    break;
            }
        }
    }
}