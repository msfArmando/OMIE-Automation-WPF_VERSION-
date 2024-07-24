using System.Net;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ClosedXML.Excel;
using System.IO;

namespace OMIE_Automation_WPF_VERSION_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var thisDirectory = Environment.CurrentDirectory;
            var fileName = "Permissions.xlsx";
            string tablePath = Path.Combine(thisDirectory, fileName);


            var xls = new XLWorkbook(tablePath);
            var table = xls.Worksheets.First(w => w.Name == "Planilha1");

            var tableNums = table.Rows().Count();


            for (int i = 2; i <= tableNums; i++)
            {
                ReturnData(table, i);
                var newDataOnList = new DataList { Id = Dado.Id, Nome = Dado.Nome};
                ListPermissions.Items.Add(newDataOnList);
            }
        }

        private static void ReturnData(IXLWorksheet planilha, int l)
        {
            Dado.Nome = planilha.Cell($"A{l}").Value.ToString();
            Dado.Id = planilha.Cell($"B{l}").Value.ToString();
        }

        private string ObterUsername()
        {
            return txtUsername.Text;
        }

        private string ObterNewUser()
        {
            return txtNewOmieUser.Text;
        }

        private string ObterPassword()
        {
            return txtPassword.Password;
        }

        private string ObterPermission()
        {
            return txtPermission.Text;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userLogin = ObterUsername();
                var passwordLogin = ObterPassword();
                var newUsernewUser = ObterNewUser();
                var idPermission = ObterPermission();

                Automations auto = new Automations();

                auto.GoToWebSite("https://app.omie.com.br/login/");

                auto.Login(userLogin, passwordLogin);

                auto.CadastrarUsuarios(newUsernewUser, idPermission);

                auto.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro na estapa {LOG.Text}: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public static class Dado
    {
        public static string Id { get; set; }
        public static string Nome { get; set; }
    }

    public class DataList
    {
        public string Id { get; set; }
        public string Nome { get; set; }
    }
}