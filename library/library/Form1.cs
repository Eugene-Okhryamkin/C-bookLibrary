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
using library.Properties;
using System.Data.SqlClient;

namespace library
{
    public partial class Form1 : Form
    {

        database DB = new database(); //Инициализируем класс database
        public OpenFileDialog OFD = new OpenFileDialog(); //Используем API Windows OpenFileDialog

        public Form1()
        {
            InitializeComponent();
        }

        private void addBook() //Добавить
        {
            OFD.Filter = "PDF files (*.pdf) | *.pdf"; //Фильтер на .PDF
            OFD.ShowDialog(); 
            string fileName = OFD.SafeFileName; //Путь к файлу
            string fileLink = OFD.FileName; //Имя файла
            if (checkedListBox1.Items.Contains(fileName) == true ) //Проверка на существование в библиотеке
            {
                MessageBox.Show("Книга существует в вашей библиотеке. Воспользуйтесь поиском");
            }
            else {
                DB.addToDB(fileName, fileLink); //Добавляем в базу и в checkListBox1
                checkedListBox1.Items.Insert(0, fileName);
            }
            
        }


        private void delBook()//Удаление
        {
            string bookName = string.Empty;
            foreach (string book in checkedListBox1.SelectedItems) { //Перебираем элементы checkedListBox1 
                bookName = Convert.ToString(book);
            }
            checkedListBox1.Items.Remove(bookName); //Удаление с checkedListBox1 
            DB.delFromDB(bookName); //Удаление с базы
        }


        public void checkedListBox1_SelectedIndexChanged (object sender, EventArgs e) {}

        private void checkedListBox1_DoubleClick(object sender, EventArgs e) { //Открываем файл
            string res = Convert.ToString(checkedListBox1.SelectedItem);
            DB.openFile(res);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addBook();
        }



        private void button2_Click(object sender, EventArgs e) 
        {
            delBook();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Settings.Default
            //Settings.Default.Save();
        }

        private async void Form1_Load(object sender, EventArgs e) //Ассинхронное обновление checkListBox 
        {
            string connection = @"Data Source=EUGENE;Initial Catalog=books;Integrated Security=True;Pooling=False";
            SqlConnection cnn = new SqlConnection(connection);

            await cnn.OpenAsync();
            SqlDataReader reader = null;

            string queryDB = "SELECT * FROM [Books]";
            SqlCommand cmd = new SqlCommand(queryDB, cnn);

            reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                checkedListBox1.Items.Insert(0, reader["bookName"]);
            }
        }

        private void searchBooks() //Поиск книги
        {
            int index = checkedListBox1.FindString(this.textBox1.Text); //Индексируем элементы
            if (0 <= index) //Ищем элемент по индексу
            {
                checkedListBox1.SelectedIndex = index;

            }
            if (this.textBox1.Text.Trim() == "")
                checkedListBox1.SelectedIndex = -1;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            searchBooks();
        }
    }
}
