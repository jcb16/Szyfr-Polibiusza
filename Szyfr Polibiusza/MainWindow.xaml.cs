using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace Szyfr_Polibiusza
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            int shift = int.Parse(ShiftTextBox.Text);
            string plaintext = PlainTextBox.Text;
            string encryptedText = PolibiusCipher.Encrypt(plaintext, shift);
            EncryptedTextBlock.Text = encryptedText;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {

            int shift = int.Parse(ShiftTextBox2.Text);
            string ciphertext = EncryptedTextBox.Text;
            string decryptedText = PolibiusCipher.Decrypt(ciphertext, shift);
            DecryptedTextBlock.Text = decryptedText;
        }
    }

    public static class PolibiusCipher
    {
        public static string Encrypt(string plaintext, int shift)
        {

            char[,] polibiuszTable = {
            {'A', 'Ą', 'B', 'C', 'Ć', 'D'},
            {'E', 'Ę', 'F', 'G', 'H', 'I'},
            {'J', 'K', 'L', 'Ł', 'M', 'N'},
            {'Ń','O', 'Ó', 'P', 'Q', 'R'},
            {'S','Ś','T', 'U', 'V', 'W'},
            {'X','Y','Z', 'Ź', 'Ż', ' ',}
            };

            plaintext = plaintext.ToUpper();
            List<string> encryptedTextList = new List<string>();

            foreach (char character in plaintext)
            {
                if (character == 'J')
                {
                    encryptedTextList.Add("24");
                }
                else if (character == ' ')
                {
                    encryptedTextList.Add(" ");
                }
                else
                {
                    bool found = false;
                    for (int i = 0; i < polibiuszTable.GetLength(0); i++)
                    {
                        for (int j = 0; j < polibiuszTable.GetLength(1); j++)
                        {
                            if (character == polibiuszTable[i, j])
                            {
                                int row = (i + shift) % polibiuszTable.GetLength(0);
                                int col = (j + shift) % polibiuszTable.GetLength(1);
                                encryptedTextList.Add($"{row}{col}");
                                found = true;
                                break;
                            }
                        }
                        if (found) break;
                    }
                }
            }

            return string.Join(" ", encryptedTextList);
        }

        public static string Decrypt(string ciphertext, int shift)
        {
            char[,] polibiuszTable = {
            {'A', 'Ą', 'B', 'C', 'Ć', 'D'},
            {'E', 'Ę', 'F', 'G', 'H', 'I'},
            {'J', 'K', 'L', 'Ł', 'M', 'N'},
            {'Ń','O', 'Ó', 'P', 'Q', 'R'},
            {'S','Ś','T', 'U', 'V', 'W'},
            {'X','Y','Z', 'Ź', 'Ż', ' ',}
            };


            // Usunięcie spacji i podzielenie tekstu na pary znaków
            string[] cipherParts = ciphertext.Split(' ');
            List<string> decryptedTextList = new List<string>();

            foreach (string cipherPart in cipherParts)
            {
                if (cipherPart == " ")
                {
                    decryptedTextList.Add(" ");
                }
                else
                {
                    int row = int.Parse(cipherPart[0].ToString());
                    int col = int.Parse(cipherPart[1].ToString());

                    // Uwzględnienie przesunięcia i zapewnienie prawidłowych indeksów
                    row = (row - shift + polibiuszTable.GetLength(0)) % polibiuszTable.GetLength(0);
                    col = (col - shift + polibiuszTable.GetLength(1)) % polibiuszTable.GetLength(1);

                    decryptedTextList.Add(polibiuszTable[row, col].ToString());
                }
            }

            return string.Join("", decryptedTextList);
        }
    }




}