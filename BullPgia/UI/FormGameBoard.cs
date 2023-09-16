using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLogics;

namespace GameBoard
{
    partial class FormGameBoard : Form
    {
        private const byte k_NumberOfGuessesInRow = 4;
        private readonly List<List<Button>> r_ButtonsGuesses;
        private readonly List<Button> r_BtnsGeneratedSequence;
        private readonly List<Button> r_BtnsSetGuess;
        private readonly List<List<Button>> r_BtnsResult;
        private readonly List<TableLayoutPanel> r_TablePanelResults;
        private readonly TableLayoutPanel r_TableLayoutPanelMain;
        private readonly byte r_NumberOfGuesses;
        private readonly Dictionary<Color, char> m_colorToLetter = new Dictionary<Color, char>
        {
            [Color.Purple] = 'a',
            [Color.Red] = 'b',
            [Color.LimeGreen] = 'c',
            [Color.Cyan] = 'd',
            [Color.Blue] = 'e',
            [Color.Yellow] = 'f',
            [Color.Black] = 'g',
            [Color.White] = 'h'
        };

        private Color m_ColorPicked;
        private GameLogic r_GameLogic;
        bool IsGameEnded = false;
        public int NumberOfGuesses
        {
            get { return r_NumberOfGuesses; }
        }
        public List<List<Button>> ButtonsGuesses
        {
            get { return r_ButtonsGuesses; }
        }
        public Color ColorPicked
        {
            get { return m_ColorPicked; }
            set { m_ColorPicked = value; }
        }
        public FormGameBoard(byte i_NumberOfGuesses)
        {
            r_NumberOfGuesses = i_NumberOfGuesses;
            r_GameLogic = new GameLogic();
            r_GameLogic.m_numOfattempts = i_NumberOfGuesses;
            r_TableLayoutPanelMain = new TableLayoutPanel();
            r_BtnsGeneratedSequence = new List<Button>(k_NumberOfGuessesInRow);
            r_ButtonsGuesses = new List<List<Button>>(k_NumberOfGuessesInRow * r_NumberOfGuesses);
            r_BtnsSetGuess = new List<Button>(r_NumberOfGuesses);
            r_BtnsResult = new List<List<Button>>(r_NumberOfGuesses * k_NumberOfGuessesInRow);
            r_TablePanelResults = new List<TableLayoutPanel>(r_NumberOfGuesses);

            InitializeComponent();
            initializeGuessButtons();
            generateSequenceButtons();
            initializeSetGuessButtons();
            initializeResultButtons();
            initializeGuessesTable();
        }
        private void buttonSetGuess_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;

