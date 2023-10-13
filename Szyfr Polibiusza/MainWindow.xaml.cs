using System;
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
            //if (EncryptedTextBox.Text != null && ShiftTextBox2.Text != null)
            //{
                int shift = int.Parse(ShiftTextBox2.Text);
                string ciphertext = EncryptedTextBox.Text;
                string decryptedText = PolibiusCipher.Decrypt(ciphertext, shift);
                DecryptedTextBlock.Text = decryptedText;
            //}
            //else
            //{
            //    MessageBox.Show("Wpisz brakujące dane!");
            //}
        }
    }

    public static class PolibiusCipher
    {
        static char[,] polibiusTable = new char[6, 6]
        {
            { 'A', 'Ą', 'B', 'C', 'Ć', 'D' },
            { 'E', 'Ę', 'F', 'G', 'H', 'I' },
            { 'J', 'K', 'L', 'Ł', 'M', 'N' },
            { 'O', 'Ó', 'P', 'Q', 'R', 'S' },
            { 'T', 'U', 'V', 'W', 'X', 'Y' },
            { 'Z', 'Ź', 'Ż', ' ', ' ', ' ' }
        };

        public static string Encrypt(string plaintext, int shift)
        {
            plaintext = RemoveDiacritics(plaintext.ToUpper());
            string encryptedText = "";

            foreach (char c in plaintext)
            {
                if (c == 'J')
                    encryptedText += "24"; 
                else if (Char.IsLetter(c))
                {
                    bool found = false; // Flaga, aby określić, czy litera została znaleziona w tabeli
                    for (int row = 0; row < 6; row++)
                    {
                        for (int col = 0; col < 6; col++)
                        {
                            if (polibiusTable[row, col] == c)
                            {
                                row = (row + shift) % 6;
                                col = (col + shift) % 6;
                                encryptedText += (row + 1).ToString() + (col + 1).ToString() + " ";
                                found = true;
                                break;
                            }
                        }
                        if (found) break;
                    }
                    if (!found)
                    {
                        // Jeśli litera nie została znaleziona, dodaj ją bez zmian
                        encryptedText += c;
                    }
                }
                else if (c == ' ')
                {
                    encryptedText += " ";
                }
            }

            return encryptedText.Trim();
        }

        public static string Decrypt(string ciphertext, int shift)
        {
            string decryptedText = "";
            string[] pairs = ciphertext.Split(' ');

            foreach (string pair in pairs)
            {
                if (pair.Length == 2)
                {
                    int row = int.Parse(pair[0].ToString()) - 1;
                    int col = int.Parse(pair[1].ToString()) - 1;
                    row = (row - shift + 6) % 6;
                    col = (col - shift + 6) % 6;
                    decryptedText += polibiusTable[row, col];
                }
                else if (pair == " ")
                {
                    decryptedText += " ";
                }
            }
            return decryptedText;
        }

        public static string RemoveDiacritics(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder result = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
