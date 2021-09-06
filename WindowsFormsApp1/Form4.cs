using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
  public partial class Form4 : Form
  {
    string path;
    RegistryKey key = Registry.CurrentUser;
    DataGridView data;

    public Form4(string _path, RegistryKey _key, ref DataGridView _data)
    {
      InitializeComponent();
      path = _path;
      key = _key;
      data = _data;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if((textBox1.Text != null || textBox1.Text != " " || textBox1.Text != "") && (textBox2.Text != null || textBox2.Text != " " || textBox2.Text != ""))
      {
        string valueName = textBox1.Text;

        switch (listBox1.SelectedIndex)
        {
          case 0:
            string sz = textBox2.Text;
            key.OpenSubKey(path, true).SetValue(valueName, sz);
            string[] values = new string[] { valueName, "REG_SZ", sz };
            data.Rows.Add(values);
            break;
          case 1:

            break;
          case 2:
            break;
          case 3:
            var b = Convert.ToByte(textBox2.Text);
            key.OpenSubKey(path, true).SetValue(valueName, b);
            string[] v = new string[] { valueName, "REG_BINARY", b.ToString() };
            data.Rows.Add(v);
            break;
          case 4:
            UInt32 dword = Convert.ToUInt32(textBox2.Text);
            key.OpenSubKey(path, true).SetValue(valueName, dword);
            string[] values1 = new string[] { valueName, "REG_DWORD", dword.ToString() };
            data.Rows.Add(values1);
            break;
          case 5:
            UInt64 qword = Convert.ToUInt64(textBox2.Text);
            key.OpenSubKey(path, true).SetValue(valueName, qword);
            string[] vals = new string[] { valueName, "REG_QWORD", qword.ToString() };
            data.Rows.Add(vals);
            break;
          default:
            break;
        }

        MessageBox.Show("Good");
        Close();
      }
    }

    private void Form4_Load(object sender, EventArgs e)
    {

    }
  }
}