            if (senderButton != null)
            {
                int rowIndex = senderButton.TabIndex;
                List<Color> guessResultColor = AnalyzeGuessResult(r_ButtonsGuesses[rowIndex]);

                if (IsGameEnded)
                {
                    revealCorrectSequence(rowIndex);
                }

                for (int i = 0; i < k_NumberOfGuessesInRow; i++)
                {
                    r_BtnsResult[rowIndex][i].BackColor = guessResultColor[i];
                    r_ButtonsGuesses[rowIndex][i].Enabled = false;

                    if (rowIndex != r_NumberOfGuesses - 1)
                    {
                        if (!IsGameEnded)
                        {
                            r_ButtonsGuesses[rowIndex + 1][i].Enabled = true;
                        }
                    }

                }

                senderButton.Enabled = false;
            }
        }
        public List<Color> AnalyzeGuessResult(List<Button> i_GuessToAnalyze)
        {
            List<Color> analyseResult = new List<Color>(k_NumberOfGuessesInRow);
            int correctPositionAndColorCounter = 0;
            int correctColorCounter = 0;
            StringBuilder charsOfColors = new StringBuilder();

            for (int i = 0; i < k_NumberOfGuessesInRow; i++)
            {
                if (ColorToChar(i_GuessToAnalyze[i].BackColor) == r_GameLogic.randomString[i])
                {
                    correctPositionAndColorCounter++;
                }
                else
                {
                    if (r_GameLogic.randomString.Contains(ColorToChar(i_GuessToAnalyze[i].BackColor)))
                    {
                        correctColorCounter++;
                    }
                }
                charsOfColors.Append(ColorToChar(i_GuessToAnalyze[i].BackColor));
            }

            if (r_GameLogic.IsGameWon(charsOfColors.ToString()) || r_GameLogic.IsGameLost()) // added tostring
            {
                IsGameEnded = true;
            }

            for (int i = 0; i < k_NumberOfGuessesInRow; i++)
            {
                if (correctPositionAndColorCounter > 0)
                {
                    analyseResult.Add(Color.Black);
                    correctPositionAndColorCounter--;
                }
                else
                {
                    if (correctColorCounter > 0)
                    {
                        analyseResult.Add(Color.Yellow);
                        correctColorCounter--;
                    }
                    else
                    {
                        analyseResult.Add(Control.DefaultBackColor);
                    }
                }
            }

            return analyseResult;
        }
        public char ColorToChar(Color color)
        {
            Color guessColor = color;
            char colorLetter;

            m_colorToLetter.TryGetValue(guessColor, out colorLetter);

            return colorLetter;
        }
        public Color CharToColor(char letter)
        {
            foreach (KeyValuePair<Color, char> kvp in m_colorToLetter)
            {
                if (kvp.Value == letter)
                {
                    return kvp.Key;
                }
            }

            return Color.White;
        }
        private void revealCorrectSequence(int i_RowIndex)
        {
            for (int i = 0; i < k_NumberOfGuessesInRow; i++)
            {
                r_BtnsGeneratedSequence[i].BackColor = CharToColor(r_GameLogic.randomString[i]);
            }

            if (i_RowIndex == NumberOfGuesses - 1)
            {
                for (int i = 0; i < k_NumberOfGuessesInRow; i++)
                {
                    r_ButtonsGuesses[i_RowIndex][i].Enabled = false;
                }
            }
        }
        private void buttonGuess_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                int rowIndex = button.TabIndex;
                FormColorSelector formSelectColor = new FormColorSelector(this, new List<Color>(m_colorToLetter.Keys), rowIndex);
                bool isAllButtonsPainted = true;
                formSelectColor.ShowDialog();
                button.BackColor = ColorPicked;

                for (int i = 0; i < k_NumberOfGuessesInRow; i++)
                {
                    if (r_ButtonsGuesses[rowIndex][i].BackColor == DefaultBackColor)
                    {
                        isAllButtonsPainted = false;
                        break;
                    }
                }

