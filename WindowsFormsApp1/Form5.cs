using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
  public partial class Form5 : Form
  {
    Form1 form = new Form1();
    helping_functions helping = new helping_functions();
    public Form5()
    {
      InitializeComponent();
    }
    private void button1_Click(object sender, EventArgs e)
    {
      dataGridView1.Rows.Clear();
      RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\Disk\\Enum");
      string[] vals = key.GetValueNames();
      helping.dataGridSet(vals, key, dataGridView1);
    }
    private void button2_Click(object sender, EventArgs e)
    {
      dataGridView1.Rows.Clear();
      RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList");
      string[] subkeys = key.GetSubKeyNames();
      foreach(string name in subkeys)
      {
        key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList").OpenSubKey(name);
        string[] values = key.GetValueNames();
        helping.dataGridSet(values, key, dataGridView1);
      }
    }
    private void button3_Click(object sender, EventArgs e)
    {
      dataGridView1.Rows.Clear();
      RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName");
      string[] val = new string[] { "ComputerName", key.GetValueKind("ComputerName").ToString(), key.GetValue("ComputerName").ToString() };
      dataGridView1.Rows.Add(val);
    }

    private void Form5_Load(object sender, EventArgs e)
    {

    }

    private void button4_Click(object sender, EventArgs e)
    {
      dataGridView1.Rows.Clear();
      RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
      string[] val = key.GetValueNames();
      helping.dataGridSet(val, key, dataGridView1);
    }
  }
}
