namespace AuthAPI.Helpers
{
    public static class ActivateEmailBody
    {
        public static string ActivateEmailStringBody(string email, string emailtoken)
        {
            string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>

            </head>
            <body style=""font-family: Arial, sans-serif;
                        background-color: #f1f1f1;
                        margin: 0;
                        padding: 20px;"">
                <div style=""max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        border-radius: 5px;
                        padding: 20px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);"">
                    <h1 style=""color: #333333;
                        text-align: center;"">Activation Email</h1>
                    <p style=""color: #555555;
                        line-height: 1.4;"">Dear User,</p>
                    <p style=""color: #555555;
                        line-height: 1.4;"">Thank you for registering on our platform. To activate your account, please click the button below:</p>
                    <a style = ""display: inline-block;
                        background-color: #6A5ACD;
                        color: #ffffff;
                        text-decoration: none;
                        padding: 10px 20px;
                        margin-top: 20px;
                        border-radius: 4px;
                        transition: background-color 0.3s;"" href=""https://localhost:4200/unlock-session?email={email}&code={emailtoken}"">Activate Account</a>
                    <p style=""color: #555555;
                        line-height: 1.4;"">If the button doesn't work, you can also copy and paste the following link into your web browser:</p>
                    <p style=""color: #555555;
                        line-height: 1.4;"">https://localhost:4200/unlock-session?email={email}&code={emailtoken}</p>
                    <p style=""color: #555555;
                        line-height: 1.4;"">Thank you for joining us!</p>
                </div>
            </body>
            </html>
        ";


            return htmlBody;
        }
    }
}
