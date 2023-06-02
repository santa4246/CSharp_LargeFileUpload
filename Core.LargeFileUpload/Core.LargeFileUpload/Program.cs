using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.Core.LargeFileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            // Request Office365 site from the user
            string siteUrl = GetSite(); // url 입력

            /* Prompt for Credentials */
            Console.WriteLine("Enter credentials for {0}", siteUrl);

            string userName = GetUserName(); // user 계정 입력
            SecureString pwd = GetPassword(); // user 비밀번호 입력

            /* End Program if no Credentials */
            if (string.IsNullOrEmpty(userName) || (pwd == null))
                return;

            ClientContext ctx = new ClientContext(siteUrl);
            ctx.AuthenticationMode = ClientAuthenticationMode.Default; // Default : 0
            ctx.Credentials = new SharePointOnlineCredentials(userName, pwd); // SharePoint Online 리소스에 액세스하기 위한 자격 증명을 제공하는 개체
            Console.WriteLine(ctx.Credentials);
            Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory)); // C:\Users\mhkang\Desktop\largefile\Core.LargeFileUpload\Core.LargeFileUpload\bin\Debug\

            // First the failing part
            // try
            // {
            //     // Works for smaller files and will cause an exception now
            //     new FileUploadService().UploadDocumentContent(ctx, "Docs", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SP2013_LargeFile1.pptx"));
            // }
            // catch (Exception ex)
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine(string.Format("Exception while uploading file to the target site {0}.", ex.ToString()));
            //     Console.ForegroundColor = ConsoleColor.White;
            //     Console.WriteLine("Press enter to continue.");
            //     Console.Read();
            //     
            // }

            // These should both work as expected.
            try
            {
                // Alternative 1 for uploading large files 
                new FileUploadService().SaveBinaryDirect(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/rag-alpha.apk"));
                new FileUploadService().SaveBinaryDirect(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/rag-alpha.ipa"));
                new FileUploadService().SaveBinaryDirect(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/exe.zip"));
                new FileUploadService().SaveBinaryDirect(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/OneDrive_2023-06-02.zip"));
                new FileUploadService().SaveBinaryDirect(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/MU_Red_1_20_20_Full.exe"));

                // Alternative 2 for uploading large files
                // new FileUploadService().UploadDocumentContentStream(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rag-alpha.apk"));

                // Alternative 3 for uploading large files: slice per slice which allows you to stop and resume a download
                // new FileUploadService().UploadFileSlicePerSlice(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SP2013_LargeFile1.pptx"), 1);
                // new FileUploadService().UploadFileSlicePerSlice(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exe.zip"), 1);

                // new FileUploadService().UploadFileSlicePerSlice(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OneDrive_2023-06-02.zip"), 1);
                // new FileUploadService().UploadFileSlicePerSlice(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rag-alpha.apk"), 1);
                // new FileUploadService().UploadFileSlicePerSlice(ctx, "repository", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rag-alpha.ipa"), 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while uploading files t" +
                    "o the target site: {0}.", ex.ToString()));
                Console.WriteLine("Press enter to continue.");
                Console.Read();
            }
            // Just to see what we have in console
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("large files were uploaded to library. Press enter to continue.");
            Console.Read();
        }


        static SecureString GetPassword()
        {
            SecureString sStrPwd = new SecureString();
            try
            {
                Console.Write("Password: ");

                for (ConsoleKeyInfo keyInfo = Console.ReadKey(true); keyInfo.Key != ConsoleKey.Enter; keyInfo = Console.ReadKey(true))
                {
                    if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (sStrPwd.Length > 0)
                        {
                            sStrPwd.RemoveAt(sStrPwd.Length - 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                    else if (keyInfo.Key != ConsoleKey.Enter)
                    {
                        Console.Write("*");
                        sStrPwd.AppendChar(keyInfo.KeyChar);
                    }

                }
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                sStrPwd = null;
                Console.WriteLine(e.Message);
            }

            return sStrPwd;
        }

        static string GetUserName()
        {
            string strUserName = string.Empty;
            try
            {
                Console.Write("Username: ");
                strUserName = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                strUserName = string.Empty;
            }
            return strUserName;
        }

        static string GetSite()
        {
            string siteUrl = string.Empty;
            try
            {
                Console.Write("Enter your Office365 site collection URL: ");
                siteUrl = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                siteUrl = string.Empty;
            }
            return siteUrl;
        }
    }
   
}
