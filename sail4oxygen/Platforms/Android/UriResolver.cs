using System;
using Android.Content;
using Java.IO;

//Android Specific code
//Copy a shared file to my local App storage space

namespace sail4oxygen.AndroidHelpers
{
	public static class UriResolver
	{
        public static System.Uri CopyFileFromUriToAppSpace(Context context, Android.Net.Uri uri)
        {
            string appFilePath = null;

            try
            {
                string scheme = uri.Scheme;

                if (ContentResolver.SchemeContent.Equals(scheme))
                {
#if DEBUG
                    System.Console.WriteLine("************ in first part resolver");
#endif
                    // Create an InputStream from the content URI
                    using (var inputStream = context.ContentResolver.OpenInputStream(uri))
                    {
                        // Create a temporary file in your app's private directory
                        var tempDir = context.GetExternalFilesDir(null);
                        var tempFile = new Java.IO.File(tempDir, "temp_file.csv");

                        // Copy the file from the InputStream to the temporary file
                        using (var outputStream = new FileOutputStream(tempFile))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = inputStream.Read(buffer)) > 0)
                            {
                                outputStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        System.Console.WriteLine("************ finished copy");

                        // Get the absolute path of the temporary file
                        appFilePath = tempFile.AbsolutePath;
                    }
                }
                else if (ContentResolver.SchemeFile.Equals(scheme))
                {
                    // Get the file path directly from the URI
                    appFilePath = uri.Path;
                }
                else if (ContentResolver.SchemeContent.Equals(scheme))
                {
                    // Handle other schemes as needed
                    // Additional logic may be required to handle specific URI schemes
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file copying
                System.Console.WriteLine($"Error copying file from URI to app space: {ex.Message}");
            }

            return new Uri(appFilePath);
        }
    }
}


