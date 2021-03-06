﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace ServerLib
{
    /// <summary>
    /// This is an abstract class for Servers of Echo type.
    /// </summary>
    public abstract class TextServer
    {
        #region Fields

        private bool running;

        private IPAddress ipAddress;
        private int port;
        private int bufferSize = 1024;
        
        private TcpListener tcpListener;
        private TcpClient tcpClient;

        #endregion

        #region Constructors
        /// <summary>
        /// A default constructor. It doesn't start the server. Invalid port numbers will thrown an exception.
        /// </summary>
        /// <param name="IP">IP address of the server instance.</param>
        /// <param name="port">Port number of the server instance.</param>
        public TextServer(IPAddress IP, int port)
        {
            running = false;
            IPAddress = IP;
            Port = port;

            if (!checkPort())
            {
                Port = 8000;
                throw new Exception("wrong port value, I set the port to 8000");
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// This function will return false if Port is set to a value lower than 1024 or higher than 49151.
        /// </summary>
        /// <returns>An information wether the set Port value is valid.</returns>
        protected bool checkPort()
        {
            if (port < 1024 || port > 49151) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This function fires off the default server behaviour. It interrupts the program.
        /// </summary>
        public abstract void Start();

        #endregion

        #region Properties
        /// <summary>
        /// This property gives access to the IP address of a server instance. Property can't be changed when the Server is running. 
        /// </summary>
        public IPAddress IPAddress
        {
            get => ipAddress;
            set
            {
                if (!running) ipAddress = value;
                else throw new Exception("the IP address cannot be changed while the server is running");
            }
        }
        /// <summary>
        /// This property gives access to the port of a server instance. Property can't be changed when the Server is running. Setting invalid port numbers will cause an exception. 
        /// </summary>

        public int Port
        {
            get => port;
            set
            {
                int tmp = port;

                if (!running) port = value;
                else throw new Exception("the port cannot be changed while the server is running");

                if (!checkPort())
                {
                    port = tmp;
                    throw new Exception("wrong port value");
                }
            }
        }

        /// <summary>
        /// This property gives access to the buffer size of a server instance. Property can't be changed when the Server is running. Setting invalid size numbers will cause an exception. 
        /// </summary>
        public int BufferSize
        {
            get => bufferSize;
            set
            {
                if (value < 0 || value > 1024 * 1024 * 64)
                {
                    throw new Exception("wrong package size");
                }

                if (!running) bufferSize = value;
                else throw new Exception("the packet size cannot be changed while the server is running");
            }
        }

        protected TcpListener TcpListener { get => tcpListener; set => tcpListener = value; }
        protected TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }

        #endregion
    }
}

