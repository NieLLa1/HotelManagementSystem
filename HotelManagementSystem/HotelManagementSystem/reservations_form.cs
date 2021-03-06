﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class reservations_form : Form
    {
        SqlDataAdapter adap;
        DataSet ds;
        SqlCommandBuilder cmdbl;

        public reservations_form()
        {
            InitializeComponent();
        }

        private void reservations_table_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con2 = new SqlConnection("Data Source=.;Initial Catalog=Hotel;Integrated Security=True");
                con2.Open();
                adap = new SqlDataAdapter("select id_reservation, id_client as 'id_client', id_employee as 'id_employee', id_room as 'id_room', start_date as 'start_date', end_date as 'end_date' from reservations_table ", con2);
                ds = new System.Data.DataSet();
                adap.Fill(ds, "reservations_table");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insert_button_Click(object sender, EventArgs e)
        {
                int amount;
            
                if ((string.IsNullOrWhiteSpace(txt_id_client.Text) ||
                string.IsNullOrWhiteSpace(txt_id_employee.Text) ||
                string.IsNullOrWhiteSpace(txt_id_room.Text)))
                {
                    MessageBox.Show("Въведете някакви данни в текстовата кутия !");
                }
                else if (int.TryParse(txt_id_client.Text, out amount) && amount <= 0 ||
                    int.TryParse(txt_id_employee.Text, out amount) && amount <= 0 ||
                    int.TryParse(txt_id_room.Text, out amount) && amount <= 0)
                {
                    MessageBox.Show("Моля въведете положително число !");
                }
                else if (dateTimePicker1.Value.Date >= dateTimePicker2.Value.Date)
                {
                    MessageBox.Show("ЕГН/Телефонен номер трябва да са поне 10 цифри!");
                }
                else
            {
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Hotel;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into reservations_table(id_client,id_employee,id_room,start_date,end_date) values ('" + txt_id_client.Text + "','" + txt_id_employee.Text + "','" + txt_id_room.Text + "','" + dateTimePicker1.Value.Date + "','" + dateTimePicker2.Value.Date + "')", con);
                
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    MessageBox.Show("Saved");

                }
                else
                {
                    MessageBox.Show("Error");
                }
                con.Close();

                ds = new System.Data.DataSet();
                adap.Fill(ds, "reservations_table");
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            try
            {
                cmdbl = new SqlCommandBuilder(adap);
                adap.Update(ds, "reservations_table");
                MessageBox.Show("Information Updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            SqlConnection con3 = new SqlConnection("Data Source=.;Initial Catalog=Hotel;Integrated Security=True");
            DialogResult del = MessageBox.Show("Are you Sure you want to Delete" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "Record", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (del == DialogResult.Yes)
            {
                con3.Open();

                SqlCommand cmd = new SqlCommand("DELETE from reservations_table WHERE (id_reservation='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "')", con3);
                int j = cmd.ExecuteNonQuery();
                if (j > 0)
                {
                    MessageBox.Show("Record Deleted Successfully!");


                }
                con3.Close();

                ds = new System.Data.DataSet();
                adap.Fill(ds, "reservations_table");
                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                this.Show();

            }
        }

        private void txt_id_client_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txt_id_employee_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txt_id_room_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }


    }
}
