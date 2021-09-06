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
  public partial class Form1 : Form
  {
    private List<TreeNode> CurrentNodeMatches = new List<TreeNode>();
    private int LastNodeIndex = 0;
    private string LastSearchText;
    TreeNode selNode;
    string registryPath = "";
    string parent = "";
    string valName = "";

    RegistryKey key = Registry.CurrentUser;
    helping_functions helping = new helping_functions();
    public Form1()
    {
      InitializeComponent();  
    }
    private void Form1_Load(object sender, EventArgs e)
    {
      refreshNodes();
    }   
    TreeNode findNode(TreeNode root, string name)
    {
      if (root == null)
        return null;

      if (root.Name == name)
        return root;
      return findNode(root.FirstNode, name) ?? findNode(root.NextNode, name);
    }

    public void refreshNodes()
    {
      treeView1.Nodes.Clear();
      
      RegistryKey key1 = Registry.ClassesRoot;
      TreeNode node1 = new TreeNode(key1.Name) { Tag = key1 };
      buildNode(node1);
      treeView1.Nodes.Add(node1);
      
      key1 = Registry.CurrentUser;
      TreeNode node2 = new TreeNode(key1.Name) { Tag = key1 };
      buildNode(node2);
      treeView1.Nodes.Add(node2);
      
      key1 = Registry.LocalMachine;
      TreeNode node3 = new TreeNode(key1.Name) { Tag = key1 };
      buildNode(node3);
      TreeNode n1 = new TreeNode("SOFTWARE") { Tag = key1.OpenSubKey("SOFTWARE") };
      buildNode(n1);
      TreeNode n2 = new TreeNode("SYSTEM") { Tag = key1.OpenSubKey("SYSTEM") };
      buildNode(n2);
      treeView1.Nodes.Add(node3);
      treeView1.Nodes[2].Nodes.Add(n1);
      treeView1.Nodes[2].Nodes.Add(n2);

      key1 = Registry.Users;
      TreeNode node4 = new TreeNode(key1.Name) { Tag = key1 };
      buildNode(node4);
      treeView1.Nodes.Add(node4);

      key1 = Registry.CurrentConfig;
      TreeNode node5 = new TreeNode(key1.Name) { Tag = key1 };
      buildNode(node5);
      treeView1.Nodes.Add(node5);
      
    }
    private TreeNode buildNode(TreeNode node)
    {
      RegistryKey key = (RegistryKey)node.Tag;
      try
      {
        if (key == null) return node;
        foreach(var name in key.GetSubKeyNames())
        {
          TreeNode child = new TreeNode(name) { Tag = key.OpenSubKey(name) };
          node.Nodes.Add(child);
          buildNode(child);
        }
      }
      catch
      {
        return node;
      }
      return node;
    }
    private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {

    }
    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
      //сделать видимыми элементы поиска
      if(e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
      {
        textBox1.Visible = true;
        button1.Visible = true;
      }
      if(e.Modifiers == Keys.F5)
      {
        refreshNodes();
      }
    }
    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
      registryPath = "";
      dataGridView1.Rows.Clear();
      List<string> parentNames = new List<string>();
      TreeNode node = treeView1.SelectedNode;

      selNode = node;


      while (node.Parent!= null)
      {
        parentNames.Add(node.Text);
        node = node.Parent;
      }
      for (int i = parentNames.Count - 1; i >= 0; i--)
      {
        if (i == parentNames.Count - 1)
        {
          registryPath += parentNames[i];
        }
        else
        {
          registryPath += $"\\{parentNames[i]}";
        }
      }
      node = treeView1.SelectedNode;
      if(node.Parent != null)
      {
        while(node.Parent != null)
        {
          node = node.Parent;
          parent = node.Text;
        }
      }
      switch(parent)
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
      string[] values = key.OpenSubKey(registryPath).GetValueNames();
      Filter filter = new Filter();
      for(int i = 0; i<= values.Length - 1; i++)
      {
        string type = key.OpenSubKey(registryPath).GetValueKind(values[i]).ToString();
        string value = key.OpenSubKey(registryPath).GetValue(values[i]).ToString();
        string[] buf = new string[] { values[i], helping.typeName(type), value };

        if(type == "String" && filter.reg_sz == true) dataGridView1.Rows.Add(buf);
        if (type == "DWord" && filter.reg_dword == true) dataGridView1.Rows.Add(buf);
        if(type == "QWord" && filter.reg_qword == true) dataGridView1.Rows.Add(buf);
        if(type == "Binary" && filter.reg_binary == true) dataGridView1.Rows.Add(buf);
        if(type == "MultiString" && filter.reg_multi_sz == true) dataGridView1.Rows.Add(buf);
        if(type == "ExpandString" && filter.reg_expand_sz == true) dataGridView1.Rows.Add(buf);
      }
    }
    private void SearchNodes(string SearchText, TreeNode StartNode)
    {
      while (StartNode != null)
      {
        if (StartNode.Text.ToLower() == SearchText.ToLower())
        {
          CurrentNodeMatches.Add(StartNode);
        };
        if (StartNode.Nodes.Count != 0)
        {
          SearchNodes(SearchText, StartNode.Nodes[0]);
        };
        StartNode = StartNode.NextNode;
      };
    }
    private void button1_Click(object sender, EventArgs e)
    {
      treeView1.CollapseAll();
      string searchText = textBox1.Text;

      if (String.IsNullOrEmpty(searchText))
      {
        MessageBox.Show("Name is empty");
      };

      if (LastSearchText != searchText)
      {
        //если введенный текст отличается от предыдущего текста поиска
        CurrentNodeMatches.Clear();
        LastSearchText = searchText;
        LastNodeIndex = 0;
        TreeNode[] nodes = new TreeNode[]
        {
          treeView1.Nodes[0], treeView1.Nodes[1],
          treeView1.Nodes[2], treeView1.Nodes[3],
          treeView1.Nodes[4]
        };
        foreach(var node in nodes)
        {
          SearchNodes(searchText, node);
        }
        if(CurrentNodeMatches.Count != 0 && CurrentNodeMatches.Count > 1)
        {
          button2.Visible = true;
        }
      }

      if(CurrentNodeMatches.Count > 0)
      {
        treeView1.SelectedNode = CurrentNodeMatches[LastNodeIndex];
        treeView1.Select();
      }
      else
      {
        MessageBox.Show("No results");
      }
    }
    private void button2_Click(object sender, EventArgs e)
    {
      if(CurrentNodeMatches.Count > 1)
      {
        TreeNode node = CurrentNodeMatches[LastNodeIndex];
        treeView1.SelectedNode = node;
        treeView1.Select();
        LastNodeIndex++;
      }
      if(LastNodeIndex == CurrentNodeMatches.Count - 1)
      {
        TreeNode node = CurrentNodeMatches[LastNodeIndex];
        treeView1.SelectedNode = node;
        treeView1.Select();
        LastNodeIndex = 0;
      }
    }
    private void button3_Click(object sender, EventArgs e)
    {
      Form2 form2 = new Form2();
      form2.Show();
    }
    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      Form3 form3 = new Form3(registryPath, parent,ref treeView1, ref selNode );
      form3.Show();
    }
    private void button4_Click_1(object sender, EventArgs e)
    {
      treeView1.Nodes.Clear();
      refreshNodes();
    }
    private void dataGridView1_DoubleClick(object sender, EventArgs e)
    {
    }
    private void button5_Click(object sender, EventArgs e)
    {
      Form4 form = new Form4(registryPath, key, ref dataGridView1);
      form.Show();
    }
    private void button6_Click(object sender, EventArgs e)
    {
      string[] valuesNames = key.OpenSubKey(registryPath).GetValueNames();
      bool valueExists = false;
      foreach(string name in valuesNames)
      {
        if(name == valName)
        {
          valueExists = true;
        }
      }
      if(valueExists == true)
      {
        try
        {
          key.OpenSubKey(registryPath, true).DeleteValue(valName);
          MessageBox.Show("Ok");
        }
        catch
        {
          MessageBox.Show("Not enough rights");
        }
      }
      else
      {
        MessageBox.Show("Not exists");
      }
    }
    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if(dataGridView1.CurrentCell.Value != null)
      {
        valName = dataGridView1.CurrentCell.Value.ToString();
      }
    }
    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void button7_Click(object sender, EventArgs e)
    {
      treeView1.CollapseAll();
    }

    private void button8_Click(object sender, EventArgs e)
    {
      Form5 form = new Form5();
      form.Show();
    }
  }
}
