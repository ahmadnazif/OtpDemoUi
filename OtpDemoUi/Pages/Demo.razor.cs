using Microsoft.AspNetCore.Components;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtpDemoUi.Pages
{
    public class DemoBase : ComponentBase
    {
        protected string Issuer { get; set; }
        protected string Username { get; set; }
        protected string PrivateKey { get; set; }
        protected string OtpPath { get; set; }
        private Totp Totp { get; set; }
        protected string OutOtpString { get; set; }
        protected string InOtpString { get; set; }
        protected string Now { get; set; }
        protected string ResetIn { get; set; }
        protected bool IsStartup { get; set; } = true;
        protected bool IsValidOtp { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Issuer = "OTP Demo App";
                Username = "User1";
                var rkey = KeyGeneration.GenerateRandomKey(20);
                PrivateKey = Base32Encoding.ToString(rkey);
                OtpPath = $"otpauth://totp/{Issuer}:{Username}?secret={PrivateKey}&issuer={Issuer}";
                Totp = new Totp(rkey);
            }

            Now = DateTime.Now.ToLongTimeString();
            OutOtpString = Totp.ComputeTotp();
            ResetIn = Totp.RemainingSeconds().ToString();

            StateHasChanged();
        }

        protected void ValidateOtp()
        {
            IsStartup = false;
            IsValidOtp = Totp.VerifyTotp(InOtpString, out _);
        }

        protected MarkupString ValidationResult()
        {
            if (!IsStartup)
            {
                return IsValidOtp ?
                    new MarkupString("<label class=\"text-success\">OTP is valid</label>") :
                    new MarkupString("<label class=\"text-danger\">OTP is invalid</label>");
            }
            else return new MarkupString();
        }
    }
}
