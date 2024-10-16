// 0xRETRO https://github.com/0xr3tro
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WORDLE_v2
{
    public partial class MainWindow : Window
    {
        // Lista słów
        private List<string> listaSlow = new List<string> { "cloud", "apple", "brave", "house", "light", "train", "water", "bread", "plane", "heart" };

        private string wylosowaneSlowo;
        private int pozostaleProby = 5;
        private int aktualnyRzad = 0;

        public MainWindow()
        {
            InitializeComponent();
            LosujSlowo();
        }

        // Losowanie słowa z listy
        private void LosujSlowo()
        {
            Random rand = new Random();
            wylosowaneSlowo = listaSlow[rand.Next(listaSlow.Count)];
            pozostaleProby = 5;
            aktualnyRzad = 0;
            ClearGrid();
            GameMessage.Text = "Masz 5 prób na odgadnięcie słowa!";
        }

        // Czyszczenie siatki (resetowanie gry)
        private void ClearGrid()
        {
            foreach (UIElement element in GridLetters.Children)
            {
                if (element is Border border && border.Child is TextBlock cell)
                {
                    cell.Text = " ";
                    cell.Background = Brushes.White;
                }
            }
        }

        // Sprawdzenie słowa wpisanego przez gracza
        private void CheckWord(object sender, RoutedEventArgs e)
        {
            string slowoGracza = WordInput.Text.ToLower();

            // Sprawdzenie długości słowa
            if (slowoGracza.Length != 5)
            {
                GameMessage.Text = "Słowo musi mieć dokładnie 5 liter!";
                return;
            }

            // Sprawdzenie, czy słowo jest w liście i czy nie zawiera cyfr
            if (!listaSlow.Contains(slowoGracza))
            {
                GameMessage.Text = "Słowo nie znajduje się na liście!";
                return;
            }

            if (slowoGracza.Any(char.IsDigit))
            {
                GameMessage.Text = "Słowo nie może zawierać cyfr!";
                return;
            }

            if (pozostaleProby <= 0)
            {
                GameMessage.Text = $"Koniec gry! Prawidłowe słowo to: {wylosowaneSlowo}";
                LosujSlowo();
                return;
            }

            pozostaleProby--;

            // Sprawdzenie i podświetlenie liter w siatce
            for (int i = 0; i < 5; i++)
            {
                Border border = (Border)GridLetters.Children[aktualnyRzad * 5 + i];
                TextBlock cell = (TextBlock)border.Child;
                cell.Text = slowoGracza[i].ToString();

                if (slowoGracza[i] == wylosowaneSlowo[i])
                {
                    cell.Background = Brushes.Green; // Litera w dobrym miejscu
                }
                else if (wylosowaneSlowo.Contains(slowoGracza[i]))
                {
                    cell.Background = Brushes.Yellow; // Litera w złym miejscu
                }
                else
                {
                    cell.Background = Brushes.Red; // Litera nie występuje w słowie
                }
            }

            // Sprawdzenie wyniku
            if (slowoGracza == wylosowaneSlowo)
            {
                GameMessage.Text = "Gratulacje! Odgadłeś słowo!";
                LosujSlowo();
            }
            else if (pozostaleProby == 0)
            {
                GameMessage.Text = $"Niestety, nie odgadłeś. Prawidłowe słowo to: {wylosowaneSlowo}";
                LosujSlowo();
            }
            else
            {
                aktualnyRzad++;
                GameMessage.Text = $"Pozostało prób: {pozostaleProby}";
            }

            WordInput.Clear(); // Czyszczenie pola do wpisania słowa
        }
    }
}