﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace CupCake.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string command = args[0].ToLower();

                if (command == "deploy")
                {
                    string cupCakePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CupCake";
                    if (!Directory.Exists(cupCakePath))
                        Directory.CreateDirectory(cupCakePath);

                    string profilesPath = cupCakePath + "\\Profiles";
                    if (!Directory.Exists(profilesPath))
                        Directory.CreateDirectory(profilesPath);

                    string debugProfilePath = profilesPath + "\\Debug";
                    if (Directory.Exists(debugProfilePath))
                        Directory.Delete(debugProfilePath, true);

                    Directory.CreateDirectory(debugProfilePath);

                    string sourcePath = String.Join(" ", args);

                    string[] files = Directory.GetFiles(sourcePath);

                    foreach (string s in files)
                    {
                        string fileName = Path.GetFileName(s);
                        string destFile = Path.Combine(debugProfilePath, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
                else if (command == "debug")
                {
                    string path;
                    try
                    {
                        using (var client = new TcpClient())
                        {
                            client.Connect(IPAddress.Loopback, 4570);

                            using (NetworkStream stream = client.GetStream())
                            using (var reader = new StreamReader(stream, Encoding.Unicode))
                            {
                                // Request Debug
                                stream.WriteByte(0xE1);

                                // Skip one byte
                                stream.ReadByte();
                                // Wait until the connection is closed by cupcake
                                path = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("Problem communicating with the client, make sure it is running.");
                        return;
                    }

                    AppDomain.CurrentDomain.ExecuteAssembly(path + "\\CupCake.Server.exe", new[] { "--envpath", path });
                }
                else
                {
                    Console.WriteLine("Invalid command specified.");
                }
            }
            else
            {
                Console.WriteLine("Please specify a command to run");
            }
        }
    }
}