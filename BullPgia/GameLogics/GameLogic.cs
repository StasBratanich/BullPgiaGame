using System;
using System.Text;

namespace GameLogics
{
    public class GameLogic
    {
        public byte m_numOfattempts;
        public byte m_currentRound = 0;
        public string randomString = GenerateRandomString(4, 'a', 'h');
        static string GenerateRandomString(int length, char minValue, char maxValue)
        {
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);
            string characters = "";

            for (char c = minValue; c <= maxValue; c++)
                characters += c;
            
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                char randomChar = characters[index];
                characters = characters.Remove(index, 1);
                result.Append(randomChar);
            }

            return result.ToString();
        }
        public bool IsGameLost()
        {
            bool isGameLostFlag = false;
            if (!(m_currentRound < m_numOfattempts))
                isGameLostFlag = true;
            return isGameLostFlag;
        }
        public bool IsGameWon(string guess)
        {
            guess = guess.ToLower();
            bool isGameWonFlag = false;
            if (guess.Equals(randomString))
                isGameWonFlag = true;
            return isGameWonFlag;
        }
    }
}
