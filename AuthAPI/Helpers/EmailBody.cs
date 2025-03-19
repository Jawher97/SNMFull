using System.Drawing;

namespace AuthAPI.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailtoken)
        {
            return $@"<html>
                <head></head>
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
                      <div style= ""text-align: center;margin-bottom: 30px;"">
                        <img style= ""max-width: 150px;"" src=""https://example.com/logo.png"" alt=""Logo"">
                      </div>
                      <h1 style=""color: #333333;
                        text-align: center;"">Password Reset</h1>
                        <p style=""color: #555555;
                        line-height: 1.4;"">Hello,</p>
                        <p style=""color: #555555;
                        line-height: 1.4;"">You requested a password reset for your Social Network Sharing account. Click on the link down below to choose a new password:</p>
                         <a href=""https://localhost:4200/reset-password?email={email}&code={emailtoken}"" target=""_blank""                    
                           style = ""display: inline-block;
                        background-color: #6A5ACD;
                        color: #ffffff;
                        text-decoration: none;
                        padding: 10px 20px;
                        margin-top: 20px;
                        border-radius: 4px;
                        transition: background-color 0.3s;"">
                           Reset Password Link
                        </a>
                        <p style=""color: #555555;
                        line-height: 1.4;"">If you didn't request this reset, please ignore this email.</p>
                        <p style=""color: #555555;
                        line-height: 1.4;"">Thank you,<br>Kind Regards, SNM!</p>
  
                </div>
                </html>";
        }
    }
}

