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
  public partial class Form3 : Form
  {
    string path;
    string parent;
    TreeView tree;
    TreeNode node;
    public Form3(string _path, string _parent, ref TreeView _tree, ref TreeNode _node)
    {
      InitializeComponent();
      path = _path;
      parent = _parent;
      tree = _tree;
      node = _node;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string name = textBox1.Text;
      RegistryKey key = Registry.CurrentUser;

      switch (parent)
      {
        case "HKEY_CLASSES_ROOT":
          key = Registry.ClassesRoot;
          break;
        case "HKEY_CURRENT_USER":
          key = Registry.CurrentUser;
          break;
        case "HKEY_USERS":
          key = Registry.Users;
          break;
        case "HKEY_CURRENT_CONFIG":
          key = Registry.CurrentConfig;
          break;
        case "HKEY_LOCAL_MACHINE":
          key = Registry.LocalMachine;
          break;
        default:
          break;
      }
      if(path.Equals(string.Empty) || path == null)
      {
        MessageBox.Show("Name is null");
      }
      else
      {
        try
        {
          key.CreateSubKey(path + "\\" + name);
          TreeNode nod = new TreeNode(textBox1.Text);
          node.Nodes.Add(nod);
        }
        catch
        {
          MessageBox.Show("Not enough rights");
          return;
        }
      }
      Close();
      MessageBox.Show("Good", "Registry key delete");
    }

    private void Form3_Load(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {
      label1.Visible = true;
      textBox1.Visible = true;
      button1.Visible = true;
      button3.Visible = false;
      button2.Visible = false;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      RegistryKey key = Registry.CurrentUser;

      switch (parent)
      {
        case "HKEY_CLASSES_ROOT":
          key = Registry.ClassesRoot;
          break;
        case "HKEY_CURRENT_USER":
          key = Registry.CurrentUser;
          break;
        case "HKEY_USERS":
          key = Registry.Users;
          break;
        case "HKEY_CURRENT_CONFIG":
          key = Registry.CurrentConfig;
          break;
        case "HKEY_LOCAL_MACHINE":
          key = Registry.LocalMachine;
          break;
        default:
          break;
      }
      key.DeleteSubKey(path);
      tree.Nodes.Remove(node);
      Close();
      MessageBox.Show("Good", "Registry key delete");
    }
  }
}
