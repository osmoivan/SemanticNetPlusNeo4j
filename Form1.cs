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
    public partial class Form1 : Form
    {
		
		public Form1()
        {
            InitializeComponent();
			textBox1.Text = "1111qwertyuiop[]asdfghjklzxcvbnm";
		}

        private void button1_Click(object sender, EventArgs e)
        {
			int[] x = new int[2];
			x=Ivan.SelectedTextIntoIndexForSemanticFragmentTable(textBox1);
			Ivan.AddIndexIntoSemanticFragmentTable("C://Users/ivan_/Desktop/time.txt");
		}

        private void button4_Click(object sender, EventArgs e)
        {
			
        }

        private void button2_Click(object sender, EventArgs e)
        {
			
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
		
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			
		}
	}
}
public class Ivan
{
	public static int[] outdata = new int[2];
	public static int[] SelectedTextIntoIndexForSemanticFragmentTable(System.Windows.Forms.TextBox textBox)
	{
		if (textBox.SelectionLength > 0)
		{
			outdata[0] = textBox.SelectionStart;
			outdata[1] = textBox.SelectionStart + textBox.SelectionLength - 1;
			return outdata;
		}
		else
		{
			MessageBox.Show("Вы не выделили смысловой фрагмент");
			return outdata;
		}
	}
	public static void AddIndexIntoSemanticFragmentTable(string File)
	{
		int lengthTable = System.IO.File.ReadAllLines(File).Length + 1;
		int[,] index = new int[lengthTable, 2];
		int ind = 0;
		using (StreamReader sr = new StreamReader(File))   //System.IO.File.Create(File))
		{
			while (sr.Peek() >= 0)
			{
				string s = sr.ReadLine();
				string[] s1 = s.Split('\t');
				index[ind, 0] = Convert.ToInt32(s1[1]);
				index[ind, 1] = Convert.ToInt32(s1[2]);
				ind = ind + 1;
			}
		}
		index = Ivan.SortMatrix(index);
		index[0, 0] = outdata[0];
		index[0, 1] = outdata[1];
		index=Ivan.CheckCrossingElements(index);
		using (StreamWriter sw = new StreamWriter(File))
		{
			for (int i = 0; i < lengthTable; i++)
			{
				sw.WriteLine("СФ" + i.ToString() + "\t" + index[i, 0].ToString() + "\t" + index[i, 1].ToString());
			}
		}

	}
	private static int[,] SortMatrix(int[,] index)
	{
		List<int> FirstElement = new List<int>();
		List<int> SecondElement = new List<int>();
		int[,] timeindex = new int[index.GetLength(0), index.GetLength(1)];
		for (int i = 0; i < index.GetLength(0); i++)
		{
			FirstElement.Add(index[i, 0]);
			SecondElement.Add(index[i, 1]);
		}
		for (int i = 0; i < index.GetLength(0); i++)
		{
			int c = FirstElement.IndexOf(FirstElement.Min());
			timeindex[i, 0] = FirstElement[c];
			timeindex[i, 1] = SecondElement[c];
			FirstElement.RemoveAt(c);
			SecondElement.RemoveAt(c);
		}
		return timeindex;
	}
	private static int[,] CheckCrossingElements(int[,] index)
	{
		int i = 2;
		System.Collections.Generic.IEnumerable<int> newRange = Enumerable.Range(index[0, 0], index[0, 1]- index[0, 0]+1);
		System.Collections.Generic.IEnumerable<int> oldRange = Enumerable.Range(index[i, 0], index[i, 1]- index[i, 0]+1);
		IEnumerable<int> both = newRange.Intersect(oldRange);
		foreach (var o in both)
		{
			oldRange.Where(old => old != o);
		}
		return index;
	}
}