                if (isAllButtonsPainted)
                {
                    r_BtnsSetGuess[rowIndex].Enabled = true;
                }
            }
        }
        private void addGeneratedSequenceButtons()
        {
            for (int i = 0; i < k_NumberOfGuessesInRow; i++)
            {
                r_TableLayoutPanelMain.Controls.Add(r_BtnsGeneratedSequence[i]);
            }
        }
        private void addLabels()
        {
            r_TableLayoutPanelMain.Controls.Add(new Label());
            r_TableLayoutPanelMain.Controls.Add(new Label());
        }
        private void addGuessButtons()
        {
            for (int i = 0; i < r_ButtonsGuesses.Count; i++)
            {
                for (int j = 0; j < k_NumberOfGuessesInRow; j++)
                {
                    r_TablePanelResults[i].Controls.Add(r_BtnsResult[i][j]);
                    r_TableLayoutPanelMain.Controls.Add(r_ButtonsGuesses[i][j]);
                }

                r_TableLayoutPanelMain.Controls.Add(r_BtnsSetGuess[i]);
                r_TableLayoutPanelMain.Controls.Add(r_TablePanelResults[i]);
            }
        }
        private void initializeGuessButtons()
        {
            for (int i = 0; i < r_NumberOfGuesses; i++)
            {
                r_ButtonsGuesses.Add(new List<Button>(k_NumberOfGuessesInRow));
                for (int j = 0; j < k_NumberOfGuessesInRow; j++)
                {
                    r_ButtonsGuesses[i].Add(new Button());
                    r_ButtonsGuesses[i][j].Size = new Size(60, 60);
                    this.Controls.Add(r_ButtonsGuesses[i][j]);
                    r_ButtonsGuesses[i][j].TabIndex = i;
                    r_ButtonsGuesses[i][j].Enabled = i == 0;
                    r_ButtonsGuesses[i][j].Click += buttonGuess_Click;
                }
            }
        }
        private void generateSequenceButtons()
        {
            for (int i = 0; i < k_NumberOfGuessesInRow; i++)
            {
                r_BtnsGeneratedSequence.Add(new Button());
                r_BtnsGeneratedSequence[i].Size = new Size(60, 60);
                r_BtnsGeneratedSequence[i].BackColor = Color.Black;
                r_BtnsGeneratedSequence[i].Enabled = false;
                this.Controls.Add(r_BtnsGeneratedSequence[i]);
            }
        }
        private void initializeSetGuessButtons()
        {
            string textForSetGuessButtons = "-->>";

            for (int i = 0; i < r_NumberOfGuesses; i++)
            {
                r_BtnsSetGuess.Add(new Button());
                r_BtnsSetGuess[i].Size = new Size(60, 60);
                r_BtnsSetGuess[i].TextAlign = ContentAlignment.MiddleCenter;
                r_BtnsSetGuess[i].Margin = new Padding(0, 3, 0, 18);
                r_BtnsSetGuess[i].Text = textForSetGuessButtons;
                r_BtnsSetGuess[i].TabIndex = i;
                r_BtnsSetGuess[i].Enabled = false;
                this.Controls.Add(r_BtnsSetGuess[i]);
                r_BtnsSetGuess[i].Click += buttonSetGuess_Click;
            }
        }
        private void initializeResultButtons()
        {
            for (int i = 0; i < r_NumberOfGuesses; i++)
            {
                r_BtnsResult.Add(new List<Button>(k_NumberOfGuessesInRow));
                for (int j = 0; j < k_NumberOfGuessesInRow; j++)
                {
                    r_BtnsResult[i].Add(new Button());
                    r_BtnsResult[i][j].Enabled = false;
                    r_BtnsResult[i][j].Size = new Size(25, 25);
                }
            }
        }
        private void initializeGuessesTable()
        {
            int numberOfRows = r_NumberOfGuesses + 1;
            int numberOfCols = k_NumberOfGuessesInRow + 2;
            initializeMainTableLayout(numberOfCols, numberOfRows);
            initializeResultTablePanels();
            initializeColumnStyles(numberOfCols);
            initializeRowStyles(numberOfRows);
            addGeneratedSequenceButtons();
            addLabels();
            addGuessButtons();
            this.Controls.Add(r_TableLayoutPanelMain);
        }
        private void initializeMainTableLayout(int numberOfCols, int numberOfRows)
        {
            r_TableLayoutPanelMain.ColumnCount = numberOfCols;
            r_TableLayoutPanelMain.RowCount = numberOfRows;
            r_TableLayoutPanelMain.Dock = DockStyle.Fill;
        }
        private void initializeResultTablePanels()
        {
            for (int i = 0; i < r_NumberOfGuesses; i++)
            {
                var resultTablePanel = new TableLayoutPanel();
                resultTablePanel.RowCount = 2;
                resultTablePanel.ColumnCount = 2;
                resultTablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100 / 2.0f));
                resultTablePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100 / 2.0f));
                resultTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 100 / 2.0f));
                resultTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 100 / 2.0f));
                r_TablePanelResults.Add(resultTablePanel);
                this.Controls.Add(resultTablePanel);
            }
        }
        private void initializeColumnStyles(int numberOfCols)
        {
            for (int i = 0; i < numberOfCols; i++)
            {
                r_TableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / (float)numberOfCols));
            }
        }
        private void initializeRowStyles(int i_NumberOfRows)
        {
            for (int i = 0; i < i_NumberOfRows; i++)
            {
                r_TableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / (float)i_NumberOfRows));
            }
        }
    }
}