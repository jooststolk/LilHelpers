# LilHelpers
Library with useful C# classes and thingies
------------------------------------------------------------------------------------------

# QRcode.cs - Modular helper class for generating, printing, and saving QR codes in WinForms apps. Uses QRCoder for generation and GDI+ for layout.
  
  This class needs the Nuget package QRCoder:
  .Net CLI: dotnet add package QRCoder --version 1.6.0
  or Package manager Console: Install-Package QRCoder -Version 1.6.0

## Features
- `createQRCode(string data)`: Generates a QR bitmap
- `Print(Image qrImage, string headerText)`: Prints QR with centered header
- `Save(Image qrImage, string folderPath, string fileName)`: Saves QR as JPEG

## Example Usage

```csharp
Bitmap qr = QRcode.createQRCode("Tool123");
QRcode.Print(qr, "Tool 123");
bool succes = QRcode.Save(qr, @"C:\Photos\Tool123", "QR.jpg");
