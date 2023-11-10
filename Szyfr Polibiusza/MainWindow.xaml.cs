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

            int shift = int.Parse(ShiftTextBox2.Text);
            string ciphertext = EncryptedTextBox.Text;
            string decryptedText = PolibiusCipher.Decrypt(ciphertext, shift);
            DecryptedTextBlock.Text = decryptedText;
        }
    }
    public static class PolibiusCipher
    {
        static string polibiusAlphabet = "ABCDEFGHIKLMNOPRSTUVWXYZĄĆĘŁŃÓŚŹŻ";
        static int tableSize = 6;

        public static string Encrypt(string plaintext, int shift)
        {
            //plaintext = RemoveDiacritics(plaintext.ToUpper());
            //string encryptedText = "";

            //foreach (char c in plaintext)
            //{
            //    if (c == 'J')
            //        encryptedText += "24 ";
            //    else if (c == 'Ó')
            //        encryptedText += "54 ";
            //    else if (Char.IsLetter(c))
            //    {
            //        int index = (polibiusAlphabet.IndexOf(c) + shift) % polibiusAlphabet.Length;
            //        int row = index / tableSize;
            //        int col = index % tableSize;
            //        encryptedText += (row + 1).ToString() + (col + 1).ToString() + " ";
            //    }
            //    else if (c == ' ')
            //    {
            //        encryptedText += " ";
            //    }
            //}

            //return encryptedText.Trim();
            plaintext = RemoveDiacritics(plaintext.ToUpper());
            string encryptedText = "";

            foreach (char c in plaintext)
            {
                if (Char.IsLetter(c))
                {
                    int index = (polibiusAlphabet.IndexOf(c) + shift) % polibiusAlphabet.Length;
                    int row = index / tableSize + 1;
                    int col = index % tableSize + 1;
                    encryptedText += row.ToString() + col.ToString() + " ";
                }
                else if (Char.IsWhiteSpace(c))
                {
                    // Jeśli to spacja, dodaj spację
                    encryptedText += " ";
                }
                else
                {
                    // Jeśli to nie litera ani spacja, dodaj bez zmian
                    encryptedText += c;
                }
            }

            return encryptedText.Trim();
        }

        public static string Decrypt(string ciphertext, int shift)
        {
            string[] pairs = ciphertext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string decryptedText = "";

            foreach (string pair in pairs)
            {
                if (pair.Length == 2)
                {
                    int row = int.Parse(pair[0].ToString()) - 1;
                    int col = int.Parse(pair[1].ToString()) - 1;

                    int index = row * tableSize + col - shift;
                    while (index < 0)
                    {
                        index += polibiusAlphabet.Length;
                    }

                    char decryptedChar = polibiusAlphabet[index % polibiusAlphabet.Length];

                    if (decryptedChar == 'J')
                    {
                        decryptedText += "J";
                    }
                    else if (decryptedChar == 'Ó')
                    {
                        decryptedText += "Ó";
                    }
                    else
                    {
                        decryptedText += decryptedChar;
                    }
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

    //public static class PolibiusCipher
    //{
    //    static char[,] polibiusTable = new char[6, 6]
    //    {
    //        { 'A', 'Ą', 'B', 'C', 'Ć', 'D' },
    //        { 'E', 'Ę', 'F', 'G', 'H', 'I' },
    //        { 'J', 'K', 'L', 'Ł', 'M', 'N' },
    //        { 'O', 'Ó', 'P', 'Q', 'R', 'S' },
    //        { 'T', 'U', 'V', 'W', 'X', 'Y' },
    //        { 'Z', 'Ź', 'Ż', ' ', ' ', ' ' }
    //    };

    //    public static string Encrypt(string plaintext, int shift)
    //    {
    //        plaintext = RemoveDiacritics(plaintext.ToUpper());
    //        string encryptedText = "";

    //        foreach (char c in plaintext)
    //        {
    //            if (c == 'J')
    //                encryptedText += "24";
    //            else if (Char.IsLetter(c))
    //            {
    //                bool found = false; // Flaga, aby określić, czy litera została znaleziona w tabeli
    //                for (int row = 0; row < 6; row++)
    //                {
    //                    for (int col = 0; col < 6; col++)
    //                    {
    //                        if (polibiusTable[row, col] == c)
    //                        {
    //                            row = (row + shift) % 6;
    //                            col = (col + shift) % 6;
    //                            encryptedText += (row + 1).ToString() + (col + 1).ToString() + " ";
    //                            found = true;
    //                            break;
    //                        }
    //                    }
    //                    if (found) break;
    //                }
    //                if (!found)
    //                {
    //                    // Jeśli litera nie została znaleziona, dodaj ją bez zmian
    //                    encryptedText += c;
    //                }
    //            }
    //            else if (c == ' ')
    //            {
    //                encryptedText += " ";
    //            }
    //        }

    //        return encryptedText.Trim();
    //    }

    //    public static string Decrypt(string ciphertext, int shift)
    //    {
    //        string decryptedText = "";
    //        string[] pairs = ciphertext.Split(' ');

    //        foreach (string pair in pairs)
    //        {
    //            if (pair.Length == 2)
    //            {
    //                int row = int.Parse(pair[0].ToString()) - 1;
    //                int col = int.Parse(pair[1].ToString()) - 1;
    //                row = (row - shift + 6) % 6;
    //                col = (col - shift + 6) % 6;
    //                decryptedText += polibiusTable[row, col];
    //            }
    //            else if (pair == " ")
    //            {
    //                decryptedText += " ";
    //            }
    //        }
    //        return decryptedText;
    //    }

    //    public static string RemoveDiacritics(string text)
    //    {
    //        string normalized = text.Normalize(NormalizationForm.FormD);
    //        StringBuilder result = new StringBuilder();

    //        foreach (char c in normalized)
    //        {
    //            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
    //            {
    //                result.Append(c);
    //            }
    //        }

    //        return result.ToString();
    //    }
    //}



}
