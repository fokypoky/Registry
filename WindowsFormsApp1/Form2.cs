using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
  public partial class Form2 : Form
  {
    private string path = "filter_config.txt";
    public Form2()
    {
      InitializeComponent();
    }
    private void Form2_Load(object sender, EventArgs e)
    {
      configLoad();
    }
    private void configLoad()
    {
      Filter filter = new Filter();
      List<bool> values = filter.getParams();
      for(int i = 0; i <= values.Count - 1; i++)
      {
        checkedListBox1.SetItemChecked(i, values[i]);
      }
    }
    private void button1_Click(object sender, EventArgs e)
    {
      List<bool> values = new List<bool>();
      for(int i = 0; i <= checkedListBox1.Items.Count - 1; i++)
      {
        values.Add(checkedListBox1.GetItemChecked(i));
      }
      Filter filter = new Filter();
      filter.setParams(values);
      Close();
    }
  }
}
