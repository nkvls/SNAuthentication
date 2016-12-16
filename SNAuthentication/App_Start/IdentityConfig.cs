using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SNAuthentication.Models;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using SNAuthentication.Helper;
using SendGrid;
using System.Configuration;
using System.Net.Mime;

namespace SNAuthentication
{
    public class EmailService : IIdentityMessageService
    {
        ILog _logger = new Log(log4net.LogManager.GetLogger("SNLogging"));
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await configSendGridasync(message);

            //var mailMessage = new MailMessage
            //    ("newlifeawakeninguk@gmail.com", message.Destination, message.Subject, message.Body);

            //mailMessage.IsBodyHtml = true;

            
            //using (var client = new SmtpClient())
            //{
            //    var cred = new NetworkCredential
            //    {
            //        UserName = "newlifeawakeninguk@gmail.com",
            //        Password = "Nitin@001"
            //    };
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = cred;
            //    client.Host = "smtp.gmail.com";
            //    client.Port = 587;
            //    client.EnableSsl = true;
            //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    //client.Send(mailMessage);
            //    //var cred = new NetworkCredential
            //    //{
            //    //    UserName = "tonitinverma@hotmail.com",
            //    //    Password = "shysnazzy"
            //    //};
            //    //client.Credentials = cred;
            //    //client.Host = "smtp-mail.outlook.com";
            //    //client.Port = 587;
            //    //client.EnableSsl = true;
            //    _logger.Debug("IdentityConfig.SendAsync: Calling smpt client to send the mail");
            //    await client.SendMailAsync(mailMessage);
            //    _logger.Debug("IdentityConfig.SendAsync: Async mail send to the new user");
            //}
        }


        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "newlifeawakeninguk@gmail.com", "New Life Awakening Movement");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["mailAccount"],
                       ConfigurationManager.AppSettings["mailPassword"]
                       );

            // Create a Web transport for sending email.
            _logger.Debug("Creating transport web request.");
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                _logger.Debug("Sending web request for delivery");
                await transportWeb.DeliverAsync(myMessage);
                _logger.Debug("delivery done");
            }
            else
            {
                _logger.Error("IdentityConfig.configSendGridasync ERROR: Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }

        private async Task configSendGridasync1(IdentityMessage message)
        {
            try
            {
                var userNameFrom = ConfigurationManager.AppSettings["mailAccount"];
               var passwordFrom = ConfigurationManager.AppSettings["mailPassword"];

                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(message.Destination, message.Destination));

                // From
                mailMsg.From = new MailAddress(userNameFrom, userNameFrom);

                // Subject and multipart/alternative Body
                mailMsg.Subject = message.Subject;
                string text = message.Body;
                string html = message.Body;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userNameFrom, passwordFrom);
                smtpClient.Credentials = credentials;

                _logger.Debug("IdentityConfig.SendAsync: Calling smpt client to send the mail");
                //smtpClient.Send(mailMsg);
                await smtpClient.SendMailAsync(mailMsg);
                _logger.Debug("IdentityConfig.SendAsync: Async mail send to the new user");
            }
            catch (Exception ex)
            {
                _logger.Error("IdentityConfig.SendAsync: ERROR " + ex);
            }


            //var mailMessage = new MailMessage
            //    ("nitinv.verma@gmail.com", message.Destination, message.Subject, message.Body);

            //mailMessage.IsBodyHtml = true;

            //using (var client = new SmtpClient())
            //{
            //    await client.SendMailAsync(mailMessage);
            //}

            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            ////var message = new MailMessage();
            //message.To.Add(new MailAddress(""));  // replace with valid value 
            //message.From = new MailAddress("nitinv.verma@gmail.com");  // replace with valid value
            //message.Subject = "Your email subject";
            //message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            //message.IsBodyHtml = true;

            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = "user@outlook.com",  // replace with valid value
            //        Password = "password"  // replace with valid value
            //    };
            //    smtp.Credentials = credential;
            //    smtp.Host = "smtp-mail.outlook.com";
            //    smtp.Port = 587;
            //    smtp.EnableSsl = true;
            //    await smtp.SendMailAsync(message);
            //    return RedirectToAction("Sent");
            //}
        }
    }


    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
