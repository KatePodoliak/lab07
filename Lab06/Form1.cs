using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace Lab06
{
    public partial class Form1 : Form
    {
        string str = "", sql;
        public Form1()
        {
            InitializeComponent();
            cmbCommand.Items.Add("Insert");
            cmbCommand.Items.Add("Delete");
            cmbCommand.Items.Add("Update");
            cmbCommand.SelectedIndex = 0;
            cmbCommand1.Items.Add("Select");
            cmbCommand1.Items.Add("Search");
            cmbCommand1.Items.Add("Sort");
            cmbCommand1.SelectedIndex = 0;
            cmbSort.Items.Add("ASC");
            cmbSort.Items.Add("DESC");
            cmbSort.SelectedIndex = 0;
            label10.Text = "Fields:";
            label11.Text = "Values:";
            textBoxFields.Visible = true;
            textBoxValuesCmd.Visible = true;
            textBoxSet.Visible = false;
            textBoxWhere.Visible = false;
            btnDoCmd.Enabled = false;
            btnDoCmd1.Enabled = false;
            btnDoQuery.Enabled = false;
        }

        //изменение выбора операции
        private void cmbCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCommand.Text)
            {
                case "Insert":
                    label10.Text = "Fields:";
                    label11.Text = "Values:";
                    textBoxFields.Visible = true;
                    textBoxValuesCmd.Visible = true;
                    textBoxSet.Visible = false;
                    textBoxWhere.Visible = false;
                    break;
                case "Delete":
                    label10.Text = "Set:";
                    label11.Text = "Where:";
                    textBoxFields.Visible = false;
                    textBoxValuesCmd.Visible = false;
                    textBoxSet.Visible = true;
                    textBoxWhere.Visible = true;
                    break;
                case "Update":
                    label10.Text = "Set:";
                    label11.Text = "Where:";
                    textBoxFields.Visible = false;
                    textBoxValuesCmd.Visible = false;
                    textBoxSet.Visible = true;
                    textBoxWhere.Visible = true;
                    break;
            }
        }
        
        //выполнение запроса
        private void btnDoQuery_Click(object sender, EventArgs e)
        {
            try {
                sql = textBoxQuery.Text;
                DataSet data = DBServerUtils.getData(sql);
                dgwData.DataSource = data.Tables[0];
            }
            catch {
                MessageBox.Show("Input valid parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //выбор команды: вывод таблицы, поиск записи и сортировка в разных направлениях
        private void btnDoCmd1_Click(object sender, EventArgs e)
        {
            str = "Input valid parameters!";
            DataSet ds;
            try
            {
                switch (cmbCommand1.Text)
                {
                    case "Select":
                        if (textBoxTable.Text.Length == 0)
                            str = "Input name of table!";
                        sql = "SELECT" + " * " + "FROM" + " " + textBoxTable.Text;
                        ds = DBServerUtils.getData(sql);
                        dgwData.DataSource = ds.Tables[0];
                        break;
                    case "Search":
                        if (textBoxTable.Text.Length == 0 || textBoxAttribute.Text.Length == 0 || textBoxValue.Text.Length == 0)
                            str = "Input parameters!";
                        sql = "SELECT * FROM " + textBoxTable.Text + " WHERE " + textBoxAttribute.Text + " ='" + textBoxValue.Text + "'";
                        ds = DBServerUtils.getData(sql);
                        dgwData.DataSource = ds.Tables[0];
                        break;
                    case "Sort":
                        if (textBoxTable.Text.Length == 0 || textBoxAttribute.Text.Length == 0)
                            str = "Input parameters!";
                        sql = "SELECT * FROM " + textBoxTable.Text + " ORDER BY " + textBoxAttribute.Text + " " + cmbSort.Text;
                        ds = DBServerUtils.getData(sql);
                        dgwData.DataSource = ds.Tables[0];
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //соединить
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (DBServerUtils.connectingDB(0))
            {
                labelInfo.Text = "Connection...\nConnection successful!\nServer: " + DBServerUtils.source + "\nDatabase: " + DBServerUtils.catalog;
                btnDoCmd.Enabled = true;
                btnDoCmd1.Enabled = true;
                btnDoQuery.Enabled = true;
            }
            else
            {
                labelInfo.Text = "Connection...\nConnection failed!\n";
                btnDoCmd.Enabled = false;
                btnDoCmd1.Enabled = false;
                btnDoQuery.Enabled = false;
            }
        }
        //отключить
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (DBServerUtils.connectingDB(1))
                labelInfo.Text = "Connection failed!\n";
            btnDoCmd.Enabled = false;
            btnDoCmd1.Enabled = false;
            btnDoQuery.Enabled = false;
        }

        //выбор команды: добавление, изменение и удаление записи
        private void btnDoCmd_Click(object sender, EventArgs e)
        {
            str = "Input valid parameters!";
            try {    
                switch(cmbCommand.Text)
                {
                    case "Insert":
                        sql = "INSERT " + textBoxTableCmd.Text + "(" + textBoxFields.Text + ")" + "VALUES(" + textBoxValuesCmd.Text + ")";
                        break;
                    case "Delete":
                        sql = "DELETE FROM " + textBoxTableCmd.Text + " WHERE " + textBoxWhere.Text;
                        break;
                    case "Update":
                        sql = "UPDATE " + textBoxTableCmd.Text + " SET " + textBoxSet.Text + " WHERE " + textBoxWhere.Text;
                        break;
                    default:
                        break;
                }
                SqlConnection conn = DBServerUtils.getDBсonnection();
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                sql = "SELECT" + " * " + "FROM" + " " + textBoxTableCmd.Text;
                DataSet data = DBServerUtils.getData(sql);
                dgwData.DataSource = data.Tables[0];
            }
            catch {
                MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
