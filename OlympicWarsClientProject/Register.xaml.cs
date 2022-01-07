using ContractsOW;
using FluentValidation;
using FluentValidation.Results;
using Server;
using OlympicWarsClientProject.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ServiceModel;

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Lógica de interacción para Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private ServerPlayerProxy _serverPlayerProxy;
        private IPlayerService _playerServiceChannel;
        private Log log = new Log();

        public Register()
        {
            InitializeComponent();
        }

        private void Button_RegisterUser(object sender, RoutedEventArgs e)
        {
            //errors.Clear();
            PlayerContract player = new PlayerContract();
            string passwordEncrypt = EncryptPassword.SHA256(passwordBoxPassword.Password);

            player.nickName = textBoxNickname.Text;
            player.password = passwordEncrypt;
            player.email = textBoxEmail.Text;

            Validator validator = new Validator();
            FluentValidation.Results.ValidationResult results = validator.Validate(player);

            if (!results.IsValid)
            {
                foreach(var item in results.Errors)
                {
                    MessageBox.Show(item.ErrorMessage);
                }
            }
            else if (passwordBoxPasswordConfirm.Password == "")
            {
                MessageBox.Show("Confirm password!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (passwordBoxPassword.Password != passwordBoxPasswordConfirm.Password)
            {
                MessageBox.Show("Different password!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } 
            else
            {
                gridInputBox.Visibility = Visibility.Visible;
                string codeConfirmation = ConfirmationCodeForEmail();
                textBlockCodeConfirm.Text = codeConfirmation;
                sendEmail(codeConfirmation);
            }
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayerContract player = new PlayerContract();

                string passwordEncrypt = EncryptPassword.SHA256(passwordBoxPassword.Password);
                string codeConfirm = textBoxCodeCofirmation.Text;
                player.nickName = textBoxNickname.Text;
                player.password = passwordEncrypt;
                player.email = textBoxEmail.Text;

                if (codeConfirm == textBoxCodeCofirmation.Text)
                {
                    var playerCallback = new PlayerServiceCallback();
                    playerCallback.ConfirmRegistrationEvent += ConfirmRegistration;
                    _serverPlayerProxy = new ServerPlayerProxy(playerCallback);
                    _playerServiceChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                    _playerServiceChannel.RegisterUser(player);
                    textBoxCodeCofirmation.Text = "";
                    gridInputBox.Visibility = Visibility.Collapsed;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            gridInputBox.Visibility = Visibility.Collapsed;
        }

        private void ConfirmRegistration(int confirmPlayer)
        {
            if (confirmPlayer > 0)
            {
                MessageBox.Show("Successful registration!!!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.textBoxNickname.Text = "";
                this.passwordBoxPassword.Password = "";
                this.passwordBoxPasswordConfirm.Password = "";
                this.textBoxEmail.Text = "";
            }
            else
            {
                MessageBox.Show("Not succesfully registration");
            }
            
        }

        private void ButtonLogin_OpenWaitingRoom(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayerContract playerLogin = new PlayerContract();

                playerLogin.nickName = textBoxNicknameLogin.Text;
                playerLogin.password = EncryptPassword.SHA256(passwordBoxPasswordLogin.Password);

                ValidatorLogin validatorLogin = new ValidatorLogin();
                FluentValidation.Results.ValidationResult resultsLogin = validatorLogin.Validate(playerLogin);

                if (!resultsLogin.IsValid)
                {
                    foreach (var item in resultsLogin.Errors)
                    {
                        MessageBox.Show(item.ErrorMessage);
                    }
                }
                else if (EncryptPassword.SHA256(passwordBoxPasswordLogin.Password) == playerLogin.password)
                {
                    try
                    {
                        var playerCallback = new PlayerServiceCallback();
                        playerCallback.ConfirmLoginEvent += ConfirmLogin;
                        _serverPlayerProxy = new ServerPlayerProxy(playerCallback);
                        _playerServiceChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                        _playerServiceChannel.LoginUser(playerLogin);
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        MessageBox.Show("Error");
                        log.Add(ex.ToString());
                    }
                    catch (System.Data.Entity.Core.EntityException ex)
                    {
                        MessageBox.Show("Database error");
                        log.Add(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Username does not exist", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("User can't find");
            }     

        }

        private void ConfirmLogin(PlayerContract player, bool confirmLogin)
        {
            if (confirmLogin)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.LoadData(player);
                mainWindow.GetName(textBoxNicknameLogin.Text);
                mainWindow.ReceiveRequest();
                this.Close();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("It isn't possible to enter");
            }
        }

        private void sendEmail(string codeConfirmation)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(textBoxEmail.Text);
            message.Subject = "Welcome to Olympic Wars";
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            //message.Body = "Your registration has been successful. It's time to have fun in Olympic Wars. " + codeConfirm;
            message.Body = EmailBody(codeConfirmation);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.From = new System.Net.Mail.MailAddress("olympicwars@gmail.com");

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

            client.Credentials = new System.Net.NetworkCredential("olympicwars@gmail.com", "olympicwars21");
            client.Port = 587;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com"; //mail.dominio.com

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught in CreateMessageWithAttachment(): {0}",
            ex.ToString());
                //MessageBox.Show("Error sending");
            }
        }

        public string EmailBody(string codeConfirmation)
        {
            return
                "<!DOCTYPE html> " +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                "<head>" +
                    "<title>Email</title>" +
                "</head>" +
                "<body style=\"font-family:'Century Gothic'\">" +
                    "<h1 style=\"text-align:center;\"> " + "Welcome to Olympic Wars" + "</h1>" +
                    "<h2 style=\"font-size:14px;\">" +
                        "It's time to get into Greek mythology!" + "<br />" +
                        "Company : " + "OW" + "<br />" +
                        "Code confirm: " + codeConfirmation +
                    "</h2>" +
                    "<p>" + " " + "</p>" +
                "</body>" +
                "</html>";
        }

        public string ConfirmationCodeForEmail()
        {
            var resultString = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var charsarray = new char[8];

            var random = new Random();

            
            for (int i = 0; i < charsarray.Length; i++)
            {
                charsarray[i] = characters[random.Next(characters.Length)];
            }

            resultString = new String(charsarray);
          
            resultString = "#" + resultString;

            return resultString;
        }

        private void Button_SeeLogin(object sender, RoutedEventArgs e)
        {
            gridLogin.Visibility = Visibility.Visible;
            zeusLogin.Visibility = Visibility.Visible;
            poseidonRegister.Visibility = Visibility.Collapsed;
        }

        private void Button_SeeRegister(object sender, RoutedEventArgs e)
        {
            gridLogin.Visibility = Visibility.Hidden;
            zeusLogin.Visibility = Visibility.Collapsed;
            poseidonRegister.Visibility = Visibility.Visible;
        }

        private void Button_See_Password_Click(object sender, RoutedEventArgs e)
        {
            textBoxPassword.Text = passwordBoxPassword.Password;
            textBoxPassword.Visibility = Visibility.Visible;
            passwordBoxPassword.Visibility = Visibility.Hidden;
            buttonHidePassword.Visibility = Visibility.Visible;
            buttonSeePassword.Visibility = Visibility.Collapsed;
        }

        private void Button_Hide_Password_Click(object sender, RoutedEventArgs e)
        {
            passwordBoxPassword.Password = textBoxPassword.Text;
            passwordBoxPassword.Visibility = Visibility.Visible;
            textBoxPassword.Visibility = Visibility.Hidden;
            buttonSeePassword.Visibility = Visibility.Visible;
            buttonHidePassword.Visibility = Visibility.Collapsed;
        }

        private void Button_See_PasswordConfirm_Click(object sender, RoutedEventArgs e)
        {
            textBoxPasswordConfirm.Text = passwordBoxPasswordConfirm.Password;
            textBoxPasswordConfirm.Visibility = Visibility.Visible;
            passwordBoxPasswordConfirm.Visibility = Visibility.Hidden;
            buttonHidePasswordConfirm.Visibility = Visibility.Visible;
            buttonSeePasswordConfirm.Visibility = Visibility.Collapsed;
        }

        private void Button_Hide_PasswordConfirm_Click(object sender, RoutedEventArgs e)
        {
            passwordBoxPasswordConfirm.Password = textBoxPasswordConfirm.Text;
            passwordBoxPasswordConfirm.Visibility = Visibility.Visible;
            textBoxPasswordConfirm.Visibility = Visibility.Hidden;
            buttonSeePasswordConfirm.Visibility = Visibility.Visible;
            buttonHidePasswordConfirm.Visibility = Visibility.Collapsed;
        }

        private void Button_See_Password_Login_Click(object sender, RoutedEventArgs e)
        {
            textBoxPasswordLogin.Text = passwordBoxPasswordLogin.Password;
            textBoxPasswordLogin.Visibility = Visibility.Visible;
            passwordBoxPasswordLogin.Visibility = Visibility.Hidden;
            buttonHidePasswordLogin.Visibility = Visibility.Visible;
            buttonSeePasswordLogin.Visibility = Visibility.Collapsed;
        }

        private void Button_Hide_PasswordConfirm_Login_Click(object sender, RoutedEventArgs e)
        {
            passwordBoxPasswordLogin.Password = textBoxPasswordLogin.Text;
            passwordBoxPasswordLogin.Visibility = Visibility.Visible;
            textBoxPasswordLogin.Visibility = Visibility.Hidden;
            buttonSeePasswordLogin.Visibility = Visibility.Visible;
            buttonHidePasswordLogin.Visibility = Visibility.Collapsed;
        }
    }
}
