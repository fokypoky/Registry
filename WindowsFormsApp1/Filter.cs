using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
  class Filter
  {
    public bool reg_sz = true;
    public bool reg_expand_sz = true;
    public bool reg_multi_sz = true;
    public bool reg_binary = true;
    public bool reg_dword = true;
    public bool reg_qword = true;

    List<bool> values = new List<bool>();
    string path = "filter_config.txt";
    public Filter()
    {
      setParams();
    }
    public void setParams(List<bool> _values)
    {
      using(var sw = new StreamWriter(path, false))
      {
        foreach(bool v in _values)
        {
          string str = v.ToString();
          sw.WriteLine(str);
        }
      }
    }
    public void setParams()
    {
      if(File.Exists(path) == false)
      {
        using(var sw = new StreamWriter(path, true))
        {
          for(int i = 0; i <= 5; i++)
          {
            sw.WriteLine(true);
          }
        }
      }
      // чтение из файла
      using (var sr = new StreamReader(path))
      {
        while (!sr.EndOfStream)
        {
          string name = sr.ReadLine().ToLower();
          if (name == "true") values.Add(true);
          else values.Add(false);
        }
      }
      reg_sz = values[0];
      reg_expand_sz = values[1];
      reg_multi_sz = values[2];
      reg_binary = values[3];
      reg_dword = values[4];
      reg_qword = values[5];
    }
    public List<bool> getParams()
    {
      return values;
    }
  }
  class helping_functions
  {
    public string typeName(string name)
    {
      if (name == "String") return "REG_SZ";
      if (name == "DWord") return "REG_DWORD";
      if (name == "Binary") return "REG_BINARY";
      if (name == "MultiString") return "REG_MULTI_SZ";
      if (name == "QWord") return "REG_QWORD";
      if (name == "ExpandString") return "REG_EXPAND_SZ";
      return name;
    }
    public void dataGridSet(string[] vals, RegistryKey key, DataGridView data)
    {
      foreach (string name in vals)
      {
        string[] values = new string[] { name, typeName(key.GetValueKind(name).ToString()), key.GetValue(name).ToString() };
        data.Rows.Add(values);
      }
    }
  }
}
