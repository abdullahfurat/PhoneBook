using PhoneAppp.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PhoneAppp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PhoneBookEntities db = new PhoneBookEntities();


        void Clear()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                    item.Text = "";
            }
        }

        void KisiListesi()
        {
            lstKisiler.Items.Clear();
            var kisiler = db.People.ToList();

            foreach (Person person in kisiler)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.Phone);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id;

                lstKisiler.Items.Add(lvi);
            }
        }

        void KisiListesi(string param)
        {
            lstKisiler.Items.Clear();
            var kisiler = db.People.Where(x =>

             x.FirstName.Contains(param) ||
             x.LastName.Contains(param) ||
             x.Phone.Contains(param) ||
             x.Mail.Contains(param)

            ).ToList();

            foreach (Person person in kisiler)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.Phone);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id;

                lstKisiler.Items.Add(lvi);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            KisiListesi();
        }

        private void tsmYeniEkle_Click(object sender, EventArgs e)
        {
            txtAdi.Text = FakeData.NameData.GetFirstName();
            txtSoyadi.Text = FakeData.NameData.GetSurname();
            txtTelefon.Text = FakeData.PhoneNumberData.GetPhoneNumber();
            txtMail.Text = $"{txtAdi.Text}.{txtSoyadi.Text}@{FakeData.NameData.GetCompanyName()}.com".ToLower().Replace(" ", "");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Person person = new Person();
            person.FirstName = txtAdi.Text;
            person.LastName = txtSoyadi.Text;
            person.Mail = txtMail.Text;
            person.Phone = txtTelefon.Text;

            db.People.Add(person);
            bool result = db.SaveChanges() > 0;


            //int count = db.SaveChanges();
            //if (count > 0)
            //{
            //    result = true;
            //}
            //else
            //{
            //    result = false;
            //}

            result = 10 > 0;
            KisiListesi();
            Clear();

            MessageBox.Show(result ? "Kayıt Eklendi" : "İşlem Hatası");
        }

        private void tsmSil_Click(object sender, EventArgs e)
        {
            if (lstKisiler.SelectedItems.Count > 0)
            {

                DialogResult dr = MessageBox.Show("Kişiyi silmek istiyormusunuz?\nİşlem geri alınamaz!", "Kişi Silme Bildirimi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    int id = (int)lstKisiler.SelectedItems[0].Tag;
                    Person silinecek = db.People.Find(id);
                    db.People.Remove(silinecek);
                    db.SaveChanges();
                    KisiListesi();
                }
                else
                {
                    MessageBox.Show("Kayıt Silme İşlemi İptal Edildi!");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir kişi seçiniz!");
            }
        }



        Person guncellenecek;
        private void tsmDuzenle_Click(object sender, EventArgs e)
        {
            if (lstKisiler.SelectedItems.Count > 0)
            {
                int id = (int)lstKisiler.SelectedItems[0].Tag;
                guncellenecek = db.People.Find(id);  // Find metodu primary key değerine göre size o kişi teslim eder.
                txtAdi.Text = guncellenecek.FirstName;
                txtSoyadi.Text = guncellenecek.LastName;
                txtTelefon.Text = guncellenecek.Phone;
                txtMail.Text = guncellenecek.Mail;
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

            if (guncellenecek == null)
            {
                MessageBox.Show("Lütfen bir kayıt seçiniz!");
                return;
            }
            guncellenecek.FirstName = txtAdi.Text;
            guncellenecek.LastName = txtSoyadi.Text;
            guncellenecek.Mail = txtMail.Text;
            guncellenecek.Phone = txtTelefon.Text;

            db.SaveChanges();
            Clear();
            KisiListesi();
            guncellenecek = null;
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            KisiListesi(txtArama.Text);
        }
    }
}

