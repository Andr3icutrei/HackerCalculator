using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HackerCalculator
{
    public class ButtonsViewModel
    {
        public List<List<String>> ButtonsContent { get; set; }
        public List<List<String>> TopRowContent { get; set; }
        private const int ROWS = 6;
        private const int COLUMNS = 4;
        public ButtonsViewModel()
        {
            ButtonsContent= new List<List<String>>();

            for (int i = 0; i < ROWS ; i++)
            {
                List<String> row = new List<String>();
                for (int j = 0; j < COLUMNS; j++)
                {
                    row.Add("");
                }
                ButtonsContent.Add(row);
            }

            ButtonsContent[0][0] ="%";
            ButtonsContent[0][1] ="CE";
            ButtonsContent[0][2] ="C";
            ButtonsContent[0][3] = "DEL";
            ButtonsContent[1][0] = "1/x";
            ButtonsContent[1][1] = "x^2";
            ButtonsContent[1][2] = "sqrt(x)";
            ButtonsContent[1][3] = "÷";

            /// digits
            for (int i = 2; i <= 4; ++i)
            {
                for(int j = 0;j <= 2; ++j)
                {
                    ButtonsContent[i][j] = (3 * (5 - i)-2+j).ToString();
                }
            }
         
            ButtonsContent[2][3] = "X";
            ButtonsContent[3][3] = "-";
            ButtonsContent[4][3] = "+";

            ButtonsContent[5][0] = "+/-";
            ButtonsContent[5][1] = "0";
            ButtonsContent[5][2] = ".";
            ButtonsContent[5][3] = "=";

            TopRowContent = new List<List<String>>();
            List<String> topRow = new List<String>();
            topRow.Add("MC");
            topRow.Add("MR");
            topRow.Add("M+");
            topRow.Add("M-");
            topRow.Add("MS");
            topRow.Add("M>");
            TopRowContent.Add(topRow);
        }
    }
}
