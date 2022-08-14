using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyscraper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int[] arrint = { 1, 2, 3, 4, 5, 6 };
            var list= Permutation.GetPermutations <int>(arrint.ToList (), 3);
            StringBuilder strB = new StringBuilder();
            
            list.ToList ().ForEach(x =>
           {
               x.ToList ().ForEach(y => strB.Append(y));
               strB.Append(Environment.NewLine);
           });
            this.textBox1.Text = strB.ToString();
        }
    }
}
