using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WORDLE_v._2
{
    public partial class MainWindow : Window
    {
        private List<string> listaSlow = new List<string> { "cloud", "apple", "brave", "house", "light", "train", "water", "bread", "plane", "heart" };
        private string wylosowaneSlowo;
        private int pozostaleProby = 5;
        private int aktualnyRzad = 0; // Wskazuje, w którym rzędzie aktualnie gracz wpisuje słowo

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
        }

        // Czyszczenie siatki (resetowanie gry)
        private void ClearGrid()
        {
            for (int i = 0; i < 25; i++)
            {
                TextBlock cell = (TextBlock)GridLetters.Children[i];
                cell.Text = " ";
                cell.Background = Brushes.White;
            }
        }

        // Sprawdzenie słowa wpisanego przez gracza
        private void CheckWord(object sender, RoutedEventArgs e)
        {
            string slowoGracza = WordInput.Text.ToLower();

            // Sprawdzenie długości słowa
            if (slowoGracza.Length != 5)
            {
                MessageBox.Show("Słowo musi mieć 5 liter!");
                return;
            }

            if (pozostaleProby <= 0)
            {
                MessageBox.Show($"Nie udało się odgadnąć! Prawidłowe słowo to: {wylosowaneSlowo}");
                LosujSlowo();
                return;
            }

            pozostaleProby--;

            // Sprawdzenie słowa i podświetlenie liter
            for (int i = 0; i < 5; i++)
            {
                TextBlock cell = (TextBlock)GridLetters.Children[aktualnyRzad * 5 + i];
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

            if (slowoGracza == wylosowaneSlowo)
            {
                MessageBox.Show("Gratulacje! Odgadłeś słowo!");
                LosujSlowo();
            }
            else if (pozostaleProby == 0)
            {
                MessageBox.Show($"Niestety, nie odgadłeś. Prawidłowe słowo to: {wylosowaneSlowo}");
                LosujSlowo();
            }
            else
            {
                aktualnyRzad++;
            }

            WordInput.Clear(); // Czyszczenie pola do wpisania słowa
        }
    }
}