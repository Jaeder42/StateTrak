using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsGoComp
{
    public partial class Form2 : Form
    {
        private TableLayoutPanel table;
        public Form2()
        {
            InitializeComponent();
            

            table = new TableLayoutPanel();
            table.ColumnCount = 1;
            table.RowCount = 1;
          
           
            container.Controls.Add(table);
           // table.Size = table.Parent.Size;
            table.AutoSize = true;
            //table.AutoScroll = true;
            /* Padding margin = table.Margin;
             margin.Right = 20;
             margin.Left = 20;
             table.Margin = margin;*/
            for (int i = 0; i < 5; i++)
            {
                GameItem[] items = new GameItem[3];
                items[0] = new GameItem("10", "10", "3", "16", "10", "CT", "de_dust2");
                items[1] = new GameItem("10", "10", "3", "10", "16", "CT", "de_dust2");
                items[2] = new GameItem("10", "10", "3", "15", "15", "CT", "de_dust2");

                createList(items);
            }



        }
        private void createList(GameItem[] items)
        {
            foreach (GameItem item in items)
            {
                String[] strings = item.getString();



              
                    TableLayoutPanel gameinfo = new TableLayoutPanel();
                    gameinfo.ColumnCount = 7;
                    gameinfo.RowCount = 2;

                foreach (String str in strings)
                {
                    Label label = new Label();
                    label.Text = str;
                   
                    label.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    

                    gameinfo.Controls.Add(label);

                }

                gameinfo.BackColor = item.getColor();

                    gameinfo.AutoSize = true;
                    

                    






                    table.Controls.Add(gameinfo);

                }

            
        }
    }



 
    public class GameItem{
        String map;
        String kills;
        String deaths;
        String ctscore;
        String tscore;
        String mvp;
        String team;
        String result;
        public GameItem( String kills, String deaths, String mvp, String ctscore, String tscore, String team, String map)
        {
            this.map = map;
            this.kills = kills;
            this.deaths = deaths;
            this.ctscore = ctscore;
            this.tscore = tscore;
            this.mvp = mvp;
            this.team = team;
            calcRes();
        }

        public void calcRes()
        {
            int ct = int.Parse(this.ctscore);
            int t = int.Parse(this.tscore);

            if (ct > t)
            {
                if (team.Equals("CT"))
                {
                    result = "Win";
                }
                else
                {
                    result = "Loss";
                }
            }
            else if (t > ct)
            {
                if (team.Equals("T"))
                {
                    result = "Win!";
                }
                else
                {
                    result = "Loss";
                }
            }
            else
            {
                result = "Tie";
            }
        }
        public Color getColor()
        {
            int ct = int.Parse(this.ctscore);
            int t = int.Parse(this.tscore);

            if(ct > t)
            {
                if (team.Equals("CT"))
                {
                    return Color.FromArgb(56, 142, 60);
                }
                else
                {
                    return Color.Red;
                }
            }
            else if(t > ct)
            {
                if (team.Equals("T"))
                {
                    return Color.FromArgb(56, 142, 60);
                }
                else
                {
                    return Color.Red;
                }
            }
            else
            {
                return Color.SaddleBrown;
            }
            
        }

        public String[] getString()
        {
            String[] result = new String[14];
            result[0] = "Result";
            result[1] = "Map";
            result[2] = "CT";
            result[3] = "T";
            result[4] = "Kills";
            result[5] = "Deaths";
            result[6] = "MVPs";
            result[7] = this.result;
            result[8] = this.map;
            result[9] = this.ctscore;
            result[10] = this.tscore;
            result[11] = this.kills;
            result[12] = this.deaths;
            result[13] = this.mvp;

            return result;



        }
    }

}
